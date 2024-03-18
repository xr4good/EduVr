using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CheckHeadset : MonoBehaviour
{
    public SteamVR_Action_Boolean headsetOnHead = SteamVR_Input.GetBooleanAction("HeadsetOnHead");
    public GameObject panel;
    public GameObject playerVR;
    public GameObject playerNonVRCamera;
    public GameObject hero;
    public GameObject ui2D;
    // Start is called before the first frame update
    void Start()
    {
        if(UserLoginData.usingHeadset){
            playerNonVRCamera.SetActive(false);
            hero.SetActive(false);
            ui2D.SetActive(false);
            if(headsetOnHead.GetState(SteamVR_Input_Sources.Head)==false)   {
                    Debug.Log("SEM OCULOS");
                } 
        }else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerVR.SetActive(false);
            panel.SetActive(false);
        }
   
    }

    // Update is called once per frame
    void Update()
    {
        if(UserLoginData.usingHeadset){
          headsetOnHead = SteamVR_Input.GetBooleanAction("HeadsetOnHead");
        if(headsetOnHead.GetState(SteamVR_Input_Sources.Head))   {
        Debug.Log("COM OCULOS");
        panel.SetActive(false);
    }  
        
        }
            

        
    }
}
