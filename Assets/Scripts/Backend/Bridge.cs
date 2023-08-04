using System;
using UnityEngine;
public class Bridge
{
    private Guid id;
    private string name;
    private Guid startSceneId;
    private Guid targetSceneId;
    private Vector3 forwardDirection;
    private Vector3 backwardDirection;

    public Bridge(
        string name,
        Guid startSceneId,
        Guid targetSceneId,
        Vector3 forwardDirection,
        Vector3 backwardDirection
    )
    {
        id = Guid.NewGuid();
        this.name = name;
        this.startSceneId = startSceneId;
        this.targetSceneId = targetSceneId;
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

    public Guid GetStartSceneId()
    {
        return startSceneId;
    }

    public Guid GetTargetSceneId()
    {
        return targetSceneId;
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
