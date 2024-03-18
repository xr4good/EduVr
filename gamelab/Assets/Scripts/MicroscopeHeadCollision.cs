using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MicroscopeHeadCollision : MonoBehaviour
{
    public GameObject text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (other.CompareTag("MicroscopeCollider"))
        {
            if(QuizControl.quizCompleted){
                SceneManager.LoadScene("MicroscopeView");
            }else{
                text.SetActive(true);
            }
            text.SetActive(true);
        }
    }
}
