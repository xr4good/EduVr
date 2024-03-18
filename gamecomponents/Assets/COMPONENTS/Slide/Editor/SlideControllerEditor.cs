using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SlideComponent
{
    [CustomEditor(typeof(SlideController))]
    public class SlideControllerEditor : Editor
    {
        SerializedProperty m_slideFiles;

        private void OnEnable()
        {
            m_slideFiles = serializedObject.FindProperty("slideFiles");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            for (int i = 0; i < m_slideFiles.arraySize; i++)
            {
                using (SerializedProperty element = m_slideFiles.GetArrayElementAtIndex(i))
                {
                    SerializedProperty m_usfw = element.FindPropertyRelative("useSlideFromWeb");
                    SerializedProperty m_url = element.FindPropertyRelative("url");

                    if (m_usfw.boolValue)
                    {
                        SerializedProperty m_slide = element.FindPropertyRelative("_slide");
                        m_slide.objectReferenceValue = null;

                        if(m_url.stringValue == "local")
                        {
                            m_url.stringValue = "enter image url";
                        }
                    }
                    else
                    {
                        m_url.stringValue = "local";
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
