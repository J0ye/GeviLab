using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Note : MonoBehaviour
{

    public GameObject fullscreenPrefab;
    public TMP_Text titel;

    private string text;

    public static Note CreateNote(string noteTitel, string content, Vector3 position, Vector3 lookAt)
    {
        GameObject notePrefab = Resources.Load<GameObject>("NoteObject");
        GameObject newNote = Instantiate(notePrefab, position, Quaternion.identity);
        newNote.transform.LookAt(2 * position - lookAt);
        Note noteComponent = newNote.GetComponent<Note>();
        noteComponent.titel.text = noteTitel;
        noteComponent.text = content;
        print("Content set to: " + noteComponent.text);

        return noteComponent;
    }

    public void OnMouseDown()
    {
        OpenNote();
    }

    public void OpenNote()
    {
        GameObject newFullscreenNote = Instantiate(fullscreenPrefab, Vector3.zero, Quaternion.identity);
        FullscreenNote fn = newFullscreenNote.GetComponent<FullscreenNote>();
        fn.content.text = titel.text + "\n" + text;
        print("Text: " + titel.text + "\n" + text);
        fn.origin = this;
    }

    public void SetDisplay(bool state)
    {
        gameObject.SetActive(state);
    }
}
