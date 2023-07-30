using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BackendSceneManager : MonoBehaviour
{
    private Scenes scenes;
    public string sceneFile = "TestScenes.json";
    void Start()
    {
        scenes = Scenes.Instance;
        scenes.LoadScenesFromResources(sceneFile);
    }
}

