using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

public class XRUIInteractable : MonoBehaviour
{
    public float highlightDuration = 1f;
    public float highlightStrength = 1f;
    public UnityEvent OnStartHover = new UnityEvent();
    public UnityEvent OnEndHover = new UnityEvent();
    public UnityEvent OnInteract = new UnityEvent();

    protected bool hovering = false;

    public void StartHover()
    {
        if(!hovering)
        {
            OnStartHover.Invoke();
            hovering = true;
        }
    }

    public void EndHover()
    {
        if(hovering)
        {
            OnEndHover.Invoke();
            hovering = false;
        }
    }

    public void Interact()
    {
        OnInteract.Invoke();
        if(highlightDuration > 0)
        {
            transform.DOPunchScale(new Vector3(highlightStrength, highlightStrength, highlightStrength), highlightDuration);
        }
    }
}
