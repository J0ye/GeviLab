using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBridge : MonoBehaviour
{
    public Transform destination;
    [Tooltip("Used to change ui buttons to new video player. leave empty if new environment does not have a player")]
    public NetworkVideoPlayerControls destinationNVPC;

    [Header("Avatars")]
    public GameObject playerAvatar;
    public GameObject playerXRAvatar;

    protected Vector3 originPosition;
    protected NetworkVideoPlayerControls originNVPC;

    private void Awake()
    {
        originPosition = transform.position;
        foreach(Transform child in transform)
        {
            // 'One of those kids should have a NetworkVideoPlayerControls component. Because thats how normal prefabs are.'
            transform.GetChild(0).TryGetComponent<NetworkVideoPlayerControls>(out originNVPC);
        }
    }

    /// <summary>
    /// Moves both avatar types to the target destination. This also pauses any active video player
    /// </summary>
    public void MoveToDestination()
    {
        MoveAvatars(destination.position);
        if(originNVPC != null) originNVPC.Pause();
        GameState.instance.SwitchButtonFunctionsInMenu(destinationNVPC);
    }

    /// <summary>
    /// Moves both avatar types back to the position of this component. This also pauses any active video player
    /// </summary>
    public void MoveToOrigin()
    {
        MoveAvatars(originPosition);
        if (destinationNVPC != null) destinationNVPC.Pause();
        GameState.instance.SwitchButtonFunctionsInMenu(originNVPC);
    }

    protected void MoveAvatars(Vector3 targetPos)
    {
        playerAvatar.transform.position = targetPos;
        playerXRAvatar.transform.position = targetPos;
    }
}
