using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageColorSwitch : MonoBehaviour
{
    public Color secondaryColor = Color.magenta;

    protected Image image;
    protected Color primaryColor;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        primaryColor = image.color;
    }

    public void SwitchColor()
    {
        if(image.color == secondaryColor)
        {
            image.color = primaryColor;
        } else
        {
            image.color = secondaryColor;
        }
    }
}
