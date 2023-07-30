using System.Collections.Generic;

public class Bridges
{
    private static Bridges instance;
    private List<Bridge> bridges;

    private Bridges()
    {
        bridges = new List<Bridge>();
    }

    public static Bridges Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Bridges();
            }
            return instance;
        }
    }

    public void AddBridge(Bridge bridge)
    {
        bridges.Add(bridge);
    }

    public void RemoveBridge(Bridge bridge)
    {
        bridges.Remove(bridge);
    }

    public List<Bridge> GetBridges()
    {
        return bridges;
    }
}