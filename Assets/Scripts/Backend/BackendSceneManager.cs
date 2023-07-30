using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GeViLab.Backend
{
    public class BackendSceneManager : MonoBehaviour
    {
        public ConfigLoader configLoader;
        public ConfigLoader.Config config;

        // public Scenes scenes;
        public GameObject spherePrefab;

        async void Start()
        {
            // Read config data from a configuration file
            config = await configLoader.LoadConfigAsync();
            if (config == null)
            {
                bool LoadConfigFailed = UnityEditor.EditorUtility.DisplayDialog(
                    "Error",
                    "Loading config failed!",
                    "Abort"
                );
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            else
            {
                configLoader.logConfig();
            }

            // Gather Metadata about locally cached files
            bool cacheInitialized = await FileCache.Instance.InitializeCache();
            if (!cacheInitialized)
            {
                bool InitializeCacheFailed = UnityEditor.EditorUtility.DisplayDialog(
                    "Error",
                    "Initializing cache failed!",
                    "Abort"
                );
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }

            // Load scene configuration from file
            // scenes = Scenes.Instance;
            // scenes.LoadScenes(Path.Combine(Application.persistentDataPath,config.SceneFilePath, config.SceneFileName));
            if (
                !Scenes.LoadScenes(
                    Path.Combine(
                        Application.persistentDataPath,
                        config.SceneFilePath,
                        config.SceneFileName
                    )
                )
            )
            {
                bool LoadScenesFailed = UnityEditor.EditorUtility.DisplayDialog(
                    "Error",
                    "Loading scenes failed!",
                    "Abort"
                );
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }

            BackendAccess.Initialize();
            
            Scenes.InitializeScenes(spherePrefab);
        }
    }
}
