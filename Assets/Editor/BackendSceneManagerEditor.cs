using UnityEditor;
using UnityEngine;

namespace GeViLab.Backend
{
    [CustomEditor(typeof(BackendSceneManager))]
    public class BackendSceneManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BackendSceneManager backendSceneManager = (BackendSceneManager)target;
            if (backendSceneManager == null || Scenes.GetScenes() == null)
            {
                return;
            }

            GUILayout.Space(10);

            GUILayout.Label("Scenes:");

            foreach (Scene scene in Scenes.GetScenes())
            {
                GUILayout.Label(scene.name + ":" + scene.description);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Save Scenes to JSON"))
            {
                string path = EditorUtility.SaveFilePanel(
                    "Save Scenes to JSON",
                    backendSceneManager.config.SceneFilePath,
                    backendSceneManager.config.SceneFileName,
                    "json"
                );
                if (path.Length != 0)
                {
                    Scenes.SerializeScenesToJson(path);
                }
            }
        }
    }
}
