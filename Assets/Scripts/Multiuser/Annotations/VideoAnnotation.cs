using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoAnnotation : Annotation, IOnEventCallback
{
    #region PUN Event
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    /// <summary>
    /// Needs to be implemented because of interface I=nEventCallback. Handles Photon events based on event code.
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == 71) // This should match the custom event code defined earlier
        {
            object[] eventData = (object[])photonEvent.CustomData;
            byte[] data = (byte[])eventData[0];
            Vector3 vectorData = (Vector3)eventData[1];

            ChangeVideo(data, gameObject);
        }
    }
    #endregion
    /// <summary>
    /// Path to the video.
    /// </summary>
    public string content;

    public static void SpawnVideoAndSend(byte[] data)
    {
        SpawnNetworkVideo(data);

        Vector3 vectorData = GetPositionInFrontOfCamera();
        object[] eventData = new object[] { data, vectorData };
        SendVideoFileAndVector(eventData);
    }

    public static void SpawnVideo(byte[] data)
    {
        GameObject videoPlayerPrefab = Resources.Load<GameObject>("VideoPrefab");
        // Instantiate a new video player GameObject
        GameObject videoPlayerObject = Instantiate(videoPlayerPrefab, GetPositionInFrontOfCamera(), Quaternion.identity);
        ChangeVideo(data, videoPlayerObject);
    }

    public static void SpawnNetworkVideo(byte[] data)
    {
        // Create new video via network
        GameObject newVideo = PhotonNetwork.Instantiate("VideoPrefab", GetPositionInFrontOfCamera(), Quaternion.identity);
        ChangeVideo(data, newVideo);
    }

    public static void ChangeVideo(byte[] data, GameObject target)
    {
        // Get VideoPlayer component to the new GameObject
        VideoPlayer videoPlayer = target.GetComponent<VideoPlayer>();
        // Create temp file for playing the video
        string tempVideoName = "tempVideo" + Guid.NewGuid().ToString() + ".mp4";
        string tempFilePath = Path.Combine(Application.temporaryCachePath, tempVideoName);
        File.WriteAllBytes(tempFilePath, data);

        // Set the video file path
        videoPlayer.url = tempFilePath;
        videoPlayer.GetComponent<VideoAnnotation>().content = tempFilePath;

        // Create a new RenderTexture
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
        videoPlayer.targetTexture = renderTexture;

        // Assign the RenderTexture to the Quad
        Renderer quadRenderer = target.GetComponent<Renderer>();
        if (quadRenderer != null)
        {
            quadRenderer.material.mainTexture = renderTexture;
        }

        // Set up the prepareCompleted event
        videoPlayer.prepareCompleted += (source) =>
        {
            // Adjust the Quad's scale to match the RenderTexture's aspect ratio
            float aspectRatio = (float)source.width / source.height;
            target.transform.localScale = new Vector3(aspectRatio, 1f, 1f);

            // Adjust the RenderTexture's size to match the video's dimensions
            renderTexture.width = (int)source.width;
            renderTexture.height = (int)source.height;

            // Start playing the video
            source.Play();
        };

        // Prepare the VideoPlayer (this triggers the prepareCompleted event when done)
        videoPlayer.Prepare();

        VideoAnnotation targetAnnotation = target.GetComponent<VideoAnnotation>();
        //Remove event listener of tharget image annotation. Images will not be changed twice.
        PhotonNetwork.RemoveCallbackTarget(targetAnnotation);
    }

    public static void SendVideoFileAndVector(object[] eventData)
    {
        // Define a custom event code
        const byte customEventCode = 71;
        // Send the custom event to all players in the room
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(customEventCode, eventData, raiseEventOptions, sendOptions);
    }

    public static void InjectedSpawn()
    {
        GameObject videoPlayerPrefab = Resources.Load<GameObject>("VideoPrefab");
        // Instantiate a new video player GameObject
        GameObject videoPlayerObject = Instantiate(videoPlayerPrefab, GetPositionInFrontOfCamera(), Quaternion.identity);
        // Get VideoPlayer component to the new GameObject
        VideoPlayer videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();

        VideoClip tempClip = Resources.Load<VideoClip>("TestVideo");;
        ChangeVideo(VideoClipToByteArray(tempClip, 24f), videoPlayerObject);
    }

    /// <summary>
    /// Spawns a new fullscreen video prefab and hides this element.
    /// </summary>
    public override void Open()
    {
        GameObject newFullscreenVideo = Instantiate(fullscreenPrefab, Vector3.zero, Quaternion.identity);
        FullscreenVideo fv = newFullscreenVideo.GetComponent<FullscreenVideo>();
        // Set the video path
        fv.content.url = content;
        fv.SetUpVideo();
        fv.origin = this;
        GameState.instance.SetActivePlayerControls(false);
    }

    public static byte[] VideoClipToByteArray(VideoClip vc, float frameRate)
    {
        byte[] data = File.ReadAllBytes(vc.originalPath);
        return data;
    }
}
