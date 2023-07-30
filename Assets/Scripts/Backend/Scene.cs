using System;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public Guid id = Guid.NewGuid();

    // public Guid id {get; private set; } = Guid.NewGuid();
    public string name = "SceneName";

    // public string name { get; private set; } = "SceneName";
    public string description = "Scene Description";

    // public string description { get; private set; } = "Scene Description";
    public Vector3 position = Vector3.zero;
    public Vector3 rotation = Vector3.zero;
    public Vector3 scale = Vector3.one;
    public string imagePath;
    private GameObject sphere;
    private Texture2D image;
    private bool isVisible;

    public Scene()
    {
        // id = Guid.NewGuid();
    }

    // public Scene(string name, string description, Vector3 position, Vector3 rotation, Vector3 scale)
    // {
    //     this.id = Guid.NewGuid();
    //     this.name = name;
    //     this.description = description;
    //     this.position = position;
    //     this.rotation = rotation;
    //     this.scale = scale;
    // }

    public Texture2D GetImage()
    {
        return image;
    }

    public void SetImage(Texture2D value)
    {
        image = value;
    }

    // public string GetImagePath()
    // {
    //     return imagePath;
    // }

    // public void SetImagePath(string value)
    // {
    //     imagePath = value;
    // }

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

    public async void Initialize(GameObject spherePrefab)
    {
        // sphere = new GameObject().transform;
        // Instantiate the prefab at the origin (0, 0, 0) with no rotation
        GameObject sphere = Instantiate(spherePrefab, position, Quaternion.Euler(rotation));
        sphere.name = name;
        // sphere.position = position;
        // sphere.rotation = Quaternion.Euler(rotation);
        sphere.transform.localScale = scale;
        // TODO Get the image from the path (from local Cache or Backend)
        Texture2D image = await GeViLab.Backend.FileCache.Instance.GetTextureFile(imagePath);
        // Apply texture to Material of the sphere
        sphere.GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    public void Sync()
    {
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
        scale = transform.localScale;
    }
}
