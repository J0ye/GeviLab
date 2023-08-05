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

            if (fileCache.PublicMetadata == null)
            {
                GUILayout.Label("No metadata available");
                return;
            }
            foreach (var item in fileCache.PublicMetadata)
            {
                GUILayout.Label($"{item.Key}: {item.Value}");
            }
        }
    }
}
