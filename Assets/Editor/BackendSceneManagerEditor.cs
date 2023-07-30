using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scenes))]
public class BackendSceneManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Scenes scenes = (Scenes)target;

        GUILayout.Space(10);

        GUILayout.Label("Scenes:");

        foreach (Scene scene in scenes.GetScenes())
        {
            GUILayout.Label(scene.GetDescription());
        }
    }
}