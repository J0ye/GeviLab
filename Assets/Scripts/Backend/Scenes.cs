using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Scenes : MonoBehaviour
{
    private static Scenes instance;
    private List<Scene> scenes;

    private Scenes()
    {
        scenes = new List<Scene>();
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
        scenes.Add(scene);
    }

    public void RemoveScene(Scene scene)
    {
        scenes.Remove(scene);
    }

    public List<Scene> GetScenes()
    {
        return scenes;
    }

    public void InitializeScenes(GameObject spherePrefab)
    {
        foreach (Scene scene in scenes)
        {
            scene.Initialize(spherePrefab);
        }
    }
    public void SyncScenes()
    {
        foreach (Scene scene in scenes)
        {
            scene.Sync();
        }
    }
    public void LoadScenes(string filePath)
    {
        // string filePath = Path.Combine("Assets", "Resources", fileName);
        // string filePath = Path.Combine(Application.persistentDataPath, fileName);
        // TextAsset textAsset = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(filePath));
        // if (textAsset != null)
        // {
        //     string json = textAsset.text;
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
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    public void SerializeScenesToJson(string filePath)
    {
        SyncScenes();
        // Debug.Log("Serializing " + scenes.Count + " scenes to " + filePath);
        string json = JsonConvert.SerializeObject(scenes, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}
