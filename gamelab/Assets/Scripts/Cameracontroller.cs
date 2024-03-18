using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class Cameracontroller : MonoBehaviour
{
    public Sprite eyehud;
    public Sprite camhud;
    public GameObject hudindicator;
    public Camera camera;
    public GameObject esfera;
    public SteamVR_Input_Sources handTypeR;
    private bool isActive = false;
    public SteamVR_Action_Boolean input;
    public void ControlCamera()
    {
        if (isActive)
        {
            hudindicator.GetComponent<Image>().sprite = eyehud;
            esfera.SetActive(false);
            isActive = false;
            camera.enabled = false;
        }
        else
        {
            hudindicator.GetComponent<Image>().sprite = camhud;
            isActive = true;
            esfera.SetActive(true);
            camera.enabled = true;
        }
    }
    private void Update()
    {
        if (input.GetStateDown(handTypeR))
        {
            ControlCamera();
        }
        
    }
}
