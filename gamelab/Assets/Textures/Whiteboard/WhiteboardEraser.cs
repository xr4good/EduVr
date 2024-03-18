using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardEraser : MonoBehaviour
{
    public Whiteboard whiteboard;
    public GameObject Sponge;
    private RaycastHit touch;
    private bool lastTouch;
    private Quaternion lastAngle;
    // Start is called before the first frame update
    void Start()
    {
        this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard>();
    }

    // Update is called once per frame
    void Update()
    {
        float tipHeight = transform.Find("Sponge").transform.localScale.y;
        Vector3 tip = transform.Find("Sponge").transform.position;

        if (lastTouch)
        {
            tipHeight *= 0.5f;
        }

        if (Physics.Raycast(tip, transform.up, out touch, tipHeight))
        {
            if (!(touch.collider.CompareTag("Whiteboard")))
                return;
            this.whiteboard = touch.collider.GetComponent<Whiteboard>();
            this.whiteboard.SetColor(Color.blue);
            this.whiteboard.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
            this.whiteboard.ToggleTouch(true);

            if (!lastTouch)
            {
                lastTouch = true;
                lastAngle = transform.rotation;
            }
        }
        else
        {
            this.whiteboard.ToggleTouch(false);
            lastTouch = false;
        }
        if (lastTouch)
        {
            transform.rotation = lastAngle;
        }
    }
}
