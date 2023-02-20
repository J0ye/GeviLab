using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Note : MonoBehaviour
{
    public static void CreateNote(string titel, Vector3 position, Vector3 lookAt)
    {
        GameObject notePrefab = Resources.Load<GameObject>("NoteObject");
        GameObject newNote = Instantiate(notePrefab, position, Quaternion.identity);
        newNote.transform.LookAt(2* position - lookAt);
        newNote.GetComponent<Note>().titel.text = titel;
    }

    public TMP_Text titel;

    public void OnMouseDown()
    {
        OpenNote();
    }

    public void OpenNote()
    {
        print("open note");
    }
}
