using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

public class MenuManagerWithXRConvert : MenuManager
{

    [Header("XR Menu Settings")]
    public Transform targetAnchor;
    public Vector3 xrScale = Vector3.zero;

    public BooleanAction openMenuTrigger = new BooleanAction();
    public bool openMenuOnConvert = true;

    protected bool openMenuTriggered = false;

    private void Update()
    {
        ExecuteOnVRInput();
    }

    protected void ExecuteOnVRInput()
    {
        if(openMenuTrigger.IsActivated && !openMenuTriggered)
        {
            ToggleUIConvert();
            if (openMenuOnConvert) SwitchMenuState(MenuState.Setting);

            openMenuTriggered = true;
        }
        else if(!openMenuTrigger.IsActivated && openMenuTriggered)
        {
            openMenuTriggered = false;
        }
    }

    public void ConvertToDesktopUI()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        transform.parent = null;
        transform.localScale = startScale;
        if (TryGetComponent<LookAtPlayer>(out LookAtPlayer temp))
        {
            Destroy(temp);
        }
    }

    public void ConvertToXRUI()
    {
        canvas.renderMode = RenderMode.WorldSpace;
        transform.parent = targetAnchor;
        transform.localScale = xrScale;
        rect.anchoredPosition3D = Vector3.zero;
        LookAtPlayer lap = gameObject.AddComponent<LookAtPlayer>();
        lap.lookFor = "MainCamera";
        lap.lookAway = true;
    }

    public void ToggleUIConvert()
    {
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            ConvertToDesktopUI();
        }
        else
        {
            ConvertToXRUI();
        }
    }
}
