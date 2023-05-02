using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class XRUIInteractable : MonoBehaviour
{
    public float highlightDuration = 1f;
    public float highlightStrength = 1f;
    public UnityEvent OnInteract = new UnityEvent();

    public void Interact()
    {
        OnInteract.Invoke();
        if(highlightDuration > 0)
        {
            transform.DOPunchScale(new Vector3(highlightStrength, highlightStrength, highlightStrength), highlightDuration);
        }
    }
}
