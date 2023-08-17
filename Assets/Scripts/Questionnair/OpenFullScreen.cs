using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OpenFullScreen : MonoBehaviour, IPointerDownHandler
{
    public GameObject fullscreenPrefab;
    public bool setNativeSize = true;
    public UnityEvent OnPress = new UnityEvent();


    protected Image img;
    public void Start()
    {
        img = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress.Invoke();
    }

    public void Open()
    {
        Transform canvas = transform.parent.parent.parent.parent;
        GameObject obj = Instantiate(fullscreenPrefab, canvas);
        FullScreenImage fullscreen = obj.GetComponent<FullScreenImage>();
        fullscreen.LoadImage(img.sprite);
        if(setNativeSize)
        {
            fullscreen.FitCenterImageToNative();
        }
        else
        {
            fullscreen.FitCenterImageToScreen();
        }
    }
}
