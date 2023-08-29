using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaAnnotation : Annotation
{
    public GameObject fullscreenPrefab;
    [Header("XR Settings")]
    public GameObject fullscreenPrefabXR;
    public float xRPrefabDistance = 0.5f;
    public float xRPrefabSize = 0.5f;
    public float xRPrefabAnimationDuration = 0.5f;

    [HideInInspector]
    public bool isEditing = false;

    public override void Open()
    {
        GameObject newFullscreenAnnotation = Instantiate(fullscreenPrefab, Vector3.zero, Quaternion.identity);
        GameState.instance.SetActivePlayerControls(false);
    }
    public override void OpenXR()
    {
        GameObject newFullscreenAnnotation = Instantiate(fullscreenPrefabXR, GetPositionInFrontOfCamera(), Quaternion.identity);
        GameState.instance.SetActivePlayerControls(false);
    }

    public static Vector3 GetPositionInFrontOfCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }

        return Vector3.one;
    }

    public Vector3 GetPositionInFrontOfAnnotation()
    {
        return transform.position + (transform.forward * xRPrefabDistance);
    }

}
