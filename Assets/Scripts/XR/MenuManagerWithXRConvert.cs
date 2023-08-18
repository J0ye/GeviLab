using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

[RequireComponent(typeof(BoxCollider))]
public class MenuManagerWithXRConvert : MenuManager
{
    [Header("XR Menu Settings")]
    public Transform targetAnchor;
    public Vector3 xrScale = Vector3.zero;

    public BooleanAction openMenuTrigger = new BooleanAction();
    public bool openMenuOnConvert = true;

    protected BoxCollider colliderComponent;
    protected bool openMenuTriggered = false;
    private void Awake()
    {
        colliderComponent = GetComponent<BoxCollider>();
    }

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
        colliderComponent.enabled = false;
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
        colliderComponent.enabled = true;
        // fit collider to canvas size
        colliderComponent.size = rect.rect.size;
        colliderComponent.center = rect.rect.center;

        rect.anchoredPosition3D = Vector3.zero;
        LookAtPlayer lap = gameObject.AddComponent<LookAtPlayer>();
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
