using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FadeController : MonoBehaviour
{
    // Start is called before the first frame update
    public void fadeStart()
    {
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, 1);
    }
}
