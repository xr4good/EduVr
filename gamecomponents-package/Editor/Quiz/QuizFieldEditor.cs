using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SeriousGameComponents.QuizComponent
{
    [CustomEditor(typeof(QuizField))]
    public class QuizFieldEditor : Editor
    {
        SerializedProperty m_labelText;
        SerializedProperty m_text;
        SerializedProperty m_autodetectProperties;

        void OnEnable()
        {
            m_labelText = serializedObject.FindProperty("labelText");
            m_text = serializedObject.FindProperty("text");
            m_autodetectProperties = serializedObject.FindProperty("autodetectProperties");
        }

        bool autodetectProperties=true;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            autodetectProperties = EditorGUILayout.Toggle(new GUIContent("Autodetect properties",
                "If checked, the game object will automatically search for any child objects with the respective names:\n" +
                "\nText\n\nLabel Text\n\n" +
                "If not found, or if the respective child objects do not contain a Text component, this field will be automatically set to false"
                )
                , autodetectProperties);

            if (!autodetectProperties)
            {
                m_autodetectProperties.boolValue = autodetectProperties;
                EditorGUILayout.PropertyField(m_text);
                EditorGUILayout.PropertyField(m_labelText);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
