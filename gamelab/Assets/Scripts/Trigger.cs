using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("É TETRA");
    }

    private void OnTriggerExit(Collider other)
    {
        print("SAIU");
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("COLIDIU");
    }

    private void OnCollisionExit(Collision collision)
    {
        print("SAIU DA COLISAO");
    }
}
