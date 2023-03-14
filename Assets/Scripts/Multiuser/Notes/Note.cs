using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Note : MonoBehaviourPunCallbacks
{
    public GameObject noteInputPrefab;
    public GameObject fullscreenPrefab;
    public TMP_Text titel;

    [HideInInspector]
    public bool isEditing = false;

    private string text;

    public static Note CreateNote(string noteTitel, string content, Vector3 position, Vector3 lookAt)
    {
        GameObject notePrefab = Resources.Load<GameObject>("NoteObject");
        GameObject newNote = Instantiate(notePrefab, position, Quaternion.identity);
        newNote.transform.LookAt(2 * position - lookAt);
        Note noteComponent = newNote.GetComponent<Note>();
        noteComponent.titel.text = noteTitel;
        noteComponent.text = content;
        noteComponent.OpenContentEdit();

        return noteComponent;
    }

    public static Note CreateNetworkedNote(string noteTitel, string content, Vector3 position, Vector3 lookAt)
    {
        GameObject newNote = PhotonNetwork.Instantiate("NoteObject", position, Quaternion.identity);
        newNote.transform.LookAt(2 * position - lookAt);
        Note noteComponent = newNote.GetComponent<Note>();
        noteComponent.titel.text = noteTitel;
        noteComponent.text = content;
        noteComponent.OpenContentEdit();
        print(noteComponent.isEditing);

        return noteComponent;

    }

    public void OnMouseDown()
    {
        print(isEditing);
        if(!isEditing)
        {
            OpenNote();
        }
    }

    public void OpenNote()
    {
        GameObject newFullscreenNote = Instantiate(fullscreenPrefab, Vector3.zero, Quaternion.identity);
        FullscreenNote fn = newFullscreenNote.GetComponent<FullscreenNote>();
        fn.content.text = titel.text + "\n" + text;
        fn.origin = this;
        GameState.instance.SetActivePlayerControls(false);
    }

    public void OpenContentEdit()
    {
        GameObject newNoteInput= Instantiate(noteInputPrefab, Vector3.zero, Quaternion.identity);
        NoteInput ni = newNoteInput.GetComponent<NoteInput>();
        ni.SetOrigin(this);
        GameState.instance.SetActivePlayerControls(false);
    }

    public void SetDisplay(bool state)
    {
        gameObject.SetActive(state);
    }

    public void SetHeader(string val)
    {
        titel.text = val;
        photonView.RPC("UpdateHeaderRemote", RpcTarget.Others, val);
    }

    public void SetContent(string val)
    {
        text = val;
        photonView.RPC("UpdateContentRemote", RpcTarget.Others, val);
    }

    public void DeleteNote()
    {
        photonView.RPC("DeleteNoteRemote", RpcTarget.Others);
        Destroy(gameObject);
    }

    #region PUNs

    [PunRPC]
    public void UpdateHeaderRemote(string val)
    {
        titel.text = val;
    }

    [PunRPC]
    public void UpdateContentRemote(string val)
    {
        text = val;
    }

    [PunRPC]
    public void DeleteNoteRemote()
    {
        Destroy(gameObject);
    }
    #endregion
}
