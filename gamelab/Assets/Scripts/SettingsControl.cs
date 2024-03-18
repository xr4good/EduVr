using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsControl : MonoBehaviour
{
    public GameObject settingsScreen;
    public GameObject settingsIcon;
    public GameObject toggle;
    // Start is called before the first frame update
    public void enableSettings(){
        if(settingsIcon.activeSelf){
            settingsIcon.SetActive(false);
        }else{
            settingsIcon.SetActive(true);
        }
    }
    public void openSettings(){
        settingsScreen.SetActive(true);
    }
    public void closeSettings(){
        settingsScreen.SetActive(false);
    }
}
