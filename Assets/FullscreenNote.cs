using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FullscreenNote : MonoBehaviour
{
    public TMP_Text content;

    [HideInInspector]
    public Note origin;
    private bool delayed = true;

    private void Start()
    {
        Invoke("Ready", 1f);
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
        print("Destroy fullscreen note " + gameObject.name);
        Destroy(gameObject);
    }

    private void Ready()
    {
        delayed = false;
    }
}
