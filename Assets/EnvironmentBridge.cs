using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBridge : MonoBehaviour
{
    public Transform destination;

    [Header("Avatars")]
    public GameObject playerAvatar;
    public GameObject playerXRAvatar;

    protected Vector3 originPosition;

    private void Awake()
    {
        originPosition = transform.position;
    }

    public void MoveToDestination()
    {
        MoveAvatars(destination.position);
    }

    public void MoveToOrigin()
    {
        MoveAvatars(originPosition);
    }

    protected void MoveAvatars(Vector3 targetPos)
    {
        playerAvatar.transform.position = targetPos;
        playerXRAvatar.transform.position = targetPos;
    }
}
