using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class FullscreenAnnotation : MonoBehaviour
{
    [HideInInspector]
    public Annotation origin;
    /// <summary>
    /// Used for a delay, so the input for opening the note does not close it again.
    /// </summary>
    protected bool delayed = true;

    protected void Start()
    {
        StartCoroutine(OpenProtocol());
        if (origin != null) origin.SetDisplay(false);
    }

    public void Update()
    {
        if (Input.anyKey && !delayed)
        {
            CloseFullscreen();
        }
    }

    public void CloseFullscreen()
    {
        if (origin != null) origin.SetDisplay(true);
        GameState.instance.SetActivePlayerControls(true);
        Destroy(gameObject);
    }

    public void DeleteAnnotation()
    {
        if (origin != null) origin.DeleteAnnotation();
        CloseFullscreen();
    }

    public void SetDelay(bool val)
    {
        delayed = val;
    }

    /// <summary>
    /// Delays any interaction with the pop up until the left mosue button is released.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator OpenProtocol()
    {
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));
        delayed = false;
    }
}

