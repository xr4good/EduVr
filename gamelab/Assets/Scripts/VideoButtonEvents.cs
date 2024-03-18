using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using UnityEngine.Video;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class VideoButtonEvents : MonoBehaviour
{
    //References
    public GameObject button;
    public GameObject videoScreen;
    public RenderTexture videoTexture;
    public Texture2D playTexture;
    public Texture2D pauseTexture;
    public VideoPlayer videoPlayer;
    private bool isFirstTime = true;

    void Start()
    {
     Debug.Log(GetDataAPI.videos[0]);
          videoPlayer.url = GetDataAPI.videos[0];
    }

  

    //Methods
    public void OnPress(Hand hand)
    {
        StartVideo();
    }

    public void StartVideo(){
        if (isFirstTime) {
            Debug.Log("CHANGE VIDEO");
            videoScreen.GetComponent<Renderer>().material.SetTexture("_MainTex", videoTexture);
            isFirstTime = false;
        }

        if (button.GetComponent<Renderer>().material.GetTexture("_MainTex") == playTexture)
        {
            button.GetComponent<Renderer>().material.SetTexture("_MainTex", pauseTexture);
            videoPlayer.Play();
        } else
        {
            button.GetComponent<Renderer>().material.SetTexture("_MainTex", playTexture);
            videoPlayer.Pause();
        }
    }
}