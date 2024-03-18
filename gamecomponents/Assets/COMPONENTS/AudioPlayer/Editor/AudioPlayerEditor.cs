using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AudioPlayerComponent
{
    [CustomEditor(typeof(AudioPlayer))]
    public class AudioPlayerEditor : Editor
    {
        SerializedProperty m_audioSource;
        SerializedProperty m_audioClip;
        SerializedProperty m_audioFiles;
        SerializedProperty m_progressBar;

        void OnEnable()
        {
            m_audioSource = serializedObject.FindProperty("audioSource");
            m_audioClip = serializedObject.FindProperty("audioClip");
            m_audioFiles = serializedObject.FindProperty("audioFiles");
            m_progressBar = serializedObject.FindProperty("progressBar");
        }

        string audioLabel = "";
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            AudioPlayer ap = (AudioPlayer)target;
            var labelTitleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter }; labelTitleStyle.richText = true;

            //----------------------------------------------------------------------------------

            EditorGUILayout.LabelField("<b>Object References</b>", labelTitleStyle);
            EditorGUILayout.PropertyField(m_audioSource);
            EditorGUILayout.PropertyField(m_progressBar);
            EditorGUILayout.Space();
            EditorGUILayout.Separator();

            //----------------------------------------------------------------------------------
            
            EditorGUILayout.LabelField("<b>Audio Files</b>", labelTitleStyle);

            EditorGUILayout.Space();

            bool isDownloading = ap.IsDownloading();

            EditorGUILayout.PropertyField(m_audioFiles);
                
            for(int i=0; i<m_audioFiles.arraySize; i++)
            {
                SerializedProperty element = m_audioFiles.GetArrayElementAtIndex(i);

                SerializedProperty uafwProperty = element.FindPropertyRelative("useAudioFromWeb");
                SerializedProperty urlProperty = element.FindPropertyRelative("url");
                SerializedProperty audioTypeProperty = element.FindPropertyRelative("audioType");

                if (!uafwProperty.boolValue)
                {
                    urlProperty.stringValue = "local";
                    audioTypeProperty.intValue = 0;
                }
                else 
                {
                    if (urlProperty.stringValue == "local")
                        urlProperty.stringValue = "enter URL";

                    SerializedProperty audioClipProperty = element.FindPropertyRelative("_audioClip");
                    audioClipProperty.objectReferenceValue = null;
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("<b>General Download Options</b>", labelTitleStyle);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(!Application.isPlaying || isDownloading || !ap.HasUndownloadedAudios());
                {
                    if(GUILayout.Button(new GUIContent("Download new Audios", 
                            Application.isPlaying? 
                                "Download any new audios that have been added after the last download" :
                                "This button is only available during play test. " +
                                "\n\nClick to download any new audios that have been added after the last download"
                        ), GUILayout.MaxWidth(250)
                    ))
                    {
                        ap.DownloadNewAudios();
                    }

                    EditorGUI.EndDisabledGroup();
                }

                EditorGUILayout.Space(); EditorGUILayout.Space();

                EditorGUI.BeginDisabledGroup(!isDownloading || !Application.isPlaying);
                {
                    if(GUILayout.Button(new GUIContent("Cancel downloads",
                        Application.isPlaying ?
                            "Cancel all downloads that haven't been completed yet" :
                            "This button is only available during play test. " +
                            "\n\nClick to cancel all downloads that haven't been completed yet"
                        ), GUILayout.MaxWidth(250)
                    ))
                    {
                        ap.CancelAllDownloads();
                    }

                    EditorGUI.EndDisabledGroup();
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(!Application.isPlaying || isDownloading);
            {
                if(GUILayout.Button(new GUIContent("Re-download All",
                    Application.isPlaying ?
                        "Re-download all audios in the list, including the ones that have already been downloaded." :
                        "This button is only available during play test. " +
                        "\n\nClick to re-download all audios in the list, including the ones that have already been downloaded."
                    )
                ))
                {
                    ap.DownloadAllAudioClips();
                }
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("<b>Individual Download Options</b>", labelTitleStyle);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(!Application.isPlaying || isDownloading);
                {
                    EditorGUILayout.LabelField("Audio Label", GUILayout.MaxWidth(80));
                    audioLabel = EditorGUILayout.TextField(audioLabel, GUILayout.MaxWidth(170));
                    EditorGUILayout.Space();
                    int audioIndex = ap.GetAudioIndexByLabel(audioLabel);
                    bool downloadingCondition = audioIndex >= 0 && ap.IsDownloading(audioIndex);
                    EditorGUI.BeginDisabledGroup(audioIndex < 0 || downloadingCondition || !Application.isPlaying);
                    {
                        if(GUILayout.Button(new GUIContent(audioIndex < 0? "Download" : downloadingCondition? "Downloading audio (" + ap.GetDownloadProgress(audioIndex).ToString("0.##") + "%)" : ap.IsDownloaded(audioIndex)? "Re-Download" : "Download",
                            Application.isPlaying ?
                                "Download or Re-download the specified audio in the \"Audio Label\" field" :
                                "This button is only available during play test. " +
                                "\n\nClick to download or re-download the specified audio in the \"Audio Label\" field." +
                                "\n\n(WORK IN PROGRESS)"
                            ), GUILayout.MaxWidth(250)
                        ))
                        {
                            ap.StartCoroutine(ap.DownloadAudioClip(audioIndex));
                        }

                        EditorGUI.EndDisabledGroup();
                    }

                    EditorGUI.EndDisabledGroup();
                }

                EditorGUILayout.EndHorizontal();
                
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}
