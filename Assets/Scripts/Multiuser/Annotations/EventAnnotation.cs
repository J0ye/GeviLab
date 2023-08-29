using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAnnotation : Annotation
{
    public UnityEvent OnOpen = new UnityEvent();
    public UnityEvent OnOpenXR = new UnityEvent();
    public override void Open()
    {
        OnOpen.Invoke();
    }

    public override void OpenXR()
    {
        OnOpenXR.Invoke();
    }
}
