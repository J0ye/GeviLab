using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SwitchImage : MonoBehaviour
{
    public Sprite switchImage;

    protected Image image;

    void Start()
    {
        SetImage();
    }

    public void SwitchDisplayState()
    {
        image.enabled = !image.enabled;
    }

    public void SetDisplayState(bool val)
    {
        image.enabled = val;
    }

    public void Switch()
    {
        Sprite temp = image.sprite;
        image.sprite = switchImage;
        switchImage = temp;
    }

    protected void SetImage()
    {
        image = GetComponent<Image>();
    }
}
