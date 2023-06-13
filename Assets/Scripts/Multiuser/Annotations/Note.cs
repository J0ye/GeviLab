using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Note : Annotation
{
    public GameObject noteInputPrefab;
    public TMP_Text titelUIObject;
    [Header("Content")]
    public string titel = "";
    [TextArea]
    public string text = "";

    public static Note CreateNote(string noteTitel, string content, Vector3 position, Vector3 lookAt)
    {
        GameObject notePrefab = Resources.Load<GameObject>("NoteObject");
        GameObject newNote = Instantiate(notePrefab, position, Quaternion.identity);
        Note noteComponent = newNote.GetComponent<Note>();
        noteComponent.SetNoteContent(noteTitel, content, position, lookAt);
        noteComponent.OpenContentEdit();

        return noteComponent;
    }

    public static Note CreateNetworkedNote(string noteTitel, string content, Vector3 position, Vector3 lookAt)
    {
        GameObject newNote = PhotonNetwork.Instantiate("NoteObject", position, Quaternion.identity);
        Note noteComponent = newNote.GetComponent<Note>();
        noteComponent.SetNoteContent(noteTitel, content, position, lookAt);
        noteComponent.OpenContentEdit();

        return noteComponent;
    }

    public void SetNoteContent(string noteTitel, string content, Vector3 position, Vector3 lookAt)
    {
        transform.LookAt(2 * position - lookAt);
        titelUIObject.text = noteTitel;
        titel = noteTitel;
        text = content;
    }

    private void Awake()
    {
        if(permanent)
        {
            SetNoteContent(titel, text, transform.position, Vector3.zero);
        }
    }

    public override void Open()
    {
        GameObject newFullscreenNote = Instantiate(fullscreenPrefab, Vector3.zero, Quaternion.identity);
        FullscreenNote fn = newFullscreenNote.GetComponent<FullscreenNote>();
        fn.content.text = titel + "\n" + text;
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

    public void SetHeader(string val)
    {
        titelUIObject.text = val;
        titel = val;
        photonView.RPC("UpdateHeaderRemote", RpcTarget.Others, val);
    }

    public void SetContent(string val)
    {
        text = val;
        photonView.RPC("UpdateContentRemote", RpcTarget.Others, val);
    }

    #region PUNs

    [PunRPC]
    public void UpdateHeaderRemote(string val)
    {
        titelUIObject.text = val;
        titel = val;
    }

    [PunRPC]
    public void UpdateContentRemote(string val)
    {
        text = val;
    }
    #endregion
}
