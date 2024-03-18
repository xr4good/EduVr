using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using UnityEngine.Video;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MicroscopeQuizButton : MonoBehaviour
{
        public GameObject screen;
    private bool isFirstTime = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public void OnPress()
    {
        if (isFirstTime) {
            screen.SetActive(true);
            isFirstTime = false;
        }else{
            screen.SetActive(false);
            isFirstTime = true;
        }

       
    }
}
