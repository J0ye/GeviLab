using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteInput : MonoBehaviour
{
    public TMP_InputField headerInput;
    public TMP_InputField textInput;

    private Note origin;
    // Start is called before the first frame update
    void Start()
    {
        headerInput.onValueChanged.AddListener(UpdateHeader);
        textInput.onValueChanged.AddListener(UpdateContent);
    }

    public void SetOrigin(Note val)
    {
        origin = val;
    }

    public void CloseInput()
    {
        origin.isEditing = false;
        GameState.instance.SetActivePlayerControls(true);
        Destroy(gameObject);
    }

    private void UpdateHeader(string val)
    {
        origin.SetHeader(val);
    }

    private void UpdateContent(string val)
    {
        origin.SetContent(val);
    }
}
