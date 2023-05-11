using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using B83.Win32;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class FileAnnotationCreator : MonoBehaviour, IOnEventCallback
{
    public GameObject ImagePrefab;
    public float imageSizeFactor = 0.5f;


    private List<string> log = new List<string>();
    void OnEnable()
    {
        // must be installed on the main thread to get the right thread id.
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;

        PhotonNetwork.AddCallbackTarget(this);
    }
    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();

        PhotonNetwork.RemoveCallbackTarget(this);
    }

    void OnFiles(List<string> aFiles, POINT aPos)
    {
        // do something with the dropped file names. aPos will contain the 
        // mouse position within the window where the files has been dropped.
        string str = "Dropped " + aFiles.Count + " files at: " + aPos + "\n\t" +
            aFiles.Aggregate((a, b) => a + "\n\t" + b);
        Debug.Log(str);
        // create position for prefab spawn based on mouse position and forward vector
        Vector3 position = transform.position;
        position += Camera.main.transform.forward;

        string path = aFiles[0];
        // Check if the file exists
        if (File.Exists(path))
        {
            // Check if the file is an image by looking at its extension
            string extension = Path.GetExtension(path).ToLower();
            if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
            {
                // Load the image file as a texture
                Texture2D texture = new Texture2D(2, 2);
                byte[] imageData = File.ReadAllBytes(path);
                texture.LoadImage(imageData);

                // Create a sprite from the texture
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Assign the sprite to the SpriteRenderer component
                GameObject newImage = Instantiate(ImagePrefab, position, Quaternion.identity);
                newImage.GetComponent<SpriteRenderer>().sprite = sprite;
                newImage.transform.localScale *= imageSizeFactor;
                log.Add("File is created as image at" + position + ": " + path);
            }
            else
            {
                Debug.LogError("File is not an image: " + path);
                log.Add("File is not an image: " + path);
            }
        }
        else
        {
            Debug.LogError("File not found at: " + path);
            log.Add("File not found at: " + path);
        }
    }

    private void SendImageFile(byte[] imageData)
    {
        if (imageData != null)
        {
            // Define a custom event code (choose a number between 1 and 199)
            const byte customEventCode = 1;

            // Send the custom event to all players in the room
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(customEventCode, imageData, raiseEventOptions, sendOptions);
        }
    }

    private byte[] ConvertImageToByteArray(string path)
    {
        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        else
        {
            Debug.LogError("File not found at: " + path);
            return null;
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("clear log"))
            log.Clear();
        foreach (var s in log)
            GUILayout.Label(s);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == 1) // This should match the custom event code defined earlier
        {
            byte[] imageData = (byte[])photonEvent.CustomData;
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            // Do something with the texture (e.g., create a sprite and assign it to a SpriteRenderer)
        }
    }
}
