using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempElementController : MonoBehaviour
{
    public GameObject element;
    private bool isActive = false;
    // Start is called before the first frame update
 

    // Update is called once per frame
   public void controlActivation()
    {
        Debug.Log("aqui");
        if (!isActive)
        {
            isActive = true;
            element.SetActive(true);
        }
        else
        {
            isActive = false;
            element.SetActive(false);
        }
    }
}
