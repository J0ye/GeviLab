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

    private GameObject itemGameObject;

    public Item(Guid id, string name, ItemType type, PolarCoordinates latLon)
    {
        Id = id;
        Name = name;
        Type = type;
        LatLon = latLon;
    }
    public void Initialize(GameObject itemPrefab, Transform parent, float sphereRadius = 5f)
    {
        Vector3 position = LatLon.ToCartesianCoordinates(sphereRadius); 
        Vector3 direction = position - Vector3.zero;
        Quaternion rotation = Quaternion.LookRotation(direction);
        itemGameObject = Instantiate(itemPrefab,parent, true);
        itemGameObject.transform.localPosition = position;
        itemGameObject.transform.localRotation = rotation;
        itemGameObject.name = Name;   
        itemGameObject.GetComponentInChildren<TMP_Text>().text = Name;
        itemGameObject.SetActive(Active);
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

