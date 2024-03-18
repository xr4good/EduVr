using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SeriousGameComponents
{
    [CustomEditor(typeof(BaseStatementSender),true)]
    public class BaseStatementSenderEditor : Editor
    {
        SerializedProperty m_useLRSInformation;
        private void OnEnable()
        {
            m_useLRSInformation = serializedObject.FindProperty("useLRSInformation");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_useLRSInformation);

            serializedObject.ApplyModifiedProperties();

            EditorGUI.BeginDisabledGroup(m_useLRSInformation.boolValue);
            {
                DrawDefaultInspector();
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
