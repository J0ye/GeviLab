using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Special script to control function and elements that are part of the video element of the GeviLab.
/// Is implemented as a singelton so that there is never more than one video element/player per scene
/// </summary>
public class VideoElementControls : MonoBehaviour
{

    [Tooltip("This object is a reference to the object displaying the video/the object that has the render texture as part of its material. The object that is the virtual screen. Name: 'videoScreen'")]
    public GameObject videoScreen;
    [Tooltip("This is a reference of the video player, which is displayed via the render texture on the virtual video screen. Name: 'videoPlayer'")]
    public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Awake()
    {
    }

    public void SwitchActiveStateOf(string name)
    {
        switch (name)
        {
            case "videoScreen":
                videoScreen.SetActive(!videoScreen.activeSelf);
                break;
            case "videoPlayer":
                videoPlayer.enabled = !videoPlayer.enabled;
                break;
        }
    }

    /// <summary>
    /// Will enable or disable the user controls of the video player, the ray pointer and the camera controls.
    /// </summary>
    /// <param name="val"> True = enable. False = disable all</param>
    public void AblePlayerControls(bool val)
    {
    }
}