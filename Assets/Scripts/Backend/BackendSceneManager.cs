using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BackendSceneManager : MonoBehaviour
{
    public Scenes scenes;
    public string sceneFilePath = "";
    public string sceneFileName = "TestScenes.json";
    public GameObject spherePrefab;

    void Start()
    {
        if (sceneFilePath == "")
            sceneFilePath = Application.persistentDataPath;
        scenes = Scenes.Instance;
        scenes.LoadScenes(Path.Combine(sceneFilePath, sceneFileName));
        scenes.InitializeScenes(spherePrefab);
    }
}
