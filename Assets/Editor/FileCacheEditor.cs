using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GeViLab.Backend
{
    [CustomEditor(typeof(FileCache))]
    public class FileCacheEditor : Editor
    {
        private Dictionary<string, DateTime> metadata;

        private void OnEnable()
        {
            metadata = ((FileCache)target).PublicMetadata;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (metadata != null)
            {
                foreach (KeyValuePair<string, DateTime> pair in metadata)
                {
                    EditorGUILayout.LabelField(pair.Key, pair.Value.ToString());
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
