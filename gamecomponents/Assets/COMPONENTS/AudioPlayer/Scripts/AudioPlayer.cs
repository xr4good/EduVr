using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace AudioPlayerComponent
{
    [SelectionBase]
    public class AudioPlayer : MonoBehaviour
    {
        public AudioClip audioClip;

        //TODO - Find a way to dinamicaly download new urls if added to the list through the inspector

        public AudioSource audioSource;
        public Transform progressBar;
        
        [Serializable]
        public class AudioFilesRegistry
        {
            public string audioLabel;
            /// <summary>
            /// Local audio clip to be played. Use the "audioClip" property to set values. Setting audio clips directly in this variable is not recommended
            /// </summary>
            [Tooltip("Local audio clip to be played")]
            public AudioClip _audioClip;

            [Header("Audios from web")]

            /// <summary>
            /// Whether the audio origin is local or from the web
            /// </summary>
            [Tooltip("Whether the audio origin is local or from the web")]
            public bool useAudioFromWeb;
            

            /// <summary> AudioClip that is stored at this moment on this object.
            /// The value that's stored in the url and audioType variables needs to be referencing
            ///  this new audio clip. Use this to set the value instead of _audioClip</summary>
            public AudioClip audioClip {

                get { return _audioClip; }
                set
                {
                    if (value != null)
                    {
                        this._audioClip = value;
                        this.referenceUrl = this.url;
                        this.referenceAudioType = this.audioType;
                    }
                    else this._audioClip = null;
                }

            }

            /// <summary> URL that references the audio clip currently stored in this object </summary>
            public string referenceUrl { get; private set; }

            /// <summary> Type of the audio clip currently stored in this object </summary>
            public AudioType referenceAudioType { get; private set; }

            [Space]
            /// <summary> URL that references an audio clip on the web. Not to be confused with referenceUrl </summary>
            [Tooltip("URL that references an audio clip on the web")]
            public string url;

            /// <summary> Type that references an audio clip on the web. Not to be confused with referenceAudioType </summary>
            [Tooltip("Type of the audio clip that will be downloaded from the web")]
            public AudioType audioType;

            [HideInInspector]
            public string httpError = "";
            [HideInInspector]
            public bool downloading;
            [HideInInspector]
            public UnityWebRequest webRequest;

            /// <summary> Local empty registry </summary>
            public AudioFilesRegistry()
            {
                url = "";
                audioLabel = "";
                audioType = AudioType.UNKNOWN;
                audioClip = null;
            }

            /// <summary> Local empty registry with label</summary>
            public AudioFilesRegistry(string label)
            {
                audioClip = null;
                audioLabel = label;
                audioType = AudioType.UNKNOWN;
                url = "local";
            }

            /// <summary> Local registry with label </summary>
            public AudioFilesRegistry(AudioClip clip, string label)
            {
                url = "local";
                audioLabel = label;
                audioType = AudioType.UNKNOWN;
                audioClip = clip;
            }

            /// <summary> Web empty registry with label. Must specify audio type for non-local registries </summary>
            public AudioFilesRegistry(string label, string url, AudioType type)
            {
                this.url = url;
                audioLabel = label;
                audioType = type;
                audioClip = null;
            }

            /// <summary> Web registry with label. Must specify audio type for non-local registries </summary>
            public AudioFilesRegistry(AudioClip clip, string label, string referenceUrl, AudioType referenceType)
            {
                this.url = referenceUrl;
                audioLabel = label;
                audioType = referenceType;
                audioClip = clip;
            }
        }

        public List<AudioFilesRegistry> audioFiles;


        void Start()
        {
            audioSource.clip = null;
        }

        public bool audioIsPlaying { get; private set; } = false;
        public bool startedAudio { get; private set; } = false;
        public int currentAudioIndex { get; private set; } = 0;

        public bool isLastAudio { get; private set; }
        public bool isFirstAudio { get; private set; }

        private bool startedDownloadingAll = false;
        void Update()
        {
            isLastAudio = currentAudioIndex + 1 >= GetAudioCount();
            isFirstAudio = currentAudioIndex == 0;

            if (audioFiles.Count > 0 && audioSource)
            {
                if (audioFiles[0].audioClip != null && currentAudioIndex == 0 && audioSource.clip == null)
                    ChangeCurrentAudioClip(0);
            }

            if (!startedDownloadingAll)
            {
                startedDownloadingAll = true;
                DownloadAllAudioClips();
            }

            if(progressBar && audioSource && audioFiles.Count > 0)
            {
                if(audioSource.clip != null)
                    progressBar.localScale = new Vector3(audioSource.time / audioSource.clip.length, progressBar.localScale.y, progressBar.localScale.z);
            }

            if (!audioIsPlaying && audioFiles.Count > 0 && audioSource)
                if (audioSource.time < 0.01f)
                    startedAudio = false;

        }

        public int GetAudioCount()
        {
            return audioFiles == null ? 0 : audioFiles.Count;
        }

        public bool IsDownloading()
        {
            foreach(AudioFilesRegistry afls in audioFiles)
            {
                if (afls.downloading)
                    return true;
            }

            return false;
        }

        public bool IsDownloading(int index)
        {
            return audioFiles[index].downloading;
        }

        public bool IsDownloading(string label)
        {
            return GetAudioByLabel(label).downloading;
        }

        public void DownloadAllAudioClips()
        {
            for(int i=0;i<audioFiles.Count;i++)
            {
                StartCoroutine(DownloadAudioClip(i));
            }
        }

        public void DownloadNewAudios()
        {
            for (int i = 0; i < audioFiles.Count; i++)
            {
                if(!IsDownloaded(i))
                    StartCoroutine(DownloadAudioClip(i));
            }
        }

        public void CancelAllDownloads()
        {
            StopAllCoroutines();
            foreach(AudioFilesRegistry afls in audioFiles)
            {
                if(afls.downloading)
                {
                    afls.downloading = false;
                    afls.webRequest = null;
                }
            }
        }

        public IEnumerator DownloadAudioClip(int index)
        {
            if (!audioFiles[index].useAudioFromWeb)
                yield return null;
            else
            {
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioFiles[index].url, audioFiles[index].audioType))
                {
                    //AudioClipUrls aux = audioFiles[index];
                    audioFiles[index].downloading = true;
                    audioFiles[index].webRequest = www;
                    audioFiles[index].httpError = "";

                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.ConnectionError)
                    {
                        audioFiles[index].httpError = www.error;

                        Debug.LogError(www.error);

                    }
                    else
                        audioFiles[index].audioClip = DownloadHandlerAudioClip.GetContent(www);

                    audioFiles[index].downloading = false;
                    audioFiles[index].webRequest = null;

                    if (currentAudioIndex == index && audioSource)
                        ChangeCurrentAudioClip(currentAudioIndex);
                }
            }
        }

        public float GetDownloadProgress(int index)
        {
            if(audioFiles[index].webRequest != null)
            {
                return audioFiles[index].webRequest.downloadProgress * 100f;
            }

            return IsDownloaded(index)? 100f : 0f;
        }

        public bool IsDownloaded(int index)
        {
            if (audioFiles[index].audioClip != null && audioFiles[index].referenceUrl == audioFiles[index].url ||
                !audioFiles[index].useAudioFromWeb)
                return true;

            return false;
        }

        public bool IsDownloaded(string label)
        {
            return IsDownloaded(GetAudioIndexByLabel(label));
        }

        public bool HasUndownloadedAudios()
        {
            for(int i=0;i<audioFiles.Count;i++)
            {
                if (!IsDownloaded(i))
                    return true;
            }

            return false;
        }

        public bool IsAudioAvailable(int index)
        {
            if (audioFiles.Count == 0)
                return false;

            if (audioFiles[index].audioClip == null)
                return false;

            return true;
        }

        public void ChangeCurrentAudioClip(int index)
        {
            if (audioSource)
            {
                if (audioSource.clip != audioFiles[index].audioClip)
                {
                    audioSource.clip = audioFiles[index].audioClip;
                    PauseAudio();
                }
            }
        }

        private bool CheckAudioFile()
        {
            if (audioFiles.Count == 0)
            {
                Debug.LogError("Audio is not available");
                return false;
            }

            return true;
        }

        public void ResumeAudio()
        {
            if (!CheckAudioFile()) return;

            if (audioSource.clip == null)
                return;

            if (!audioSource.isPlaying)
                audioSource.Play();

            startedAudio = true;
            audioIsPlaying = true;
        }

        public void PauseAudio()
        {
            if (!CheckAudioFile()) return;

            if (audioSource.clip == null)
                return;

            if (audioSource.isPlaying)
                audioSource.Pause();

            audioIsPlaying = false;
        }

        public void StopAudio()
        {
            if (!CheckAudioFile()) return;

            if (audioSource.clip == null)
                return;

            audioSource.Stop();
            startedAudio = false;
            audioIsPlaying = false;
        }

        //Get to next or previous audio in the list. Default is to pause the audio after changing
        //If you don't want the audio to auto-pause, just call the ResumeAudio function afterwards
        public void NextAudio()
        {
            if (currentAudioIndex >= audioFiles.Count-1)
            {
                currentAudioIndex = audioFiles.Count - 1;
                Debug.LogWarning("Next audio doesn't exist!");
                return;
            }

            currentAudioIndex++;
            ChangeCurrentAudioClip(currentAudioIndex);
        }

        public void PreviousAudio()
        {
            if (currentAudioIndex <= 0)
            {
                currentAudioIndex = 0;
                Debug.LogWarning("Previous audio doesn't exist!");
                return;
            }

            currentAudioIndex--;
            ChangeCurrentAudioClip(currentAudioIndex);
        }

        public AudioFilesRegistry GetAudioByLabel(string label)
        {
            foreach(AudioFilesRegistry acurl in audioFiles)
            {
                if (acurl.audioLabel == label)
                    return acurl;
            }

            throw new NullReferenceException("Specified audio label: " + label + " does not exist in the dictionary");
        }

        public int GetAudioIndexByLabel(string label)
        {
            for (int i=0;i<audioFiles.Count;i++)
            {
                if (audioFiles[i].audioLabel == label)
                    return i;
            }

            return -1;
        }
    }
}
