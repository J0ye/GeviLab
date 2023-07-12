using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnnotationType { Image, Note, Gallery, Basic};

public class Annotation : MonoBehaviourPunCallbacks
{
    public bool permanent = false;
    public GameObject fullscreenPrefab;
    [Header("XR Settings")]
    public GameObject fullscreenPrefabXR;
    public float xRPrefabDistance = 0.5f;
    public float xRPrefabSize = 0.5f;
    public float xRPrefabAnimationDuration = 0.5f;

    [HideInInspector]
    public bool isEditing = false;

    public virtual AnnotationType annotationType { get => AnnotationType.Basic; protected set => annotationType = value; }

    protected Material standardMat;

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
        GameObject newFullscreenAnnotation = Instantiate(fullscreenPrefabXR, GetPositionInFrontOfCamera(), Quaternion.identity);
        GameState.instance.SetActivePlayerControls(false);
    }

    public void Highlight()
    {
        //standardMat = GetComponent<Renderer>().material;
        //GetComponent<Renderer>().material = Resources.Load<Material>("Highlight");
    }

    public void UnHighlight()
    {
        //GetComponent<Renderer>().material = standardMat
    }

    public void DeleteAnnotation()
    {
        photonView.RPC("DeleteAnnotationRemote", RpcTarget.Others);
        Destroy(gameObject);
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

    /// <summary>
    /// Checks each child of target object if their name contains 'Delete'. 
    /// Changes able state to bool newState
    /// </summary>
    /// <param name="fullscreenTarget">target collection of gameobjects</param>
    /// <param name="newState">New state of each child with 'Delete' in their name</param>
    protected void SetStateForDeleteInteractors(GameObject fullscreenTarget, bool newState)
    {
        foreach(Transform child in fullscreenTarget.transform)
        {
            if(child.name.Contains("Delete"))
            {
                child.gameObject.SetActive(newState);
            }
        }
    }

    [PunRPC]
    public void DeleteAnnotationRemote()
    {
        Destroy(gameObject);
    }
}
