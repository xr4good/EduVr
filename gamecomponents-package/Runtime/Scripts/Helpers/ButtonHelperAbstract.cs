using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGameComponents
{
    ///<summary> ButtonHelperAbstract is a base for all button helpers of all components </summary>
    public abstract class ButtonHelperAbstract : MonoBehaviour
    {
        [Serializable]
        public struct ButtonRenderer
        {
            public string name;
            public Renderer renderer;

            [HideInInspector]
            public bool isEnabled;

            [HideInInspector]
            public int materialArrayPosition;
        }
        public ButtonRenderer[] buttonRenderers;

        public Material disableMaterial;

        protected virtual void Start()
        {
            UpdateButtonStatuses();
            ChildStart();
        }

        protected virtual void Update()
        {
            ChildUpdate();
        }

        /// <summary>
        /// Override this method instead of creating a new Start method: <code>Example: protected override void ChildStart() { ... } </code>
        /// If you wish, you can also override the regular Start method using a base call at the start:  <code>Example: protected override void Start() { base.Start(); } </code>
        /// This is to ensure proper functionality of the functions implemented in the superclass. It is not advised to override both Start and ChildStart, as this may cause inconsistencies in game behaviour.
        /// </summary>
        protected virtual void ChildStart() { }

        /// <summary>
        /// Override this method instead of creating a new Update method: <code>Example: protected override void ChildUpdate() { ... } </code>
        /// If you wish, you can also override the regular Update method using a base call at the start: <code>Example: protected override void Update() { base.Update(); ... } </code> 
        /// This is to ensure proper functionality of the functions implemented in the superclass. It is not advised to override both Update and ChildUpdate, as this may cause inconsistencies in game behaviour.
        /// </summary>
        protected virtual void ChildUpdate() { }

        protected void UpdateButtonStatuses()
        {
            for (int i = 0; i < buttonRenderers.Length; i++)
            {
                if (!buttonRenderers[i].isEnabled)
                {
                    buttonRenderers[i].isEnabled = true;
                    DisableButton(i);
                }
            }
        }

        public void DisableAllButtons()
        {
            for (int i = 0; i < buttonRenderers.Length; i++)
            {
                DisableButton(i);
            }
        }

        protected int Find(string name)
        {
            int i = 0;
            foreach (ButtonRenderer br in buttonRenderers)
            {
                if (br.name == name)
                    return i;

                i++;
            }

            throw new System.Exception("Could not find button " + name);
        }

        public void DisableButton(string key)
        {
            int i = Find(key);
            DisableButton(i);
        }

        public void DisableButton(int i)
        {
            if (buttonRenderers[i].isEnabled)
            {
                List<Material> mAux = new List<Material>(buttonRenderers[i].renderer.materials);
                mAux.Add(disableMaterial);
                buttonRenderers[i].materialArrayPosition = mAux.Count - 1;
                buttonRenderers[i].renderer.materials = mAux.ToArray();
                buttonRenderers[i].isEnabled = false;
            }
        }

        public void EnableButton(string key)
        {
            int i = Find(key);
            EnableButton(i);
        }

        public void EnableButton(int i)
        {
            if (!buttonRenderers[i].isEnabled)
            {
                List<Material> mAux = new List<Material>(buttonRenderers[i].renderer.materials);
                mAux.RemoveAt(buttonRenderers[i].materialArrayPosition);
                buttonRenderers[i].renderer.materials = mAux.ToArray();
                buttonRenderers[i].isEnabled = true;
            }
        }


    }
}