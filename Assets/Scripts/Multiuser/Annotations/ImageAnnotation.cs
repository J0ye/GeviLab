using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ImageAnnotation : Annotation, IOnEventCallback
{
    public Sprite content;
    public static float imageSizeFactor = 0.1f;
    #region PUN Event
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == 69) // This should match the custom event code defined earlier
        {
            byte[] imageData = (byte[])photonEvent.CustomData;

            ChangeImage(imageData, GetPositionForImage());
        }
        else if (eventCode == 70) // This should match the custom event code defined earlier
        {
            object[] eventData = (object[])photonEvent.CustomData;
            byte[] imageData = (byte[])eventData[0];
            Vector3 vectorData = (Vector3)eventData[1];

            ChangeImage(imageData, vectorData);
        }

        PhotonNetwork.RemoveCallbackTarget(this);
    }
    #endregion
    public static void SpawnImageWithStaticPosition(byte[] imageData)
    {
        SpawnImageAndSend(imageData, GetPositionForImage());
    }

    public static void SpawnImageAndSend(byte[] imageData, Vector3 position)
    {
        SpawnNetworkImage(imageData, position);

        Vector3 vectorData = position;
        object[] eventData = new object[] { imageData, vectorData };
        SendImageFileAndVector(eventData);
    }

    public static void SpawnImage(byte[] imageData, Vector3 position)
    {
        // Load the image file as a texture
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        // Create a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        GameObject imagePrefab = Resources.Load<GameObject>("ImagePrefab");

        // Assign the sprite to the SpriteRenderer component
        GameObject newImage = Instantiate(imagePrefab, position, Quaternion.identity);
        newImage.GetComponent<SpriteRenderer>().sprite = sprite;
        newImage.GetComponent<ImageAnnotation>().content = sprite;
        newImage.transform.localScale *= imageSizeFactor;
    }

    public static void SpawnNetworkImage(byte[] imageData, Vector3 position)
    {
        // Load the image file as a texture
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        // Create a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Assign the sprite to the SpriteRenderer component
        GameObject newImage = PhotonNetwork.Instantiate("ImagePrefab", position, Quaternion.identity);
        newImage.GetComponent<SpriteRenderer>().sprite = sprite;
        newImage.GetComponent<ImageAnnotation>().content = sprite;
        newImage.transform.localScale *= imageSizeFactor;
    }

    public void ChangeImage(byte[] imageData, Vector3 position)
    {
        // Load the image file as a texture
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        // Create a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        GetComponent<SpriteRenderer>().sprite = sprite;
        GetComponent<ImageAnnotation>().content = sprite;
    }

    public static void SendImageFile(byte[] imageData)
    {
        // Define a custom event code (choose a number between 1 and 199)
        const byte customEventCode = 69;
        if (imageData != null)
        {
            // Send the custom event to all players in the room
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(customEventCode, imageData, raiseEventOptions, sendOptions);
        }
    }

    public static void SendImageFileAndVector(object[] eventData)
    {
        // Define a custom event code (choose a number between 1 and 199)
        const byte customEventCode = 70;
        // Send the custom event to all players in the room
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(customEventCode, eventData, raiseEventOptions, sendOptions);
    }

    public override void Open()
    {
        GameObject newFullscreenImage = Instantiate(fullscreenPrefab, Vector3.zero, Quaternion.identity);
        FullscreenImage fn = newFullscreenImage.GetComponent<FullscreenImage>();
        fn.content.sprite = content;
        fn.content.GetComponent<AspectRatioFitter>().aspectRatio = content.texture.width / content.texture.height;
        fn.origin = this;
        GameState.instance.SetActivePlayerControls(false);
    }

    private static Vector3 GetPositionForImage()
    {
        // create position for prefab spawn based on mouse position and forward vector
        Vector3 position = Camera.main.transform.position;
        position += Camera.main.transform.forward;

        return position;
    }
}
