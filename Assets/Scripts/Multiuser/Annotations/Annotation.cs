using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnnotationType { Image, Note, Gallery, Basic};

public class Annotation : MonoBehaviourPunCallbacks
{
    public bool permanent = false;

    public virtual AnnotationType annotationType { get => AnnotationType.Basic; protected set => annotationType = value; }

    protected Material standardMat;
    public void SetDisplay(bool state)
    {
        gameObject.SetActive(state);
    }

    public void OnMouseDown()
    {
        Open();
    }

    public virtual void Open()
    {
    }

    public virtual void OpenXR()
    {
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
