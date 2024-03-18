using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SeriousGameComponents.QuizComponent
{
    [CustomEditor(typeof(QuizController))]
    public class QuizControllerEditor : Editor
    {
        SerializedProperty m_componentIdentifier;
        SerializedProperty m_useLmsLoader;

        GUIStyle labelTitleStyle;
        private void OnEnable()
        {
            m_componentIdentifier = serializedObject.FindProperty("identifier");
            m_useLmsLoader = serializedObject.FindProperty("useLmsLoader");
        }

        const bool WIP = true;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            labelTitleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            labelTitleStyle.richText = true;

            EditorGUILayout.LabelField("<b>Quiz UI</b>", labelTitleStyle);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAnswerCount"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("quizTitleObject"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("quizDescriptionObject"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("quizStatusMessageObject"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("correctAnswerMessage"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("wrongAnswerMessage"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("questionField"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("answerFields"));

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.LabelField("<b>Quiz Content</b>", labelTitleStyle);

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(!m_useLmsLoader.boolValue || WIP);
            {
                EditorGUILayout.PropertyField(m_componentIdentifier);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(m_useLmsLoader);
            EditorGUILayout.Space(); EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
            EditorGUI.BeginDisabledGroup(m_useLmsLoader.boolValue);
            {
                DrawDefaultInspector();
            }
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
