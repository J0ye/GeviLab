using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Adds content for video as string (path to video). Is in FullscreenAnnotation.cs
/// </summary>
public class FullscreenVideo : FullscreenAnnotation
{
    public VideoPlayer content;

    public void SetUpVideo()
    {
        // Set the video to loop
        content.isLooping = true;

        // Set up the prepareCompleted event
        content.prepareCompleted += (source) =>
        {
            // Create RenderTexture for VideoPlayer
            RenderTexture renderTexture = new RenderTexture((int)source.width, (int)source.height, 24); // change dimensions
            content.targetTexture = renderTexture;

            // Assign RenderTexture to RawImage
            content.GetComponent<RawImage>().texture = renderTexture;
            // convert int values to float
            float tempWidth = renderTexture.width;
            float tempHeigth = renderTexture.height;
            content.GetComponent<AspectRatioFitter>().aspectRatio = tempWidth / tempHeigth;
            // Start playing the video
            source.Play();
        };

        // Prepare the VideoPlayer (this triggers the prepareCompleted event when done)
        content.Prepare();
    }
}
