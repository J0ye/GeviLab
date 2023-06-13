using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annotation : MonoBehaviourPunCallbacks
{
    public bool permanent = false;
    public GameObject fullscreenPrefab;
    public GameObject fullscreenPrefabXR;

    [HideInInspector]
    public bool isEditing = false;

    public void OnMouseDown()
    {
        if (!isEditing)
        {
            Open();
        }
    }

    public void SetDisplay(bool state)
    {
        gameObject.SetActive(state);
    }

    public virtual void Open()
    {
        GameObject newFullscreenAnnotation = Instantiate(fullscreenPrefab, Vector3.zero, Quaternion.identity);
        GameState.instance.SetActivePlayerControls(false);
    }

    public virtual void OpenXR()
    {
        GameObject newFullscreenAnnotation = Instantiate(fullscreenPrefabXR, GetPositionInFront(), Quaternion.identity);
        GameState.instance.SetActivePlayerControls(false);
    }

    public void DeleteAnnotation()
    {
        photonView.RPC("DeleteAnnotationRemote", RpcTarget.Others);
        Destroy(gameObject);
    }

    public static Vector3 GetPositionInFront()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }

        return Vector3.one;
    }

    [PunRPC]
    public void DeleteAnnotationRemote()
    {
        Destroy(gameObject);
    }
}
