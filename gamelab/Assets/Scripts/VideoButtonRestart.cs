using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using UnityEngine.Video;
public class VideoButtonRestart : MonoBehaviour
{
    //References
    public GameObject button;
    public GameObject videoScreen;
    public RenderTexture videoTexture;
    public VideoPlayer videoPlayer;
    public Texture2D playTexture;
    private bool isFirstTime = true;

    //Methods
    public void OnPress(Hand hand)
    {
        if (isFirstTime)
        {
            videoScreen.GetComponent<Renderer>().material.SetTexture("_MainTex", videoTexture);
            isFirstTime = false;
        }

  
            videoPlayer.Stop();
            videoPlayer.Play();
            button.GetComponent<Renderer>().material.SetTexture("_MainTex", playTexture);

    }
}