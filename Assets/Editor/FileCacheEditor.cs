using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GeViLab.Backend
{
    [CustomEditor(typeof(FileCache))]
    public class FileCacheEditor : Editor
    {
        // private Dictionary<string, DateTime> metadata;

        private void OnEnable()
        {
            // metadata = ((FileCache)target).PublicMetadata;
        }

        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();

            GUILayout.Space(10);

            GUILayout.Label("Metadata", EditorStyles.boldLabel);

            serializedObject.Update();

            FileCache fileCache = (FileCache)target;

            foreach (var item in fileCache.PublicMetadata)
            {
                GUILayout.Label($"{item.Key}: {item.Value}");
            }
            // if (metadata != null)
            // {
            //     foreach (KeyValuePair<string, DateTime> pair in metadata)
            //     {
            //         EditorGUILayout.LabelField(pair.Key, pair.Value.ToString());
            //     }

            //     serializedObject.ApplyModifiedProperties();
            // }
        }
    }
}
