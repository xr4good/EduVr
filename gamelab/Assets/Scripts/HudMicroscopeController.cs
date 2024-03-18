using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudMicroscopeController : MonoBehaviour
{
    public GameObject hud;
    private bool isFirstTime = true;
        // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
 {
            if (other.gameObject.tag == "Player" && isFirstTime)
           {

                    hud.SetActive(true);
                    //or gameObject.SetActive(false);
           }
 }
  void OnTriggerExit (Collider other)
 {
            if (other.gameObject.tag == "Player" && isFirstTime)
           {
                    isFirstTime=false;
                    hud.SetActive(false);
                    //or gameObject.SetActive(false);
           }
 }
 public void ManualToggle (){
     if(hud.activeSelf){
        hud.SetActive(false);
     }else{
        hud.SetActive(true);
     }
 }
}
