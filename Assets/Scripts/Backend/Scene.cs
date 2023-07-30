using System;
using UnityEngine;

public class Scene
{
    public Guid id {get; set; } = Guid.NewGuid();
    public string name { get; set; } = "SceneName";
    public string description { get; set; } = "Scene Description";
    private Transform transform;
    private Texture2D image;
    private string imagePath;
    private bool isVisible;

    public Scene()
    {
        id = Guid.NewGuid();
        transform = new GameObject().transform;
    }

    // public Guid GetId()
    // {
    //     return id;
    // }

    // public string GetName()
    // {
    //     return name;
    // }

    // public void SetName(string value)
    // {
    //     name = value;
    // }

    // public string GetDescription()
    // {
    //     return description;
    // }

    // public void SetDescription(string value)
    // {
    //     description = value;
    // }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 value)
    {
        transform.position = value;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public void SetRotation(Quaternion value)
    {
        transform.rotation = value;
    }

    public Vector3 GetScale()
    {
        return transform.localScale;
    }

    public void SetScale(Vector3 value)
    {
        transform.localScale = value;
    }

    public Texture2D GetImage()
    {
        return image;
    }

    public void SetImage(Texture2D value)
    {
        image = value;
    }

    public string GetImagePath()
    {
        return imagePath;
    }

    public void SetImagePath(string value)
    {
        imagePath = value;
    }

    public bool GetIsVisible()
    {
        return isVisible;
    }

    public void SetIsVisible(bool value)
    {
        isVisible = value;
    }

    public void ToggleVisibility()
    {
        isVisible = !isVisible;
    }
}
