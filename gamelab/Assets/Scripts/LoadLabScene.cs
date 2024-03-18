 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.SceneManagement;
 using UnityEngine.UI;

public class LoadLabScene : MonoBehaviour
{
    public static Text emailText;
    public static GameObject inGameToggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void openLab(){
        UserLoginData.email = emailText.text.ToString();
        if(inGameToggle.GetComponent<Toggle>().isOn == true){
                UserLoginData.usingHeadset = true;
               UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
 
              UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.StartSubsystems();
        }else{
            UserLoginData.usingHeadset = false;
        }
        SceneManager.LoadScene("Laboratory");
    }
}
