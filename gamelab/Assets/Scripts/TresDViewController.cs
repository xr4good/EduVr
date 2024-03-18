using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Valve.VR;


public class TresDViewController : MonoBehaviour
{
    public GameObject player;
    public GameObject sphere;
    public VideoPlayer videoSphere1;
    public GameObject restartObj;
    public GameObject playPauseObj;
    public GameObject exitObj;
    public GameObject playPauseWithTexture;
    public Texture2D playTexture;
    public Texture2D pauseTexture;

    // Start is called before the first frame update
    private Vector3 pastPosition;
    private Quaternion pastRotation;
    public void ActivateScene(string url)
    {
        
        StartCoroutine(fadeStart(url));
      
    }
    private IEnumerator fadeStart(string url)
    {
        SteamVR_Fade.View(Color.black, 1f);
        yield return new WaitForSeconds(2);
        restartObj.SetActive(true);
        playPauseObj.SetActive(true);
        exitObj.SetActive(true);
        videoSphere1.url = url;
        pastPosition = player.transform.position;
        pastRotation = player.transform.rotation;
        print(sphere.transform.position);
        Vector3 spherePosition = sphere.transform.position;
        Vector3 subtractPosition = new Vector3(0, -2.5f, 0);
        player.transform.position = spherePosition - subtractPosition;
        print(player.transform.position);
        changePlayPauseButtonTexture();
        SteamVR_Fade.View(Color.clear, 2f);
        videoSphere1.Play();
    }

    public void playPauseVideo()
    {
    
        if (videoSphere1.isPlaying) {
            videoSphere1.Pause();
        } else
        {
            videoSphere1.Play();
        }
        changePlayPauseButtonTexture();
    }

    private void changePlayPauseButtonTexture()
    {
        print(playPauseWithTexture.GetComponent<Renderer>().material.GetTexture("_MainTex"));
        if (playPauseWithTexture.GetComponent<Renderer>().material.GetTexture("_MainTex") == playTexture)
        {
            playPauseWithTexture.GetComponent<Renderer>().material.SetTexture("_MainTex", pauseTexture);
        }
        else
        {
            playPauseWithTexture.GetComponent<Renderer>().material.SetTexture("_MainTex", playTexture);
        }
    }

    public void restartVideo()
    {
        videoSphere1.Stop();
        videoSphere1.Play();         
    }

    public void exitFromVideo()
    {
        StartCoroutine(fadeForEnd());
    }
    private IEnumerator fadeForEnd()
    {
        SteamVR_Fade.View(Color.black, 1f);
        yield return new WaitForSeconds(2);
        restartObj.SetActive(false);
        playPauseObj.SetActive(false);
        exitObj.SetActive(false);
        player.transform.position = pastPosition;
        player.transform.rotation = pastRotation;
        SteamVR_Fade.View(Color.clear, 2f);
        videoSphere1.Stop();
    }


    // Update is called once per frame
    public void ExitScene()
    {
        player.transform.Translate(pastPosition, Space.World);

    }
}
