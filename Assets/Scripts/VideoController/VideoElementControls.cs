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
    public static VideoElementControls instance;

    [Tooltip("This object is a reference to the object displaying the video/the object that has the render texture as part of its material. The object that is the virtual screen. Name: 'videoScreen'")]
    public GameObject videoScreen;
    [Tooltip("This is a reference of the video player, which is displayed via the render texture on the virtual video screen. Name: 'videoPlayer'")]
    public VideoPlayer videoPlayer;
    [Tooltip("This is a reference to the main camera or player camera. The object with the camera component: 'playerCamera'")]
    public GameObject playerCamera;
    [Tooltip("This is a reference to the elements of the UI that are used to control video playback, i.e. Play, Pause, Forward, Backwards. Name: 'videoPlayerUI'")]
    public GameObject videoPlayerUI;

    private POVCamera pov;
    private NetworkPointerControls pointerControls;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        pov = playerCamera.GetComponent<POVCamera>();
        pointerControls = playerCamera.GetComponent<NetworkPointerControls>();
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
            case "playerCamera":
                playerCamera.SetActive(!playerCamera.activeSelf);
                break;
            case "videoPlayerUI":
                videoPlayerUI.SetActive(!videoPlayerUI.activeSelf);
                break;
        }
    }

    /// <summary>
    /// Will enable or disable the user controls of the video player, the ray pointer and the camera controls.
    /// </summary>
    /// <param name="val"> True = enable. False = disable all</param>
    public void AblePlayerControls(bool val)
    {
        pov.enabled = val;
        pointerControls.enabled = val;

        if(GameState.instance.roleState.playerAuthority)
        {
            // Do only change the video player ui if the user has the right role
            videoPlayerUI.SetActive(val);
        }
    }
}