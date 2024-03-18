using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SeriousGameComponents.SlideComponent
{
    [CustomEditor(typeof(SlideController))]
    public class SlideControllerEditor : Editor
    {
        SerializedProperty m_slideFiles;
        SerializedProperty m_useLmsLoader;
        SerializedProperty m_errorSlide;
        SerializedProperty m_screen;
        SerializedProperty m_componentIdentifier;

        private void OnEnable()
        {
            m_slideFiles = serializedObject.FindProperty("slideFiles");
            m_useLmsLoader = serializedObject.FindProperty("useLmsLoader");
            m_errorSlide = serializedObject.FindProperty("errorSlide");
            m_screen = serializedObject.FindProperty("screen");
            m_componentIdentifier = serializedObject.FindProperty("identifier");
        }

        const bool WIP = true;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(!m_useLmsLoader.boolValue || WIP);
            EditorGUILayout.PropertyField(m_componentIdentifier);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(m_useLmsLoader);


            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Separator(); EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(m_useLmsLoader.boolValue);

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

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator(); EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(m_screen);
            EditorGUILayout.PropertyField(m_errorSlide);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
