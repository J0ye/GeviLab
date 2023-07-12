using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Controls avatar specific functions that need the network. Avatars are the representation of player positions and interactions on other users clients. 
/// </summary>
public class NetworkAvatarControls : MonoBehaviourPunCallbacks
{
    public static NetworkAvatarControls instance;
    public GameObject playerPrefab;
    [HideInInspector]
    public GameObject myAvatar;
    public int avatarID = -1;
    [Header("Spawn location")]
    public EnvironmentBridge startEnvironment;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// This method is responsible for spawning the player avatar. If the current client is not the master
    /// client, it requests the position from the master client using an RPC call. If it is the master 
    /// client, it calls the InstantiateAvatar method to create the avatar at a random position.
    /// </summary>
    public void SpawnPlayer()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            // Request the position from the master client. Master client will answer with InstantiateAvatar RPC
            photonView.RPC(nameof(GetPosition), PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer);
        }
        else
        {
            // If this is the master client, instantiate the avatar at a random position
            InstantiateAvatar(startEnvironment.GetPositionInEnvironment());
        }
    }

    #region RPCs
    /// <summary>
    /// This RPC method is used by the master client to send the random position to the target player. It 
    /// calls the InstantiateAvatar method on the target player's client.
    /// </summary>
    /// <param name="target"></param>
    [PunRPC]
    public void GetPosition(Player target)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(InstantiateAvatar), target, startEnvironment.GetPositionInEnvironment());
        }
    }

    /// <summary>
    /// This RPC method instantiates the player avatar using PhotonNetwork.Instantiate. It creates the 
    /// avatar GameObject at the specified position and sets it as inactive initially. It also retrieves 
    /// the ViewID of the avatar's PhotonView and stores it in the avatarID variable.
    /// The new local avatar is hidden.
    /// </summary>
    /// <param name="position"></param>
    [PunRPC]
    public void InstantiateAvatar(Vector3 position)
    {
        // Instantiate networked avatar at the specified position
        myAvatar = PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity); 
        // Deactivate the LOCAL avatar instance renderer. Client does not have to see own avatar.
        myAvatar.GetComponent<Renderer>().enabled = false;
        myAvatar.GetComponent<LineRenderer>().enabled = false;
        foreach(Transform child in myAvatar.transform)
        {
            if(child.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.enabled = false;
            }
        }
        myAvatar.GetComponent<NameLabel>().SyncName(); // Sync the avatar's name across the network
        avatarID = myAvatar.GetComponent<PhotonView>().ViewID; // Store the network ID of my avatar reference.
    }

    /// <summary>
    /// Called by EnvironmentBridge to move avatar. If master then move, if client ask master to move.
    /// </summary>
    /// <param name="name">Name of target EnvironmentBridge</param>
    public void MoveAvatarTo(string name)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            // Skip sending rpc. Directly move avatar
            MoveAvatarToEnvironment(EnvironmentBridge.environments[name].GetPositionInEnvironment());
        }
        else
        {
            // Request master to move avatar
            photonView.RPC(nameof(RequestMoveAvatar), RpcTarget.MasterClient, name, PhotonNetwork.LocalPlayer);
            LogCreator.instance.AddLog("Requested move to " + name);
        }
    }

    /// <summary>
    /// Called by clients so the master client can asgin them a free position. Makes RPC for client to move avatar. The master exclusively controls position assignment to prevent duplicate position assignments.
    /// </summary>
    /// <param name="name">Name of environment</param>
    /// <param name="requestSender"></param>
    [PunRPC]
    public void RequestMoveAvatar(string name, Player requestSender)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if(EnvironmentBridge.environments != null)
            {
                if (EnvironmentBridge.environments.ContainsKey(name))
                {
                    // Client knows target environment
                    Vector3 newEnvPos = EnvironmentBridge.environments[name].GetPositionInEnvironment();
                    photonView.RPC(nameof(MoveAvatarToEnvironment), requestSender, newEnvPos);
                }
                else
                {
                    Debug.LogError("No environment with name " + name);
                    LogCreator.instance.AddLog("No environment with name " + name);
                    // environment is missing/wrong
                }
            }
            else
            {
                Debug.LogError("No environments");
                LogCreator.instance.AddLog("No environments");
                // There are no environments at the moment
            }
        }
    }
    /// <summary>
    /// Called only by the master client to move local avatar (avatar of user who asked to be moved) to a free position.
    /// </summary>
    /// <param name="pos">Free position</param>
    [PunRPC]
    public void MoveAvatarToEnvironment(Vector3 pos)
    {
        LogCreator.instance.AddLog("Recieved rpc to move avatar to environment");
        myAvatar.transform.position = pos;
    }
    #endregion
}
