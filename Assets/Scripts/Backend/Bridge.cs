// BEGIN: f8e9d6a6gjw3
using System;

public class Bridge
{
    private Guid id;
    private Scene startScene;
    private Scene targetScene;

    public Bridge(Scene startScene, Scene targetScene)
    {
        id = Guid.NewGuid();
        this.startScene = startScene;
        this.targetScene = targetScene;
    }

    public Guid GetId()
    {
        return id;
    }

    public Scene GetStartScene()
    {
        return startScene;
    }

    public Scene GetTargetScene()
    {
        return targetScene;
    }
}

