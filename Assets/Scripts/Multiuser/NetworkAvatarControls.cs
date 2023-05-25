using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Controls player specific functions that need the network
/// </summary>
public class NetworkAvatarControls : MonoBehaviourPunCallbacks
{
    public static NetworkAvatarControls instance;
    public GameObject playerPrefab;
    public int avatarID = -1;
    private PositionList positions;
    // Start is called before the first frame update
    void Awake()
    {
        Vector2 xTemp = new Vector2(-5, 5);
        Vector2 zTemp = new Vector2(-2, 2);
        positions = new PositionList(1, xTemp, zTemp);

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
            photonView.RPC("GetPosition", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer);
        }
        else
        {
            // If this is the master client, instantiate the avatar at a random position
            InstantiateAvatar(positions.GetRandomPosition());
        }
    }

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
            photonView.RPC("InstantiateAvatar", target, positions.GetRandomPosition());
        }
    }

    /// <summary>
    /// This RPC method instantiates the player avatar using PhotonNetwork.Instantiate. It creates the 
    /// avatar GameObject at the specified position and sets it as inactive initially. It also retrieves 
    /// the ViewID of the avatar's PhotonView and stores it in the avatarID variable.
    /// </summary>
    /// <param name="position"></param>
    [PunRPC]
    public void InstantiateAvatar(Vector3 position)
    {
        GameObject newAvatar = PhotonNetwork.Instantiate(playerPrefab.name, position - Vector3.up, Quaternion.identity);
        newAvatar.SetActive(false);
        avatarID = newAvatar.GetComponent<PhotonView>().ViewID;
    }
}
