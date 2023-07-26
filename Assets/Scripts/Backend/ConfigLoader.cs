using System.IO;
using UnityEngine;

namespace GeViLab.Backend
{
    public class ConfigLoader : MonoBehaviour
    {
        // Define a class to hold your configuration
        [System.Serializable]
        public class Config
        {
            public string AWSAccessKey;
            public string AWSSecretKey;
            public string AWSS3Bucket;
            public string AWSS3Region;
            public string CacheFolder;
        }

        public static Config config;

        void Awake()
        {
            // Read the JSON file
            string json = File.ReadAllText(Application.dataPath + "/config.json");

            // Deserialize it into your Config class
            config = JsonUtility.FromJson<Config>(json);

            // Use the values
            // Debug.Log("Access Key: " + config.AWSAccessKey);
            // Debug.Log("Secret Key: " + config.AWSSecretKey);
            // Debug.Log("Bucket Name: " + config.AWSS3Bucket);
            // Debug.Log("S3 Region: " + config.AWSS3Region);
            // Debug.Log("CacheFolder: " + config.CacheFolder);
        }
    }
}
