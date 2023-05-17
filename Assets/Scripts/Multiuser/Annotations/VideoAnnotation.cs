using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoAnnotation : Annotation
{
    public static void SpawnVideo(byte[] data)
    {
        GameObject videoPlayerPrefab = Resources.Load<GameObject>("VideoPrefab");
        // Instantiate a new video player GameObject
        GameObject videoPlayerObject = Instantiate(videoPlayerPrefab, Vector3.zero, Quaternion.identity);
        videoPlayerObject.transform.position = GetPositionInFront();
        // Get VideoPlayer component to the new GameObject
        VideoPlayer videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
        // Create temp file for playing the video
        string tempFilePath = Path.Combine(Application.temporaryCachePath, "tempVideo.mp4");
        File.WriteAllBytes(tempFilePath, data);

        // Set the video file path
        videoPlayer.url = tempFilePath;

        // Create a new RenderTexture
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
        videoPlayer.targetTexture = renderTexture;

        // Assign the RenderTexture to the Quad
        Renderer quadRenderer = videoPlayerObject.GetComponent<Renderer>();
        if (quadRenderer != null)
        {
            quadRenderer.material.mainTexture = renderTexture;
        }

        // Set up the prepareCompleted event
        videoPlayer.prepareCompleted += (source) =>
        {
            // Adjust the Quad's scale to match the RenderTexture's aspect ratio
            float aspectRatio = (float)source.width / source.height;
            videoPlayerObject.transform.localScale = new Vector3(aspectRatio, 1f, 1f);

            // Adjust the RenderTexture's size to match the video's dimensions
            renderTexture.width = (int)source.width;
            renderTexture.height = (int)source.height;

            // Start playing the video
            source.Play();
        };

        // Prepare the VideoPlayer (this triggers the prepareCompleted event when done)
        videoPlayer.Prepare();
    }
}
