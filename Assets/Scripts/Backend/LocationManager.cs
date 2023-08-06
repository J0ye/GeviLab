using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GeViLab.Backend
{
    public class LocationManager : MonoBehaviour
    {
        public ConfigLoader configLoader;
        public ConfigLoader.Config config;

        public GameObject spherePrefab;
        public GameObject itemPrefab;
        public GameObject bridgePrefab;

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
                if (config.CacheFolder == null || config.CacheFolder == "")
                    config.CacheFolder = "cache";
                if (config.LocationFilePath == null)
                    config.LocationFilePath = "projects";
                if (config.LocationFileName == null || config.LocationFileName == "")
                    config.LocationFileName = "Default.project";
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

            // Load location configuration from backend
            // BackendAccess.DownloadObjectFromBucketAsync(
            //     config.AWSS3BucketName,
            //     config.AWSS3LocationFileName,
            //     Path.Combine(
            //         config.LocationFilePath,
            //         config.LocationFileName
            //     )
            // );

            // Load location configuration from file
            // OpenFileDialog openFileDialog = new OpenFileDialog();
            // openFileDialog.Filter = "Project files (*.project)|*.project";
            // openFileDialog.Title = "Select a Project file (JSON)";

            // if (openFileDialog.ShowDialog() == DialogResult.OK)
            // {
            //     string selectedFilePath = openFileDialog.FileName;
            //     // Do something with the selected file path
            // }

            if (
                !Locations.LoadLocationsLocal(
                    Path.Combine(
                        Application.persistentDataPath,
                        config.CacheFolder,
                        config.LocationFilePath,
                        config.LocationFileName
                    )
                )
            )
            {
                bool LoadLocationsFailed = UnityEditor.EditorUtility.DisplayDialog(
                    "Error",
                    "Loading locations failed!",
                    "Abort"
                );
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }

            BackendAccess.Initialize();

                Locations.InitializeLocations(spherePrefab, itemPrefab, bridgePrefab);
        }
    }
}
