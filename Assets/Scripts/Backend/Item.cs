using System;
using UnityEngine;
using TMPro;
public class Item : MonoBehaviour
{
    /// <summary>
    /// Gets the unique identifier of the item.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the name of the item.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The type of the item.
    /// </summary>
    public ItemType Type { get; }

    /// <summary>
    /// The position of the item in polar coordinates.
    /// </summary>
    public PolarCoordinates LatLon { get; }
    
    public bool Active { get; set; }
    public bool IsSelected { get; set; }
    public float sphereRadius { get; set; } = 5f;

    public Item(Guid id, string name, ItemType type, PolarCoordinates latLon)
    {
        Id = id;
        Name = name;
        Type = type;
        LatLon = latLon;
    }
    public async void Initialize(GameObject itemPrefab, Transform parent)
    {
        Vector3 position = LatLon.ToCartesianCoordinates(sphereRadius); 
        Vector3 direction = position - Vector3.zero;
        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject item = Instantiate(itemPrefab,parent, true);
        item.transform.localPosition = position;
        item.transform.localRotation = rotation;
        item.name = Name;   
        item.GetComponentInChildren<TMP_Text>().text = Name;
        item.SetActive(Active);
    }
}

/// <summary>
/// Enum of possible types of items.
/// </summary>
public enum ItemType
{
    Note,
    Other
}

