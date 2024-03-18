using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeriousGameComponents.QuizComponent
{
    public class QuizField : MonoBehaviour
    {
        [Tooltip("Text object that will show the label that will enumerate the answer for you")]
        public Text labelText;

        [Tooltip("Text object that will show the text that represents the answer/question")]
        public Text text;

        public bool autodetectProperties;
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (autodetectProperties)
            {
                DetectProperties();
            }
        }

        public void DetectProperties()
        {
            if (labelText == null)
            {
                Transform ltext = transform.Find("Label Text");
                if (ltext == null)
                {
                    autodetectProperties = false;
                    Debug.LogWarning("Could not find child with \"Label Text\" name on " + gameObject.name + " object. Did you name them correctly?");
                }
                else
                {
                    labelText = ltext.GetComponent<Text>();
                    if (labelText == null)
                    {
                        autodetectProperties = false;
                        Debug.LogWarning("Child object \"Label Text\" does not have a Text component!");
                    }
                }
            }

            if (text == null)
            {
                Transform atext = transform.Find("Text");
                if (atext == null)
                {
                    autodetectProperties = false;
                    Debug.LogWarning("Could not find child with \"Text\" name on " + gameObject.name + " object. Did you name them correctly?");
                }
                else
                {
                    text = atext.GetComponent<Text>();
                    if (text == null)
                    {
                        autodetectProperties = false;
                        Debug.LogWarning("Child object named \"Text\" does not have a Text component!");
                    }
                }
            }
        }

        public void InstantiateProperties()
        {
            GameObject child;

            child = new GameObject("Label Text");
            child.transform.parent = gameObject.transform;
            child.AddComponent<Text>();

            child = new GameObject("Text");
            child.transform.parent = gameObject.transform;
            child.AddComponent<Text>();

            DetectProperties();
            autodetectProperties = true;
        }

        public void SetText(string text)
        {
            DetectProperties();
            this.text.text = text;
        }

        public void SetLabelText(string text)
        {
            DetectProperties();
            labelText.text = text;
        }
    }
}
