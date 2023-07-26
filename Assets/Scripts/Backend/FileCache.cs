using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
// using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace GeViLab.Backend
{
    /// <summary>
    /// A class that provides caching functionality for files, including downloading and saving files from Amazon S3.
    /// </summary>
    public class FileCache : MonoBehaviour
    {
        // A dictionary to store the local files' metadata
        private Dictionary<string, DateTime> metadata;

        void Start() // The constructor
        {
            metadata = new Dictionary<string, DateTime>();
            InitializeMetadata();
        }

        private void InitializeMetadata()
        {
            string cacheDir = Path.Combine(
                Application.persistentDataPath,
                ConfigLoader.config.CacheFolder
            );
            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }

            foreach (string file in Directory.GetFiles(cacheDir))
            {
                AddToCache(file);
                // FileInfo fileInfo = new FileInfo(file);
                // metadata[fileInfo.Name] = fileInfo.LastWriteTimeUtc;
            }
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void ClearCache()
        {
            metadata.Clear();
        }

        // /// <summary>
        // /// Adds a file to the metadata dictionary.
        // /// </summary>
        // /// <param name="key">The key (name) of the file to add.</param>
        // /// <param name="value">The modification time.</param>
        public void AddToCache(string file)
        {
            // metadata[key] = date;
            FileInfo fileInfo = new FileInfo(file);
            metadata[fileInfo.FullName] = fileInfo.LastWriteTimeUtc;
        }

        #region fileHandling
        /// <summary>
        /// Gets a file from the cache or downloads it from S3 if it is not cached or out-of-date.
        /// </summary>
        /// <param name="key">The key of the file to get.</param>
        /// <returns>A Texture2D object representing the downloaded file.</returns>
        public async Task<Texture2D> GetTextureFile(string key)
        {
            // Check if the file exists locally
            if (metadata.ContainsKey(key) && File.Exists(GetLocalPath(key)))
            {
                // Get the last modified date of the file on S3
                var lastModifiedOnS3 = await GetLastModifiedOnS3(key);

                // Compare the last modified date of the local file and the one on S3
                if (metadata[key] >= lastModifiedOnS3)
                {
                    // If the local file is up-to-date, load it directly
                    return LoadLocalTextureFile(key);
                }
            }

            // If the file doesn't exist locally or it's out-of-date, download it
            var texture = await DownloadTextureFile(key);

            // Update the local file and its metadata
            SaveLocalTextureFile(key, texture);
            metadata[key] = DateTime.Now;

            return texture;
        }

        /// <summary>
        /// Returns the local path for a given cache key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>The local path for the cache key.</returns>
        private string GetLocalPath(string key)
        {
            // Return a path that's unique for each key
            return Path.Combine(Application.persistentDataPath, key);
        }

        /// <summary>
        /// Loads a local file as a Texture2D object.
        /// </summary>
        /// <param name="key">The key used to identify the file.</param>
        /// <returns>The Texture2D object loaded from the file.</returns>
        private Texture2D LoadLocalTextureFile(string key)
        {
            // Load the local file and return it
            var bytes = File.ReadAllBytes(GetLocalPath(key));
            var texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            return texture;
        }

        /// <summary>
        /// Saves a Texture2D to a local file with the given key.
        /// </summary>
        /// <param name="key">The key to use for the file name.</param>
        /// <param name="texture">The Texture2D to save.</param>
        private void SaveLocalTextureFile(string key, Texture2D texture)
        {
            // Save the Texture2D to a file
            var bytes = texture.EncodeToPNG();
            File.WriteAllBytes(GetLocalPath(key), bytes);
        }

        private async Task<DateTime> GetLastModifiedOnS3(string key)
        {
            // Get the last modified date of the file on S3
            var response = await BackendAccess.s3Client.GetObjectMetadataAsync(
                BackendAccess.bucketName,
                key
            );
            return response.LastModified;
        }

        /// <summary>
        /// Downloads a file from S3 and returns it as a Texture2D object.
        /// </summary>
        /// <param name="key">The S3 object key of the file to download.</param>
        /// <returns>A Texture2D object representing the downloaded file.</returns>
        private async Task<Texture2D> DownloadTextureFile(string key)
        {
            Texture2D texture = null;
            // Download the file from S3
            var response = await BackendAccess.s3Client.GetObjectAsync(
                BackendAccess.bucketName,
                key
            );

            using (BinaryReader reader = new BinaryReader(response.ResponseStream))
            {
                // byte[] imageData = new byte[count];
                // reader.Read(imageData, 0, count);
                int count = (int)response.ResponseStream.Length;
                byte[] imageData = reader.ReadBytes(count);
                texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
            }
            // var bytes = ReadToEnd(response.ResponseStream);
            // using (var reader = new StreamReader(response.ResponseStream))
            // {
            //     var bytes = reader.ReadToEnd();
            //     byte[] imageData = Encoding.UTF8.GetBytes(bytes);
            //     texture = new Texture2D(2, 2);
            //     texture.LoadImage(imageData);
            // }
            return texture;
        }

        #endregion fileHandling
        private async Task SyncWithRemoteStorageAsync(string bucketName)
        {
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request { BucketName = bucketName };

                ListObjectsV2Response response;
                do
                {
                    response = await BackendAccess.s3Client.ListObjectsV2Async(request);
                    foreach (S3Object entry in response.S3Objects)
                    {
                        if (
                            !metadata.ContainsKey(entry.Key)
                            || metadata[entry.Key] < entry.LastModified
                        )
                        {
                            // await DownloadAndCacheFileAsync(BackendAccess.bucketName, entry.Key);
                        }
                    }

                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Debug.Log($"An error occurred with AWS.S3. Exception: {amazonS3Exception}");
            }
            catch (Exception e)
            {
                Debug.Log($"An unknown error occurred. Exception: {e}");
            }
        }
    }
    /// <summary>
    /// Lists the objects in an S3 bucket with a given prefix and adds them to the cache if they are not already cached.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket to list objects from.</param>
    /// <param name="prefix">The prefix to filter the objects by.</param>
    // public void ListObjects(string bucketName, string prefix)
    // {
    //     ListObjectsV2Request request = new ListObjectsV2Request
    //     {
    //         BucketName = bucketName,
    //         Prefix = prefix
    //     };

    //     using (IAmazonS3 client = new AmazonS3Client())
    //     {
    //         ListObjectsV2Response response = client.ListObjectsV2(request);

    //         foreach (S3Object obj in response.S3Objects)
    //         {
    //             byte[] data;
    //             if (!TryGetFromCache(obj.Key, out data))
    //             {
    //                 GetObjectRequest getRequest = new GetObjectRequest
    //                 {
    //                     BucketName = bucketName,
    //                     Key = obj.Key
    //                 };

    //                 using (GetObjectResponse getResponse = client.GetObject(getRequest))
    //                 {
    //                     using (MemoryStream memoryStream = new MemoryStream())
    //                     {
    //                         getResponse.ResponseStream.CopyTo(memoryStream);
    //                         data = memoryStream.ToArray();
    //                         AddToCache(obj.Key, data);
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }
}
