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

    public void SerializeScenesToJson(string filePath)
    {
        string json = JsonConvert.SerializeObject(scenes, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public void LoadScenesFromResources(string fileName)
    {
        string filePath = Path.Combine("Assets", "Resources", fileName);
        TextAsset textAsset = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(filePath));
        if (textAsset != null)
        {
            string json = textAsset.text;
            if (json.StartsWith("{"))
            {
                // If the JSON string starts with a '{', it is a JSON object and needs to be wrapped in an array
                json = "[" + json + "]";
            }
            // scenes = JsonConvert.DeserializeObject<List<Scene>>(json);
            scenes = JsonConvert.DeserializeObject<List<Scene>>(json);
            Debug.Log("Loaded " + scenes.Count + " scenes from " + fileName);
            foreach (Scene scene in scenes)
            {
                Debug.Log(scene.GetDescription());
            }
        }
        else
        {
            Debug.LogError("File not found: " + fileName);
        }
    }
}

