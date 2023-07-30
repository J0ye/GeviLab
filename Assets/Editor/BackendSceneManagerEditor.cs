using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BackendSceneManager))]
public class BackendSceneManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BackendSceneManager backendSceneManager = (BackendSceneManager)target;
        if (backendSceneManager == null || backendSceneManager.scenes.GetScenes() == null)
        {
            return;
        }

        GUILayout.Space(10);

        GUILayout.Label("Scenes:");

        foreach (Scene scene in backendSceneManager.scenes.GetScenes())
        {
            GUILayout.Label(scene.description);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Save Scenes to JSON"))
        {
            string path = EditorUtility.SaveFilePanel(
                "Save Scenes to JSON",
                backendSceneManager.sceneFilePath,
                backendSceneManager.sceneFileName,
                "json"
            );
            if (path.Length != 0)
            {
                backendSceneManager.scenes.SerializeScenesToJson(path);
            }
        }
    }
}
