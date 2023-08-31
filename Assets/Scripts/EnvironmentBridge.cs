using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBridge : MonoBehaviour
{
    public EnvironmentBridge destination;
    [Tooltip("Used to change ui buttons to new video player. leave empty if new environment does not have a player")]
    public NetworkVideoPlayerControls destinationNVPC;
    public bool usePlayerUIInOrigin = true;

    [Header("Avatars")]
    public GameObject playerAvatar;
    public GameObject playerXRAvatar;


    [Header("Origin locations")]
    public Vector2 xBounds = Vector2.zero;
    public Vector2 zBounds = Vector2.zero;
    public int margin = 1;
    /// <summary>
    /// List of every envrionmentbridge. If this is null, then there are no envrionements
    /// </summary>
    public static Dictionary<string, EnvironmentBridge> environments;

    /// <summary>
    /// Represents the unique name of this environment. Used to ask Master for position.
    /// designation = gameobject name + world position.
    /// </summary>
    public string designation { get; private set; }

    protected Vector3 originPosition;
    protected NetworkVideoPlayerControls originNVPC;

    protected PositionList positions;

    private void Awake()
    {
        if(environments == null)
        {
            environments = new Dictionary<string, EnvironmentBridge>();
        }

        designation = gameObject.name + transform.position;
        if(!environments.ContainsKey(designation))
        {
            // Only add the designation to the lsit, if this environment does not yet have one.
            // This allows one environment to have multiple bridges, that all are able to target the same origin but different destinations.
            environments.Add(designation, this);
        }
        originPosition = transform.position;
        foreach(Transform child in transform)
        {
            // 'One of those kids should have a NetworkVideoPlayerControls component. Because thats how normal prefabs are.'
            transform.GetChild(0).TryGetComponent<NetworkVideoPlayerControls>(out originNVPC);
        }

        positions = new PositionList(margin, transform.position, xBounds, zBounds);
        positions.RemovePositionsCloseTo(transform.position, 1); // Remove positions too close to user
    }
    
    public void MoveAvatarTo(string name)
    {
        NetworkAvatarControls.instance.MoveAvatarTo(name);
    }    

    /// <summary>
    /// Moves both avatar types to the target destination. This also pauses any active video player
    /// </summary>
    public void MoveToDestination()
    {
        MoveUser(destination.originPosition); // Move to origin position of destination
        MoveAvatarTo(destination.designation);
        if(originNVPC != null) originNVPC.Pause(true);
        GameState.instance.SetActiveVideoPlayerControls(destination.usePlayerUIInOrigin); // Activate player controls if needed
        GameState.instance.SwitchButtonFunctionsInMenu(destinationNVPC);
        GameState.instance.SwitchButtonFunctionsXR(destinationNVPC);
    }

    /// <summary>
    /// Moves both avatar types back to the position of this component. This also pauses any active video player
    /// </summary>
    public void MoveToOrigin()
    {
        MoveUser(originPosition);
        MoveAvatarTo(designation);
        if (destinationNVPC != null) destinationNVPC.Pause(true);
        GameState.instance.SetActiveVideoPlayerControls(usePlayerUIInOrigin); // Activate player controls if needed
        GameState.instance.SwitchButtonFunctionsInMenu(originNVPC);
        GameState.instance.SwitchButtonFunctionsXR(originNVPC);
    }

    /// <summary>
    /// Returns a free, random position in this environment
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPositionInEnvironment()
    {
        return positions.GetRandomPosition();
    }

    protected void MoveUser(Vector3 targetPos)
    {
        playerAvatar.transform.position = targetPos;
        playerXRAvatar.transform.position = targetPos;
    }

    private void OnDestroy()
    {
        environments.Remove(gameObject.name);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (Vector3 pos in positions.GetCopyPositionList())
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(pos, margin / 2f);
            }
        }
    }
}
