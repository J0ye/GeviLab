using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FullscreenNote : MonoBehaviour
{
    public TMP_Text content;

    [HideInInspector]
    public Note origin;
    /// <summary>
    /// Used for a delay, so the input for opening the note does not close it again.
    /// </summary>
    private bool delayed = true;

    private void Start()
    {
        StartCoroutine(OpenProtocol());
        if (origin != null) origin.SetDisplay(false);
    }

    public void Update()
    {
        if(Input.anyKey && !delayed)
        {
            CloseFullscreenNote();
        }
    }

    public void CloseFullscreenNote()
    {
        if (origin != null) origin.SetDisplay(true);
        GameState.instance.SetActivePlayerControls(true);
        Destroy(gameObject);
    }

    public void DeleteNote()
    {
        if (origin != null) origin.DeleteNote();
        CloseFullscreenNote();
    }

    public void SetDelay(bool val)
    {
        delayed = val;
    }

    private IEnumerator OpenProtocol()
    {
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));
        delayed = false;
    }
}
