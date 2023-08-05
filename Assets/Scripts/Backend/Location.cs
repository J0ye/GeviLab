using System;
using UnityEngine;

public class Location : MonoBehaviour
{
    public Guid id = Guid.NewGuid();

    // public Guid id {get; private set; } = Guid.NewGuid();
    public string name = "Default Location";

    // public string name { get; private set; } = "LocationName";
    public string description = "(No Description)";

    // public string description { get; private set; } = "Location Description";
    public Vector3 position = Vector3.zero;
    public Vector3 rotation = Vector3.zero;
    public Vector3 scale = Vector3.one;
    public string imagePath;
    public Item[] items;
    private GameObject location;
    private Texture2D image;
    // private bool isVisible;

    // public Location()
    // {
        // id = Guid.NewGuid();
    // }

    // public Location(string name, string description, Vector3 position, Vector3 rotation, Vector3 scale)
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

    // public bool GetIsVisible()
    // {
    //     return isVisible;
    // }

    // public void SetIsVisible(bool value)
    // {
    //     isVisible = value;
    // }

    // public void ToggleVisibility()
    // {
    //     isVisible = !isVisible;
    // }

    /// <summary>
    /// Initializes the location with the given location prefab and sets its properties.
    /// </summary>
    /// <param name="locationPrefab">The prefab to instantiate.</param>
    public async void Initialize(GameObject locationPrefab, GameObject itemPrefab)
    {
        // location = new GameObject().transform;
        // Instantiate the prefab at the origin (0, 0, 0) with no rotation
        GameObject location = Instantiate(locationPrefab, position, Quaternion.Euler(rotation));
        location.name = name;
        // location.position = position;
        // location.rotation = Quaternion.Euler(rotation);
        location.transform.localScale = scale;
        // Get the image from the path (from local Cache or Backend)
        image = await GeViLab.Backend.FileCache.Instance.GetTextureFile(imagePath);
        // Debug.Log("Image: " + image.width + "x" + image.height);
        // Apply texture to Material of the first MeshRenderer in a child
        location.GetComponentInChildren<MeshRenderer>().material.mainTexture = image;
        foreach (Item item in items)
        {
            item.Initialize(itemPrefab, location.transform);
        }
    }

    public void Sync()
    {
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
        scale = transform.localScale;
    }
}
