using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

namespace SeriousGameComponents.SlideComponent
{
    [SelectionBase]
    public class SlideController : MonoBehaviour
    {
        [Tooltip("String that uniquely identifies this component for the LMS Loader. Leave empty to use the default identifier (WIP)")]
        [HideInInspector]
        public string identifier;

        [Tooltip("Flag to tell if the assets that this component will use are being loaded from the XR4GOOD LMS with the LMSLoader component")]
        [HideInInspector]
        public bool useLmsLoader;

        [Tooltip("UI Raw Image object that represents the screen that the slides will appear on")]
        [HideInInspector]
        public RawImage screen;

        [Tooltip("Slide that will be shown if any errors had ocurred during it's loading process")]
        [HideInInspector]
        public Texture2D errorSlide;

        [Serializable]
        public class SlideRegistry
        {

            /// <summary> Texture that is stored at this moment on this object.
            /// The value that's stored in the url variable needs to be referencing
            ///  this new image. Use this to set the value instead of _slide</summary>
            public Texture slide
            {

                get { return _slide; }
                set
                {
                    if (value != null)
                    {
                        this._slide = value;
                        this.referenceUrl = this.url;
                    }
                    else this._slide = null;
                }

            }

            /// <summary> URL that references the image currently stored in this object </summary>
            public string referenceUrl { get; private set; }

            /// <summary> Custom label for the slide </summary>
            [Tooltip("Custom label for the slide")]
            public string slideLabel;

            /// <summary>
            /// Local image slide to be showed. Use the "slide" property to set values. Setting images directly in this variable is not recommended
            /// </summary>
            [Tooltip("Local image file for the slide")]
            public Texture _slide;

            [Header("Web Slide Information")]

            /// <summary>
            /// Whether to use the url to download the slide from the web or to use the local file assigned to the slide field
            /// </summary>
            [Tooltip("Whether to use the url to download the slide from the web or to use the local file assigned to the slide field")]
            public bool useSlideFromWeb;
            [Space]

            /// <summary>
            /// If true, the URL defined in the "url" variable will use the urlPrefix string value as prefix
            /// </summary>
            [Tooltip("If checked, the URL in the field below will be concatenated after the determined prefix above")]
            public bool useURLPrefix = false;

            /// <summary> URL that references an image on the web. Not to be confused with referenceUrl </summary>
            [Tooltip("URL that references an image on the web.")]
            public string url;

            [HideInInspector]
            public string httpError = "";
            [HideInInspector]
            public bool downloading;
            [HideInInspector]
            public UnityWebRequest webRequest;

            /// <summary> Local empty registry </summary>
            public SlideRegistry()
            {
                url = "";
                slideLabel = "";
                slide = null;
                useSlideFromWeb = false;
            }

            /// <summary> Local empty registry with label</summary>
            public SlideRegistry(string label)
            {
                slide = null;
                slideLabel = label;
                url = "local";
                useSlideFromWeb = false;
            }

            /// <summary> Local registry with label </summary>
            public SlideRegistry(Texture slideImage, string label)
            {
                url = "local";
                slideLabel = label;
                slide = slideImage;
                useSlideFromWeb = false;
            }

            /// <summary> Web empty registry with label. </summary>
            public SlideRegistry(string label, string url)
            {
                this.url = url;
                slideLabel = label;
                slide = null;
                useSlideFromWeb = true;
            }

            /// <summary> Web registry with label. </summary>
            public SlideRegistry(Texture slideImage, string label, string referenceUrl)
            {
                this.url = referenceUrl;
                slideLabel = label;
                slide = slideImage;
                useSlideFromWeb = true;
            }
        }

        [Space]
        public string urlPrefix;

        public List<SlideRegistry> slideFiles;

        public int currentSlideIndex { get; private set; } = 0;
        public int slideCount { get; private set; } = 0;
        public bool isLastSlide { get; private set; }
        public bool isFirstSlide { get; private set; }

        void Start()
        {
            if(screen) screen.texture = null;
            if (slideFiles == null) slideFiles = new List<SlideRegistry>();
        }

        bool lmsDataLoaded = false;
        /// <summary>
        /// Load the downloaded data from the XR4GOOD LMS registered in the LMS Loader component. If the reload flag is set to false, the new slides will be appended to the slide list
        /// </summary>
        /// <param name="reload"></param>
        public void LoadDownloadedLMSData(bool reload = true)
        {
            if(!useLmsLoader)
            {
                Debug.LogWarning("This object " + gameObject.name + " has not been set to receive data from the XR4GOOD LMS. If you wish to call this method, set the useLmsData attribute value to true");
                return;
            }

            if (reload)
                slideFiles.Clear();

            if (LmsComponent.LmsLoader.slides == null ||
                LmsComponent.LmsLoader.slides.Count == 0)
            {
                return;
            }

            string identifier = this.identifier == "" ? LmsComponent.LmsLoader.DEFAULT_IDENTIFIER : this.identifier;

            int slideCount = 1;

            if(!LmsComponent.LmsLoader.slides.ContainsKey(identifier))
            {
                throw new KeyNotFoundException("Could not load data. \"" + identifier + "\" identifier is not registered in the LMS for this type of component");
            }

            foreach(Texture2D t in LmsComponent.LmsLoader.slides[identifier])
            {
                slideFiles.Add(new SlideRegistry(t, "slide_" + slideCount));
                slideCount++;
            }

            lmsDataLoaded = true;
        }


        private bool startedDownloadingAll = false;
        private void Update()
        {
            if(useLmsLoader && LmsComponent.LmsLoader.slidesFetched && !lmsDataLoaded)
            {
                LoadDownloadedLMSData();
            }

            isLastSlide = currentSlideIndex + 1 >= GetSlideCount();
            isFirstSlide = currentSlideIndex == 0;

            if (!errorSlide)
                errorSlide = new Texture2D(100, 50);

            if (slideFiles.Count > 0 && screen)
            {
                if (slideFiles[0].slide != null && currentSlideIndex == 0 && screen.texture == null)
                    ChangeCurrentSlide(0);
            }

            if (!startedDownloadingAll)
            {
                startedDownloadingAll = true;
                DownloadAllSlides();
            }
        }

        public bool IsSlideAvailable(int index)
        {
            if (slideFiles.Count == 0)
                return false;

            try
            {
                if (slideFiles[index].slide == null)
                    return false;
            }
            catch(ArgumentOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        public int GetSlideCount()
        {
            return slideFiles == null ? 0 : slideFiles.Count;
        }

        /// <summary>
        /// Download all slides that have been referenced by URLs in the component instead of images.
        /// </summary>
        public void DownloadAllSlides()
        {
            //UnityWebRequest www = UnityWebRequest.Get("https://kodagabriel.com.br/get_photos_tp.php");
            //yield return www.SendWebRequest();

            for (int i = 0; i < slideFiles.Count; i++)
            {
                StartCoroutine(DownloadSlide(i));
            }
        }

        IEnumerator DownloadSlide(int index)
        {
            if (!slideFiles[index].useSlideFromWeb)
                yield return null;
            else
            {
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(slideFiles[index].useURLPrefix ? urlPrefix + slideFiles[index].url : slideFiles[index].url))
                {
                    slideFiles[index].downloading = true;
                    slideFiles[index].webRequest = www;
                    slideFiles[index].httpError = "";

                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.ConnectionError ||
                        www.result == UnityWebRequest.Result.ProtocolError)
                    {
                        slideFiles[index].httpError = www.error;
                        Debug.LogWarning(www.error);
                    }
                    else
                        slideFiles[index].slide = DownloadHandlerTexture.GetContent(www);

                    slideFiles[index].downloading = false;
                    slideFiles[index].webRequest = null;

                    if (currentSlideIndex == index && screen)
                    {
                        ChangeCurrentSlide(currentSlideIndex);
                    }
                }
            }
        }

        public void ChangeCurrentSlide(int index)
        {
            currentSlideIndex = index;

            if(screen)
            {
                if (screen.texture != slideFiles[index].slide)
                {
                    screen.texture = slideFiles[index].slide != null ? slideFiles[index].slide : errorSlide != null? errorSlide : null;
                }
            }
        }

        public void NextSlide()
        {
            if (currentSlideIndex < slideFiles.Count - 1)
            {
                currentSlideIndex += 1;
                ChangeCurrentSlide(currentSlideIndex);
                //screen.GetComponent<Renderer>().material.mainTexture = slides[currentSlideIndex];
            }
        }
        public void PreviousSlide()
        {
            if (currentSlideIndex > 0)
            {
                currentSlideIndex -= 1;
                ChangeCurrentSlide(currentSlideIndex);
                //screen.GetComponent<Renderer>().material.mainTexture = slides[currentSlideIndex];
            }
        }


    }
}
