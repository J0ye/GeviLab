using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using B83.Win32;

public class FileAnnotationCreator : MonoBehaviour
{
    private List<string> log = new List<string>();

    void OnEnable()
    {
        // must be installed on the main thread to get the right thread id.
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;
    }

    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }

    void OnFiles(List<string> aFiles, POINT aPos)
    {
        // do something with the dropped file names. aPos will contain the 
        // mouse position within the window where the files has been dropped.
        string str = "Dropped " + aFiles.Count + " files at: " + aPos + "\n\t" +
            aFiles.Aggregate((a, b) => a + "\n\t" + b);
        Debug.Log(str);

        string path = aFiles[0];
        // Check if the file exists
        if (File.Exists(path))
        {
            // Check if the file is an image by looking at its extension
            string extension = Path.GetExtension(path).ToLower();
            if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
            {
                byte[] imageData = File.ReadAllBytes(path);
                ImageAnnotation.SpawnImageWithStaticPosition(imageData);
            }


            if (extension == ".mp4")
            {
                print("Loading video");
                log.Add("Loading video");
                byte[] videoData = File.ReadAllBytes(path);
                VideoAnnotation.SpawnVideo(videoData);
            }
        }
        else
        {
            Debug.LogError("File not found at: " + path);
            log.Add("File not found at: " + path);
        }
    }

    public void UseInjectedSpawn()
    {
        ImageAnnotation.InjectedSpawn();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("clear log"))
            log.Clear();
        foreach (var s in log)
            GUILayout.Label(s);
    }
}
