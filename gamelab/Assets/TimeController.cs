using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeController : MonoBehaviour
{
    
    public Material[] noite;
    public Material[] meioDia;
    public Material[] chuvaDia;
    public Material[] chuvaNoite;
    public Material[] anoitecer;
    public Light sun;
    public GameObject neblina;
    public GameObject chuva;
    public AudioSource audioData;
    private Material skyMaterial;
    private int skybox;

    // Start is called before the first frame update
    void Start()
    {
       // GetDataAPI.setTime
         switch (GetDataAPI.setTime)
        {
        case 1: //noite
            skybox = new System.Random().Next(0, noite.Length-1); // Generates a number between 1 to 10
            RenderSettings.skybox = noite[skybox];
            sun.color = new Color32(147,147,147,255);
            break;
        case 2: //meio dia
            skybox = new System.Random().Next(0, meioDia.Length-1);
            RenderSettings.skybox = meioDia[skybox];
            sun.color = new Color32(255,242,191,255);
            break;
        case 3: //chuvaDia
            skybox = new System.Random().Next(0, chuvaDia.Length-1);
            RenderSettings.skybox = chuvaDia[skybox];
            sun.color = new Color32(147,147,147,255);
            neblina.SetActive(true);
            chuva.SetActive(true);
            audioData.Play();
            break;
        case 4: //chuvaNoite
            skybox = new System.Random().Next(0, chuvaNoite.Length-1);
            RenderSettings.skybox = chuvaNoite[skybox];
            sun.color = new Color32(147,147,147,255);
            neblina.SetActive(true);
            chuva.SetActive(true);
            audioData.Play();
            break;
         case 5: //anoitecer
            skybox = new System.Random().Next(0, anoitecer.Length-1);
            RenderSettings.skybox = anoitecer[skybox];
            sun.color = new Color32(255,130,107,255);
            break;
        }

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    
}
