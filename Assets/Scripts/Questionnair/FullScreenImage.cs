using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FullScreenImage : MonoBehaviour, IPointerDownHandler
{
    public Image centerimage;
    public UnityEvent OnPress = new UnityEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress.Invoke();
    }

    public void LoadImage(Sprite target)
    {
        centerimage.sprite = target;
    }

    public void FitCenterImageToNative()
    {
        centerimage.SetNativeSize();
    }

    public void FitCenterImageToScreen()
    {
        RectTransform canvas = transform.parent.GetComponent<RectTransform>();
        Vector2 screenDimensions = new Vector2(canvas.sizeDelta.x, canvas.sizeDelta.y);
        centerimage.SetNativeSize();
        RectTransform ciTransform = centerimage.GetComponent<RectTransform>();
        Vector2 newSize;
        if (ciTransform.sizeDelta.x > ciTransform.sizeDelta.y)
        {
            float val = screenDimensions.x / ciTransform.sizeDelta.x;
            newSize = new Vector2(screenDimensions.x, ciTransform.sizeDelta.y * val);
        }
        else
        {
            float val = screenDimensions.y / ciTransform.sizeDelta.y;
            newSize = new Vector2(ciTransform.sizeDelta.x * val, screenDimensions.y);
        }
        ciTransform.sizeDelta = newSize;
    }

    public void Close()
    {
        Debug.Log("Destroyed");
        Destroy(gameObject);
    }
}
