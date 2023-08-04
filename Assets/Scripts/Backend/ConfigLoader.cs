using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace GeViLab.Backend
{
    public class ConfigLoader : MonoBehaviour
    {
        [SerializeField]
        private string configFilePath;

        [SerializeField]
        private string configFileName = "Config.json";

        // Define a class to hold your configuration
        [System.Serializable]
        public class Config
        {
            public string AWSAccessKey;
            public string AWSSecretKey;
            public string AWSS3Bucket;
            public string AWSS3Region;
            public string CacheFolder;
            public string SceneFilePath;
            public string SceneFileName;
        }

        public static Config config = null;

        public async Task<Config> LoadConfigAsync()
        {
            if (configFilePath == "")
                configFilePath = Application.persistentDataPath;

            try
            {
                // Read the JSON file
                string json = File.ReadAllText(Path.Combine(configFilePath, configFileName));
                // Deserialize it into your Config class
                config = JsonUtility.FromJson<Config>(json);
            }
            catch (System.Exception)
            {
                Debug.LogError(
                    "Config file not found. Option to create new config file to eit with your AWS credentials."
                );
                // Show a message box to confirm creating a new config file
                bool createNewConfig = UnityEditor.EditorUtility.DisplayDialog(
                    "Create New Config File",
                    "Are you sure you want to create a new config file?",
                    "Yes",
                    "No"
                );

                if (createNewConfig)
                {
                    CreateConfig();
                }
            }
            return config;
        }

        private void CreateConfig()
        {
            // Create a new Config object with default values
            Config newConfig = new Config
            {
                AWSAccessKey = "ENTER ACCESS KEY HERE",
                AWSSecretKey = "ENTER SECRET KEY HERE",
                AWSS3Bucket = "ENTER BUCKET NAME HERE",
                AWSS3Region = "eu-central-1",
                CacheFolder = "cache",
                SceneFilePath = "",
                SceneFileName = "TestScenes.json"
            };

            // Serialize the Config object to JSON
            string json = JsonUtility.ToJson(newConfig);

            // Write the JSON to a file
            File.WriteAllText(
                Path.Combine(Application.persistentDataPath, "Config.json"),
                json
            );

            // Set the static config variable to the new Config object
            config = newConfig;
        }

        public void logConfig()
        {
            Debug.Log("Access Key: " + config.AWSAccessKey);
            Debug.Log("Secret Key: " + config.AWSSecretKey);
            Debug.Log("Bucket Name: " + config.AWSS3Bucket);
            Debug.Log("S3 Region: " + config.AWSS3Region);
            Debug.Log("CacheFolder: " + config.CacheFolder);
            Debug.Log("SceneFilePath: " + config.SceneFilePath);
            Debug.Log("SceneFileName: " + config.SceneFileName);
        }
    }
}
