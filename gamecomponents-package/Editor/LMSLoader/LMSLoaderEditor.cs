using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SeriousGameComponents.LmsComponent
{
    [CustomEditor(typeof(LmsLoader))]
    public class LMSLoaderEditor : Editor
    {
        SerializedProperty m_username;
        SerializedProperty m_password;

        void OnEnable()
        {
            m_username = serializedObject.FindProperty("username");
            m_password = serializedObject.FindProperty("password");
        }

        string password;
        bool showPassword = false;
        GUIStyle labelTitleStyle;
        GUIStyle loginStatusStyle;
        System.Exception loginException = null;
        int selectedLesson = 0;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            labelTitleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            labelTitleStyle.richText = true;
            loginStatusStyle = new GUIStyle(GUI.skin.label);
            loginStatusStyle.richText = true;

            LmsLoader ll = (LmsLoader)serializedObject.targetObject;

            bool loggedIn;
            try
            {
                loggedIn = ll.IsLoggedIn();
            }
            catch (System.UnauthorizedAccessException e)
            {
                loggedIn = false;
                loginException = e;
            }

            loginStatusStyle.normal.textColor = ll.loggingIn ? Color.yellow : loggedIn ? Color.green : Color.red;

            EditorGUILayout.LabelField("<b>Login Credentials</b>", labelTitleStyle);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_username);
            EditorGUILayout.BeginHorizontal();
            if (!showPassword)
                password = EditorGUILayout.PasswordField("Password", password);
            else
                password = EditorGUILayout.TextField("Password", password);
            EditorGUILayout.LabelField("Show", GUILayout.Width(35));
            showPassword = EditorGUILayout.Toggle(showPassword, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(!Application.isPlaying || ll.loggingIn);
            {
                if (GUILayout.Button(new GUIContent("Login", "Login to the LMS with these credentials and download the contents for lessons. Only works in playmode")))
                {
                    ll.SubmitLogin(ll.username, ll.password);
                }
            }
            EditorGUI.EndDisabledGroup();

            m_password.stringValue = password;
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("<b>LMS Information</b>", labelTitleStyle);

            bool contentFetched = false;
            EditorGUI.BeginDisabledGroup(true);
            {
                string labelStatusText = "";
                if (ll.loggingIn)
                {
                    labelStatusText = "Logging In...";
                    loginStatusStyle.normal.textColor = Color.yellow;
                }
                else if (!loggedIn)
                {
                    labelStatusText = "Not Logged! " +
                            (loginException != null ? loginException.Message :
                                Application.isPlaying ? "Invalid username or password" : ""
                            );

                    loginStatusStyle.normal.textColor = Application.isPlaying? Color.red : Color.white;
                }
                else
                {
                    loginStatusStyle.normal.textColor = Color.green;
                    if (ll.fetchingContent)
                    {
                        labelStatusText = "Fetching Contents...";
                    }
                    else
                    {
                        try
                        {
                            labelStatusText = "Could not fetch contents. ";
                            contentFetched = ll.ContentIsFetched();
                        }
                        catch (KeyNotFoundException e)
                        {
                            labelStatusText = e.Message;
                            contentFetched = false;
                        }

                        if (contentFetched)
                            labelStatusText = "Content was fetched! Logged In!";
                        else
                            loginStatusStyle.normal.textColor = Color.red;
                    }
                }

                EditorGUILayout.LabelField("<b>" + labelStatusText +
                    "</b>", loginStatusStyle, GUILayout.Height(35));
                EditorGUILayout.TextField("LMS Name: ", ll.LMSName);
                EditorGUILayout.TextField("LMS Address: ", ll.baseURL);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("<b>Lesson Information</b>", labelTitleStyle);

            EditorGUI.BeginDisabledGroup(!contentFetched);
            {
                var selectedLabelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                selectedLabelStyle.richText = true;
                LayoutLessonList();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("<size=20><b>Selected Lesson: </b></size>", selectedLabelStyle);
                EditorGUILayout.LabelField("<size=15>" + (ll.lessonList == null ? "None" : ll.lessonList.Count == 0 ? "None" : ll.lessonList[selectedLesson]) + "</size>",selectedLabelStyle, GUILayout.Height(30));
            }

            if(GUILayout.Button(new GUIContent("Fetch Assets", "Fetch Assets registered in the selected lesson")))
            {
                ll.DownloadLessonAssets(selectedLesson);
            }

            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight), ll.GetAssetDownloadPercentage()/100f, "Asset Download Progress");
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }

        bool showLessonFold;
        private void LayoutLessonList()
        {
            SerializedProperty m_lessonList = serializedObject.FindProperty("lessonList");
            int len = m_lessonList.arraySize;

            showLessonFold = EditorGUILayout.Foldout(showLessonFold, new GUIContent("Lessons","Lessons registered for the logged user"));
            if (showLessonFold && Application.isPlaying)
            {
                EditorGUI.indentLevel = 1;
                for (int i = 0; i < len; i++)
                {
                    string lesson = m_lessonList.GetArrayElementAtIndex(i).stringValue;
                    GUI.SetNextControlName("" + i);
                    EditorGUILayout.SelectableLabel(lesson, GUILayout.Height(18));
                }
                EditorGUI.indentLevel = 0;

                if(GUI.GetNameOfFocusedControl() != "")
                    selectedLesson = int.Parse(GUI.GetNameOfFocusedControl());
            }
        }
    }
}
