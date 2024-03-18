using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartBoard : MonoBehaviour
{
    public GameObject title;
    public GameObject content;
    // Start is called before the first frame update
    void Start()
    {
        if(GetDataAPI.title!=null){
            title.GetComponent<TMPro.TextMeshPro>().text = GetDataAPI.title;
        }else{
            title.GetComponent<TMPro.TextMeshPro>().text = "";
        }
        if(GetDataAPI.contentText!=null){
            content.GetComponent<TMPro.TextMeshPro>().text = GetDataAPI.contentText;
        }else{
            content.GetComponent<TMPro.TextMeshPro>().text = "";
        }
    }

  
}
