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
    /// <summary>
    /// Needs to be implemented because of interface I=nEventCallback. Handles Photon events based on event code.
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == 69) // This should match the custom event code defined earlier
        {
            byte[] imageData = (byte[])photonEvent.CustomData;

            ChangeImage(imageData);
        }
        else if (eventCode == 70) // This should match the custom event code defined earlier
        {
            object[] eventData = (object[])photonEvent.CustomData;
            byte[] imageData = (byte[])eventData[0];
            Vector3 vectorData = (Vector3)eventData[1];

            ChangeImage(imageData);
        }
    }
    #endregion
    public static void SpawnImageWithStaticPosition(byte[] imageData)
    {
        SpawnImageAndSend(imageData, GetPositionInFront());
    }

    /// <summary>
    /// Fully creates a netwrok image obejct for every user and assignes the image data as texture to each object.
    /// </summary>
    /// <param name="imageData"></param>
    /// <param name="position"></param>
    public static void SpawnImageAndSend(byte[] imageData, Vector3 position)
    {
        SpawnNetworkImage(imageData, position);

        Vector3 vectorData = position;
        object[] eventData = new object[] { imageData, vectorData };
        SendImageFileAndVector(eventData);
    }

    /// <summary>
    /// Spawns the image only for this client. No networking code.
    /// </summary>
    /// <param name="imageData"></param>
    /// <param name="position"></param>
    public static void SpawnImage(byte[] imageData, Vector3 position)
    {
        GameObject imagePrefab = Resources.Load<GameObject>("ImagePrefab");
        GameObject newImage = Instantiate(imagePrefab, position, Quaternion.identity);
        // Assign the sprite to the SpriteRenderer component
        ChangeImage(imageData, newImage);
        newImage.transform.localScale *= imageSizeFactor;

        // Get the BoxCollider and MeshFilter components
        BoxCollider boxCollider = newImage.GetComponent<BoxCollider>();
        SpriteRenderer spriteRenderer = newImage.GetComponent<SpriteRenderer>();
        if (boxCollider && spriteRenderer)
        {
            // Set the size equal to the sprite
            boxCollider.size = spriteRenderer.sprite.bounds.size;
        }
    }

    /// <summary>
    /// Spawns the Image prefab as a Photon networked object and changes the local image based on image data.
    /// </summary>
    /// <param name="imageData"></param>
    /// <param name="position"></param>
    public static void SpawnNetworkImage(byte[] imageData, Vector3 position)
    {
        // Create new image via network
        GameObject newImage = PhotonNetwork.Instantiate("ImagePrefab", position, Quaternion.identity);
        // Assign the sprite to the SpriteRenderer component
        ChangeImage(imageData, newImage);
        newImage.transform.localScale *= imageSizeFactor;



        // Get the BoxCollider and MeshFilter components
        BoxCollider boxCollider = newImage.GetComponent<BoxCollider>();
        SpriteRenderer spriteRenderer = newImage.GetComponent<SpriteRenderer>();
        if (boxCollider && spriteRenderer)
        {
            // Set the size equal to the sprite
            boxCollider.size = spriteRenderer.sprite.bounds.size;
        }
    }

    /// <summary>
    /// Changes the image of a sprite renderer component on this object to the image based on data. The position data is ignored for now.
    /// </summary>
    /// <param name="imageData">Image data as byte array</param>
    public void ChangeImage(byte[] imageData)
    {
        // Load the image file as a texture
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        // Create a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        GetComponent<SpriteRenderer>().sprite = sprite;
        GetComponent<ImageAnnotation>().content = sprite;

        //Remove event listener of this image annotation. Images will not be changed twice.
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    /// <summary>
    /// Static variant of the ChangeImage function. Removes event listener.
    /// </summary>
    /// <param name="imageData"></param>
    /// <param name="target"></param>
    public static void ChangeImage(byte[] imageData, GameObject target)
    {
        // Load the image file as a texture
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        // Create a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        target.GetComponent<SpriteRenderer>().sprite = sprite;
        ImageAnnotation targetAnno = target.GetComponent<ImageAnnotation>();
        targetAnno.content = sprite;

        //Remove event listener of tharget image annotation. Images will not be changed twice.
        PhotonNetwork.RemoveCallbackTarget(targetAnno);
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
        transform.position = position;
    }


    /// <summary>
    /// Raises the data for an image as an event with event code 69. Does not include information about position.
    /// </summary>
    /// <param name="imageData"></param>
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

    /// <summary>
    /// Raises the data for image and image position as a Photon event. Event code is 70.
    /// object[] eventData = new object[] { imageData, vectorData };
    /// </summary>
    /// <param name="eventData">Should be an array. first is image and second should be vector data.</param>
    public static void SendImageFileAndVector(object[] eventData)
    {
        // Define a custom event code (choose a number between 1 and 199)
        const byte customEventCode = 70;
        // Send the custom event to all players in the room
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(customEventCode, eventData, raiseEventOptions, sendOptions);
    }

    /// <summary>
    /// Spawns a new fullscreen image prefab and hides this one.
    /// </summary>
    public override void Open()
    {
        GameObject newFullscreenImage = Instantiate(fullscreenPrefab, Vector3.zero, Quaternion.identity);
        FullscreenImage fn = newFullscreenImage.GetComponent<FullscreenImage>();
        fn.content.sprite = content;
        fn.content.GetComponent<AspectRatioFitter>().aspectRatio = content.texture.width / content.texture.height;
        fn.origin = this;
        GameState.instance.SetActivePlayerControls(false);
    }

    public static void InjectedSpawn()
    {
        Sprite sprite = Resources.Load<Sprite>("TestImage");
        // Assign the sprite to the SpriteRenderer component
        GameObject newImage = PhotonNetwork.Instantiate("ImagePrefab", GetPositionInFront(), Quaternion.identity);
        newImage.transform.localScale *= imageSizeFactor;
        ImageAnnotation targetAnno = newImage.GetComponent<ImageAnnotation>();
        targetAnno.content = sprite;

        //Remove event listener of tharget image annotation. Images will not be changed twice.
        PhotonNetwork.RemoveCallbackTarget(targetAnno);

        BoxCollider boxCollider = newImage.GetComponent<BoxCollider>();
        SpriteRenderer spriteRenderer = newImage.GetComponent<SpriteRenderer>();
        if (boxCollider && spriteRenderer)
        {
            // Set the size equal to the sprite
            boxCollider.size = spriteRenderer.sprite.bounds.size;
        }
    }

    /// <summary>
    /// [Depricated] Returns the position in front of the camera one unit away. Use GetPositionInFront() instead.
    /// </summary>
    /// <returns>Postion in front of the main camera</returns>
    private static Vector3 GetPositionForImage()
    {
        // create position for prefab spawn based on mouse position and forward vector
        Vector3 position = Camera.main.transform.position;
        position += Camera.main.transform.forward;

        return position;
    }
}
