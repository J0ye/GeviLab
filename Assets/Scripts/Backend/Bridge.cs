using System;
using UnityEngine;
using TMPro;

public class Bridge : MonoBehaviour
{
    private Guid id;
    private string name;
    private BridgeType type;
    private Guid targetLocationId;
    private float startDirection;
    private float targetDirection;
    private string targetDescription;
    private GameObject bridgeGameObject;
    private bool active;

    public Bridge(
        string name,
        BridgeType type,
        Guid targetLocationId,
        float startDirection,
        float targetDirection,
        string targetDescription,
        bool active = true
    )
    {
        this.id = Guid.NewGuid();
        this.name = name;
        this.type = type;
        this.targetLocationId = targetLocationId;
        this.startDirection = startDirection;
        this.targetDirection = targetDirection;
        this.targetDescription = targetDescription;
        this.active = active;
    }

    public Guid GetId()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public BridgeType GetType()
    {
        return type;
    }

    public Guid GetTargetLocationId()
    {
        return targetLocationId;
    }

    public float GetStartDirection()
    {
        return startDirection;
    }

    public float GetTargetDirection()
    {
        return targetDirection;
    }
    public string GetTargetDescription()
    {
        return targetDescription;
    }
public bool GetActive()
    {
        return active;
    }
    public void Initialize(GameObject bridgePrefab, Transform parent, float sphereRadius = 5f)
    {
        PolarCoordinates startLatLon = new PolarCoordinates(0.5f,startDirection,-0.25f);
        Vector3 position = startLatLon.ToCartesianCoordinates(sphereRadius);
        Vector3 direction = position - Vector3.zero;
        Quaternion rotation = Quaternion.LookRotation(direction,Vector3.up);
        bridgeGameObject = Instantiate(bridgePrefab, parent, true);
        bridgeGameObject.transform.localPosition = position;
        bridgeGameObject.transform.localRotation = rotation;
        bridgeGameObject.name = name;
        bridgeGameObject.GetComponentInChildren<TMP_Text>().text = targetDescription;
        bridgeGameObject.SetActive(active);
    }
}

/// <summary>
/// Enum of possible types of bridges.
/// </summary>
public enum BridgeType
{
    Forward,
    Backward,
    Other
}
