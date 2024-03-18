using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class SlideChange : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject buttonBack;
    public GameObject buttonNexts;
    public static int totalNumberOfSliders = 0;
    public static bool isCompleted = false;
    public Texture2D[] slides;
    public Texture2D[] photos;
    public GameObject screen;
    public static int slideAtual = 0;
    void Start()
    {
        slides = GetDataAPI.slides;
        StartCoroutine(GetText());
        totalNumberOfSliders = slides.Length - 1;
        buttonBack.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
        screen.GetComponent<Renderer>().material.mainTexture = slides[slideAtual];
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://kodagabriel.com.br/get_photos_tp.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            string fotosString = www.downloadHandler.text;
            string[] fotos = fotosString.Split(',');
            photos = new Texture2D[fotos.Length -1];
            for (int i = 0; i < fotos.Length - 1; i++) {
                UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://kodagabriel.com.br/fotos_saul/" + fotos[i]);
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                    Debug.Log(request.error);
                else
                    photos[i] = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }
            // slides = photos;
            screen.GetComponent<Renderer>().material.mainTexture = slides[slideAtual];
        }
    }

    public void NextSlide()
    {
        if (slideAtual < slides.Length -1)
        {
            slideAtual += 1;
            screen.GetComponent<Renderer>().material.mainTexture = slides[slideAtual];
        } 
        if(slideAtual==1){
            buttonBack.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
        if(slideAtual==slides.Length-1){
            SlideChange.isCompleted = true;
            Debug.Log("SLIDECHANGE IS COMPLETED");
            buttonNexts.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
        }
    }
    public void PastSlide()
    {
        if (slideAtual > 0)
        {
            slideAtual -= 1;
            screen.GetComponent<Renderer>().material.mainTexture = slides[slideAtual];
            if(slideAtual==0){
                buttonBack.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
            }
                 if(slideAtual==slides.Length-2){
            buttonNexts.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
