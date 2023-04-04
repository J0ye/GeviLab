using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractNoteObject
{
    public Note note;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public AbstractNoteObject(GameObject target, out bool hasNote)
    {
        Note targetNote;
        hasNote = target.TryGetComponent<Note>(out targetNote);
        if (hasNote)
        {
            note = targetNote;
            position = target.transform.position;
            rotation = target.transform.rotation;
            scale = target.transform.localScale;
        }
    }
}


public class Session : MonoBehaviour
{
    public static Session instance { get; private set; }

    private List<AbstractNoteObject> notes = new List<AbstractNoteObject>();

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddNoteToSession(GameObject newNote)
    {
        bool able;
        AbstractNoteObject abstractNote = new AbstractNoteObject(newNote, out able);
        if(able)
        {
            notes.Add(abstractNote);
        }
        return able;
    }
}
