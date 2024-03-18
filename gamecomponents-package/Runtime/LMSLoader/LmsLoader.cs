using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using SeriousGameComponents.LmsComponent.Json;

//TODO - MAKE THE DOWNLOAD FUNCTIONS ASYCHRONOUS "async", TO AVOID GAME STUTTERING
//TODO - FETCH ONLY USER REGISTERED LESSONS (method IEnumerator GetContents())
//TODO - IMPLEMENT A PASSWORD AUTHENTICATION ON LMS. CURRENT IS JUST A GET WITH THE EMAIL (method IEnumerator VerifyLogin())
//TODO - FIND A WAY TO SUPPORT MORE THAN ONE QUIZ USING THE IDENTIFIER SYSTEM (method void GetQuizesFromLesson())
namespace SeriousGameComponents.LmsComponent
{
    /// <summary>
    /// API that handles communication with the XR4GOOD LMS and stores the data in C# classes through static attributes. This is a singleton, meaning that can exist only one LMSLoader in the scene. Any other will be automatically deleted
    /// </summary>
    public class LmsLoader : MonoBehaviour
    {
        public static LmsLoader singleton { get; private set; }

        public const string DEFAULT_IDENTIFIER = "default_id";

        [HideInInspector]
        public string username;
        [HideInInspector]
        public string password;

        /// <summary>
        /// Name of the LMS
        /// </summary>
        public string LMSName { get; } = "XR4GOOD LMS";
        /// <summary>
        /// Base URL of the LMS
        /// </summary>
        public string baseURL { get; } = "http://200.239.128.208/";
        /// <summary>
        /// URL used for the login check on the LMS after a request
        /// </summary>
        public string loginCheck { get; } = "lms/wp-json/lesson_for_game/v1/check-email/";
        /// <summary>
        /// URL that stores the json containing the user's lessons information
        /// </summary>
        public string lessons { get; } = "lms/wp-json/lesson_for_game/v1/list-lessons";

        /// <summary>
        /// The content from the json specifying the lessons data registered in the LMS for this user*
        /// </summary>
        [HideInInspector]
        public Lessons content;


        //STATIC ATTRIBUTES

        /// <summary>
        /// Registry to store the slides for a component. The key is the slide component identifier and the value are the images that will be shown.
        /// </summary>
        public static Dictionary<string, List<Texture2D>> slides;
        /// <summary>
        /// Registry to store a quiz for a component. The key is the quiz component identifier and the value are the images that will be shown.
        /// </summary>
        public static Dictionary<string, Quiz> quizes;
        /// <summary>
        /// Selected lesson description text
        /// </summary>
        public static string description; //post-title
        /// <summary>
        /// Selected lesson title
        /// </summary>
        public static string title; //post-content

        //Download flags:

        /// <summary>
        /// If the download process for the slides has finished
        /// </summary>
        public static bool slidesFetched { get; private set; } = false;
        /// <summary>
        /// If the download process for the quiz has finished
        /// </summary>
        public static bool quizFetched { get; private set; } = false;
        /// <summary>
        /// If the download process for the videos has finished (WIP)
        /// </summary>
        public static bool videosFetched { get; private set; } = false;
        /// <summary>
        /// If the download process for the audios has finished (WIP)
        /// </summary>
        public static bool audiosFetched { get; private set; } = false;

        
        public List<string> lessonList;

        void Start()
        {
            if (slides == null) slides = new Dictionary<string, List<Texture2D>>();
            if (quizes == null) quizes = new Dictionary<string, Quiz>();
            if (lessonList == null) lessonList = new List<string>();
        }

        int fetchContentsTries = 0;
        int maximumRequestTries = 5;
        void Update()
        {
            if (singleton == null)
                singleton = this;

            if (singleton != this)
                Destroy(this);

            bool cfetched;
            try
            {
                cfetched = ContentIsFetched();
            }
            catch(Exception)
            {
                cfetched = false;
            }

            if (loginSuccessful && !cfetched && !fetchingContent && fetchContentsTries < maximumRequestTries)
            {
                if (!cfetched)
                    fetchContentsTries++;

                GetContent();
            }

            if (cfetched)
                fetchContentsTries = 0;
        }

        public void SubmitLogin(string user, string password = "")
        {
            StartCoroutine(VerifyLogin(user, password));
        }

        /// <summary>
        /// Checks if the user is logged in. If any errors occurred during the HTTP request for login, this will raise an UnauthorizedAccessException
        /// </summary>
        /// <returns>Boolean value saying if the user is logged in or not</returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public bool IsLoggedIn()
        {
            if (!loginSuccessful && loginException != null)
                throw loginException;

            return loginSuccessful;
        }

        public bool loginSuccessful { get; private set; } = false;
        public Exception loginException { get; private set; }
        /// <summary>
        /// If the API is trying to log in to the LMS. In other words, if the API is waiting for the request of login to have a response
        /// </summary>
        public bool loggingIn { get; private set; } = false;
        IEnumerator VerifyLogin(string user, string password = "")
        {
            //TODO - IMPLEMENT A PASSWORD AUTHENTICATION ON LMS. CURRENT IS JUST A GET WITH THE EMAIL

            loggingIn = true;
            loginSuccessful = false;
            loginException = null;
            fetchContentsTries = 0;

            UnityWebRequest www = UnityWebRequest.Get(baseURL + loginCheck + "/" + user);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                loginException = new UnauthorizedAccessException("Login to LMS failed.\n HTTP Error: " + www.error);
            }
            else
                loginSuccessful = www.downloadHandler.text.Equals("true");

            loginSuccessful = true;

            loggingIn = false;
        }

        /// <summary>
        /// Download all lesson assets such as Slides, Audios (WIP), Videos (WIP), Quizes, etc. as well as storing the lesson information in the static attributes.
        /// Call this after the json containing the metadata has been fetched
        /// </summary>
        /// <param name="lessonIndex"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public void DownloadLessonAssets(int lessonIndex)
        {
            Lesson l;
            try
            {
                l = content.lessons[lessonIndex];
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Content not yet fetched. Download the lessons json using the GetContents method first");
            }

            DownloadLessonAssets(l);
        }

        /// <summary>
        /// Download all lesson assets such as Slides, Audios (WIP), Videos (WIP), Quizes, etc. as well as storing the lesson information in the static attributes.
        /// Call this after the json containing the metadata has been fetched
        /// </summary>
        /// <param name="l"></param>
        public void DownloadLessonAssets(Lesson l)
        {
            LmsLoader.description = l.post.post_content;
            LmsLoader.title = l.post.post_title;
            StartCoroutine(GetSlidesFromLesson(l));
            GetQuizesFromLesson(l);
        }

        /// <summary>
        /// Start downloading the json that holds the contents for the lessons registered to the logged user
        /// </summary>
        /// <param name="force"></param>
        public void GetContent(bool force = false)
        {
            if (!fetchingContent || force)
            {
                StopCoroutine(GetContents());
                StartCoroutine(GetContents());
            }
        }

        /// <summary>
        /// Method to check if the content json has already been fetched from the url specified in the lessons variable. Throws an exception if any HTTP error occurred in the request
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <returns>Boolean specifying if content has been successfully fetched from the link</returns>
        public bool ContentIsFetched()
        {
            if (fetchingContentException != null)
                throw fetchingContentException;

            return !fetchingContent && contentFetched;
        }

        /// <summary>
        /// If the API is trying to fetch the json content from the LMS. In other words, if the API is waiting for the request of get contents to have a response
        /// </summary>
        public bool fetchingContent { get; private set; } = false;
        public Exception fetchingContentException { get; private set; }
        public bool contentFetched { get; private set; } = false;
        IEnumerator GetContents()
        {
            contentFetched = false;
            fetchingContent = true;
            fetchingContentException = null;
            lessonList.Clear();

            string url = baseURL + lessons;

            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                fetchingContent = false;
                fetchingContentException = new KeyNotFoundException("Could not fetch json content from the link " + url + ".\nHTTP error: " + www.error);
            }
            else
            {
                //TODO - FETCH ONLY USER REGISTERED LESSONS
                content = JsonUtility.FromJson<Lessons>(
                    "{\"lessons\":" + www.downloadHandler.text + "}"
                );

                foreach(Lesson l in content.lessons)
                {
                    lessonList.Add(l.post.post_title);
                }

                fetchingContent = false;
                contentFetched = true;
            }
        }


        /// <summary>
        /// Number of slides registered for this lesson, from all slide components
        /// </summary>
        public int numberOfSlidesFromLesson { get; private set; } = 0;
        /// <summary>
        /// Number of slides that has been already iterated from the json, being successfully downloaded or not. If this is equal to numberOfSlidesFromLesson, then all the slides were iterated and are now stored
        /// </summary>
        public int slidesDownloaded { get; private set; } = 0;
        IEnumerator GetSlidesFromLesson(int lessonIndex)
        {
            return GetSlidesFromLesson(content.lessons[lessonIndex]);
        }
        IEnumerator GetSlidesFromLesson(Lesson l)
        {
            slides.Clear();
            slidesDownloaded = 0;
            numberOfSlidesFromLesson = 0;
            slidesFetched = false;

            string images_str = l.post.metadata.aoc_image_image;
            if (images_str == "")
            {
                //TODO - CANT FETCH SLIDES, NO IMAGE REFERENCES AT JSON?
                Debug.LogWarning("Can't fetch slides, no image references at JSON?");
            }
            else
            {
                char[] cs = { ';' };
                string[] images = images_str.Split(cs, StringSplitOptions.RemoveEmptyEntries);

                numberOfSlidesFromLesson = images.Length;

                foreach (string imageUrl in images)
                {
                    UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
                    yield return request.SendWebRequest();
                    Texture2D slideTexture = null;

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        slideTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    }

                    string componentId;

                    //if ( don't have identifier ), then just add a default id to the dictionary key
                    {
                        componentId = DEFAULT_IDENTIFIER;
                    }
                    //else
                    //{
                    //  componentId = id in json;
                    //}


                    if (!slides.ContainsKey(componentId))
                    {
                        slides.Add(componentId, new List<Texture2D>());
                    }

                    //Adds the slide texture to the registry. If texture is null, then it has ocurred an error
                    slides[componentId].Add(slideTexture);
                    slidesDownloaded++;
                }
            }
            slidesFetched = true;
        }

        void GetQuizesFromLesson(int lessonIndex)
        {
            GetQuizesFromLesson(content.lessons[lessonIndex]);
        }
        void GetQuizesFromLesson(Lesson l)
        {
            quizes.Clear();
            quizFetched = false;
            //TODO - FIND A WAY TO SUPPORT MORE THAN ONE QUIZ USING THE IDENTIFIER SYSTEM

            if (l.quiz == null)
            {
                //TODO - CANT FETCH QUIZ, NO REFERENCES AT JSON?
            }
            else
            {
                List<Quiz> quizes = new List<Quiz>(); //Delete this line after quiz list is implemented on json
                quizes.Add(l.quiz); //Delete this line after quiz list is implemented on json

                //List<Quiz> quizes = new List<Quiz>(l.quiz); // <- Use this after quiz list is implemented on json


                //TODO - Add a form to identify which quiz component will be using these questions using a new identifier field in the json

                string componentId;

                //if ( don't have identifier ), then just add a default id to the dictionary key
                {
                    componentId = LmsLoader.DEFAULT_IDENTIFIER;
                }

                foreach (Quiz q in quizes)
                {
                    LmsLoader.quizes.Add(componentId, q);
                }
            }

            quizFetched = true;
        }

        IEnumerator GetAudiosFromLesson(Lesson l)
        {
            audiosFetched = false;
            throw new NotImplementedException("Audios are not yet implemented on json. Standing by implementation on the API");
            //audiosFetched = true;
        }

        IEnumerator GetVideosFromLesson(Lesson l)
        {
            videosFetched = false;
            throw new NotImplementedException("Videos are not yet implemented on the game components package. Standing by implementation on the API");
            //videosFetched = true;
        }

        /// <summary>
        /// Get the download progress from all assets being downloaded
        /// </summary>
        /// <returns>A number between 0-100 specifying the progress</returns>
        public float GetAssetDownloadPercentage()
        {
            try
            {
                if (!ContentIsFetched())
                    return 0f;
            }
            catch(Exception)
            {
                return 0f;
            }

            int progress =
                slidesDownloaded; //+
                                  //  audiosDownloaded + //WIP
                                  //  videosDownloaded; //WIP

            int goal =
                numberOfSlidesFromLesson; //+
                                          //  numberOfAudiosFromLesson + //WIP
                                          //  numberOfVideosFromLesson; //WIP

            if (goal == 0)
                return 0f;

            return 100.0f * progress / (goal * 1.0f);
        }

    }
}
