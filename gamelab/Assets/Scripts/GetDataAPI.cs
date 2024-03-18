using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Net.Mail;

public class GetDataAPI : MonoBehaviour
{
    public Text emailText;
    public GameObject inGameToggle;

    public GameObject panel;
    public GameObject panelData;
    public string url;
    public string urlMailCheck;
    public int id;

    public static int setTime;
    Lessons content;
    public static Lesson selectedLesson;
    public Dropdown dropdown;
    public static Texture2D[] slides;
    public static string[] videos;
    public static string title;
    public GameObject button;
    public Text textInfo;
    public static string contentText;
    public static Lessons lessons;
    public Text validMail;
    public GameObject buttonData;
    public Text textInfoData;
    public Text progress;
    int downloadCounter = 0;
    bool success = true;
    bool finished = false;
    static int progressCounter = 0;
    string[] idAulas;
    string[] titleAulas;
    public Button startButton;
    public Text textButton;
    public Text email;
    bool checkMail = false;
    bool checkDropdown = false;

    public GameObject emailSpinning;
    // Start is called before the first frame update


    void Start()
    {
        StartCoroutine(GetContent());
        setTime = new System.Random().Next(1, 5);
                Debug.Log(setTime);

    }

    void Update()
    {
        
    }

    public void updateText(){
        if (validMail.text.Equals("Email inválido")){
            validMail.text = "Digite seu email cadastrado no curso";
                        validMail.color = new Color32(50, 50, 50, 255);

        }
    }
    public void checkMailValid(){
         StartCoroutine(verifyMail());
    }

    IEnumerator verifyMail(){
        startButton.enabled = false;
        emailSpinning.SetActive(true);
        textButton.text="";
        if(urlMailCheck[urlMailCheck.Length-1].Equals('/')){
            urlMailCheck = urlMailCheck + email.text;
        }else{
            urlMailCheck = urlMailCheck + "/" + email.text;
        }
        UnityWebRequest www = UnityWebRequest.Get(urlMailCheck);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {   
            validMail.text = "Falha na conexão";
            validMail.color = new Color32(255, 112, 112, 255);
            startButton.enabled = true;
        emailSpinning.SetActive(false);
        textButton.text="Entrar";
        }
        else
        {   
            Debug.Log(urlMailCheck);
            Debug.Log(www.downloadHandler.text);
            if(www.downloadHandler.text.Equals("true")){
                        emailSpinning.SetActive(false);

                StartGetSlides();
            }else{
                validMail.text = "Email inválido";
                validMail.color = new Color32(255, 112, 112, 255);
                startButton.enabled = true;
        emailSpinning.SetActive(false);
                textButton.text="Entrar";

            }
           
        }
    }

    public void StartGetContent()
    {
        textInfo.text = "Buscando Aulas";
        if (button.activeSelf)
        {
            button.SetActive(false);
        }
        StartCoroutine(GetContent());
    }

    public void StartGetSlides()
    {
        StartLesson();
        if (!panelData.activeSelf)
        {
            panelData.SetActive(true);
        }
        progress.text = "0%";
        progressCounter = 0;
        textInfoData.text = "Obtendo Dados";
        if (buttonData.activeSelf)
        {
            buttonData.SetActive(false);
        }
        success = true;
        finished = false;
        StartCoroutine(GetSlides());
    }

    // Update is called once per frame
    public void UpdateSelectedId()
    {
        id = dropdown.value;
    }

    IEnumerator GetContent()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            textInfo.text = "Erro ao buscar aulas";
            button.SetActive(true);
        }
        else
        {
            content = JsonUtility.FromJson<Lessons>(
                "{\"lessons\":" + www.downloadHandler.text + "}"
            );
            int count = 0;
            idAulas = new string[content.lessons.Count];
            titleAulas = new string[content.lessons.Count];
            foreach (Lesson i in content.lessons)
            {
                idAulas[count] = i.post.id;
                titleAulas[count] = i.post.post_title;
                count++;
            }
            StartDropdown();
            panel.SetActive(false);
        }
    }

    public void StartLesson()
    {
        videos = content.lessons[id].post.metadata.aoc_vid_video.Split(char.Parse(";"));
        title = content.lessons[id].post.post_title;
        contentText = content.lessons[id].post.post_content;
        contentText = contentText.Replace("<ul>","");
        contentText = contentText.Replace("</ul>","");
        contentText = contentText.Replace("</li>","");
        contentText = contentText.Replace("<li>","-");
        contentText = contentText.Replace("	","");
        selectedLesson=content.lessons[id];
    }

    public void StartDropdown()
    {
        dropdown.ClearOptions();
        int count = 0;
        foreach (string i in idAulas)
        {
            if (i != "" && i != null)
            {
                dropdown.options.Add(
                    new Dropdown.OptionData() { text = idAulas[count] + " - " + titleAulas[count] }
                );
                count++;
            }
        }
        dropdown.RefreshShownValue();
    }

    IEnumerator GetSlides()
    {
        string[] images = content.lessons[id].post.metadata.aoc_image_image.Split(char.Parse(";"));
        slides = new Texture2D[images.Length];
        int count = 0;

        if (images.Length > 0 && images[id] != "")
        {
            foreach (string i in images)
            {
                if (i != "" && i != null)
                {
                    StartCoroutine(GetImages(i, count, images.Length));
                    count++;
                }
            }
        }
        yield break;
    }

    IEnumerator GetImages(string i, int count, int totalLength)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(i);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            success = false;
            textInfoData.text = "Erro ao buscar dados";
            buttonData.SetActive(true);
        }
        else
        {
            slides[count] = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        downloadCounter++;
        progressCounter = (progressCounter + (100 / totalLength));
        progress.text = progressCounter + "%";
        if (downloadCounter == totalLength - 1)
        {
            finished = true;
            if (success != false)
            {
                openLab();
            }
        }
    }

    public void openLab()
    {
        UserLoginData.email = emailText.text.ToString();
        if (inGameToggle.GetComponent<Toggle>().isOn == true)
        {
            UserLoginData.usingHeadset = true;
            UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.InitializeLoaderSync();

            UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
        else
        {
            UserLoginData.usingHeadset = false;
        }
        SceneManager.LoadScene("Laboratory");
    }
}
