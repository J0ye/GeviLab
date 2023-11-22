using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SelectionOption : ImageColorSwitch, IPointerDownHandler
{
    public bool keepColor = false;
    public bool selected = false;

    public UnityEvent OnSelect = new UnityEvent();

    protected Text option;

    public void OnPointerDown(PointerEventData eventData)
    {
        print("pointer happend");
        ExecuteOnSelection();
    }

    public void SetText(string value)
    {
        if (option == null) RegisterTextComponent();

        option.text = value;
    }

    public void ExecuteOnSelection()
    {
        if (!keepColor) SwitchColor();
        selected = !selected;
        OnSelect.Invoke();
    }

    protected void RegisterTextComponent()
    {
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent<Text>(out option))
            {
                return;
            }
        }

        Debug.LogError("No text component was found for " + gameObject.name + "Removing script.");
        Destroy(this);
    }
}