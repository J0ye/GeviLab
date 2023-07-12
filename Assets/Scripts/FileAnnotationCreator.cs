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

        string path = aFiles[0];
        // Check if the file exists
        if (File.Exists(path))
        {
            // Get the file size in bytes
            FileInfo fileInfo = new FileInfo(path);
            long fileSizeInBytes = fileInfo.Length;
            double fileSizeInMB = fileSizeInBytes / (1024.0 * 1024.0); // Convert bytes to MB

            // Check if the file is smaller than 1.1 MB
            if (fileSizeInMB > 1.1)
            {
                LogCreator.instance.AddLog("File is: " + fileSizeInMB + " Mb. The file size limit is 1.1 Mb");
                return;
            }


            // Check if the file is an image by looking at its extension
            string extension = Path.GetExtension(path).ToLower();
            if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
            {
                byte[] imageData = File.ReadAllBytes(path);
                ImageAnnotation.SpawnImageWithStaticPosition(imageData);
            }


            if (extension == ".mp4")
            {
                LogCreator.instance.AddLog("Loading video");
                LogCreator.instance.AddLog("Loading video");
                byte[] videoData = File.ReadAllBytes(path);
                VideoAnnotation.SpawnVideoAndSend(videoData);
            }
        }
        else
        {
            Debug.LogError("File not found at: " + path);
            LogCreator.instance.AddLog("File not found at: " + path);
        }
    }

    public void UseInjectedSpawn()
    {
        VideoAnnotation.InjectedSpawn();
    }
}
