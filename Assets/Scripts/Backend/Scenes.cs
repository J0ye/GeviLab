using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Scenes : MonoBehaviour
{
    private static Scenes instance;
    private static List<Scene> scenes = null;

    private Scenes()
    {
        // scenes = new List<Scene>();
    }

    public static Scenes Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Scenes();
            }
            return instance;
        }
    }

    public void AddScene(Scene scene)
    {
        if (scenes != null)
            scenes.Add(scene);
    }

    public void RemoveScene(Scene scene)
    {
        if (scenes != null)
            scenes.Remove(scene);
    }

    public static List<Scene> GetScenes()
    {
        return scenes;
    }

    public static void InitializeScenes(GameObject spherePrefab)
    {
        foreach (Scene scene in scenes)
        {
            scene.Initialize(spherePrefab);
        }
    }
    public static void SyncScenes()
    {
        foreach (Scene scene in scenes)
        {
            scene.Sync();
        }
    }
    public static bool LoadScenes(string filePath)
    {
        
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            if (json.StartsWith("{"))
            {
                // If the JSON string starts with a '{', it is a JSON object and needs to be wrapped in an array
                json = "[" + json + "]";
            }
            scenes = JsonConvert.DeserializeObject<List<Scene>>(json);
            // Debug.Log("Loaded " + scenes.Count + " scenes from " + fileName);
            // foreach (Scene scene in scenes)
            // {
            //     Debug.Log(
            //         $"{scene.id} {scene.name} {scene.description} {scene.position} {scene.rotation} {scene.scale}"
            //     );
            // }
            return true;
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
        return false;
    }

    /// <summary>
    /// Serializes the list of scenes to a JSON file at the specified file path.
    /// </summary>
    /// <param name="filePath">The file path where the JSON file will be saved.</param>
    public static void SerializeScenesToJson(string filePath)
    {
        SyncScenes(); // Sync the transform properties to the current object transformations before serializing
        // Debug.Log("Serializing " + scenes.Count + " scenes to " + filePath);
        string json = JsonConvert.SerializeObject(scenes, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}
