using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        print("colidiu");
        if(collision.gameObject.tag == "3DSphere1")
        {
            print("colidiu2");
            collision.gameObject.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "3DSphere1")
        {
            print("colidiu3");
            collision.gameObject.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
        }
    }
}
