using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annotation : MonoBehaviourPunCallbacks
{
    public GameObject fullscreenPrefab;

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

    public void DeleteAnnotation()
    {
        photonView.RPC("DeleteAnnotationRemote", RpcTarget.Others);
        Destroy(gameObject);
    }

    [PunRPC]
    public void DeleteAnnotationRemote()
    {
        Destroy(gameObject);
    }
}
