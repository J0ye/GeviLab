using System;
using UnityEngine;
public class Bridge
{
    private Guid id;
    private string name;
    private Guid startLocationId;
    private Guid targetLocationId;
    private Vector3 forwardDirection;
    private Vector3 backwardDirection;

    public Bridge(
        string name,
        Guid startLocationId,
        Guid targetLocationId,
        Vector3 forwardDirection,
        Vector3 backwardDirection
    )
    {
        id = Guid.NewGuid();
        this.name = name;
        this.startLocationId = startLocationId;
        this.targetLocationId = targetLocationId;
        this.forwardDirection = forwardDirection;
        this.backwardDirection = backwardDirection;
    }

    public Guid GetId()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public Guid GetStartLocationId()
    {
        return startLocationId;
    }

    public Guid GetTargetLocationId()
    {
        return targetLocationId;
    }

    public Vector3 GetForwardDirection()
    {
        return forwardDirection;
    }

    public Vector3 GetBackwardDirection()
    {
        return backwardDirection;
    }
}
