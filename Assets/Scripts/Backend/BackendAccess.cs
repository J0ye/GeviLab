using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GeViLab.Backend
{
    public class BackendAccess : MonoBehaviour
    {
        private static BackendAccess instance;

        public static BackendAccess Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<BackendAccess>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(BackendAccess).Name;
                        instance = obj.AddComponent<BackendAccess>();
                    }
                }
                return instance;
            }
        }
        // private const string accessKey = "your-access-key";
        // private const string secretKey = "your-secret-key";
        // private static string accessKey;
        // private static string secretKey;
        // public static string bucketName;
        private static RegionEndpoint bucketRegion = RegionEndpoint.EUCentral1;

        public static IAmazonS3 s3Client;

        [SerializeField]
        private int timeout = 10000;

        // [SerializeField]
        // private static string storagePath;

        public static void Initialize()
        {
            bucketRegion = RegionEndpoint.GetBySystemName(ConfigLoader.config.AWSS3Region);

            s3Client = new AmazonS3Client(ConfigLoader.config.AWSAccessKey, ConfigLoader.config.AWSSecretKey, bucketRegion);
        }

        // static async Task ReadObjectData()
        // {
        //     try
        //     {
        //         /// <summary>
        //         /// Represents a request to retrieve a text object from an Amazon S3 bucket.
        //         /// </summary>
        //         GetObjectRequest request = new GetObjectRequest
        //         {
        //             BucketName = bucketName,
        //             Key = "gude-2023-01-08.log" // "Screenshot 2023-03-24 112335.png"
        //         };
        //         Debug.Log($"Reading {request.Key} from {request.BucketName} ....");
        //         using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
        //         using (Stream responseStream = response.ResponseStream)
        //         using (StreamReader reader = new StreamReader(responseStream))
        //         {
        //             string contents = reader.ReadToEnd();
        //             Debug.Log(contents);
        //         }
        //     }
        //     catch (AmazonS3Exception e)
        //     {
        //         Debug.Log(
        //             "Error encountered on server. Message:'" + e.Message + "' when reading object"
        //         );
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.Log(
        //             "Unknown encountered on server. Message:'" + e.Message + "' when reading object"
        //         );
        //     }
        // }

        // DownloadObjectFromBucketAsync(s3Client, bucketName,"Screenshot 2023-03-24 112335.png",storagePath).Wait(timeout);
        public static async Task<bool> DownloadObjectFromBucketAsync(string objectName, string storagePath)
        {
            // Create a GetObject request
            var request = new GetObjectRequest { BucketName = ConfigLoader.config.AWSS3Bucket, Key = objectName, };

            // Issue request and remember to dispose of the response
            Debug.Log($"Reading {request.Key} from {request.BucketName} ....");
            using GetObjectResponse response = await s3Client.GetObjectAsync(request);

            Debug.Log($"Saving {objectName} to {storagePath} ....");
            try
            {
                // Save object to local file
                await response.WriteResponseStreamToFileAsync(
                    $"{storagePath}\\{objectName}",
                    true,
                    CancellationToken.None
                );
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error saving {objectName}: {ex.Message}");
                return false;
            }
            catch (Exception e)
            {
                Debug.Log(
                    "Unknown encountered on server. Message:'" + e.Message + "' when reading object"
                );
                return false;
            }
        }

        // UploadFileAsync("test.png", "image/png").Wait(timeout);
        public static async Task<bool> UploadFileAsync(string storagePath, string objectName, string contentType)
        {
            try
            {
                var filePath = Path.Combine(storagePath, objectName);
                Debug.Log($"Uploading {filePath} to {ConfigLoader.config.AWSS3Bucket} ....");
                var fileStream = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read
                );

                var putRequest = new PutObjectRequest
                {
                    BucketName = ConfigLoader.config.AWSS3Bucket,
                    Key = Path.GetFileName(filePath),
                    InputStream = fileStream,
                    ContentType = contentType
                };
                // Debug.Log($"Uploading {putRequest.Key} to {putRequest.BucketName} ....");
                PutObjectResponse response = await s3Client.PutObjectAsync(putRequest);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (AmazonS3Exception e)
            {
                Debug.Log(
                    "Error encountered on server. Message:'"
                        + e.Message
                        + "' when writing an object"
                );
                return false;
            }
            catch (Exception e)
            {
                Debug.Log(
                    "Unknown encountered on server. Message:'"
                        + e.Message
                        + "' when writing an object"
                );
                return false;
            }
        }
    }

    // public static RegionEndpoint GetBySystemName(string systemName)
    // {
    //     if (_hashBySystemName.TryGetValue(systemName, out regionEndpoint))
    //         return regionEndpoint;
    //     return regionEndpoint.EUCentral1;
    // }
}
