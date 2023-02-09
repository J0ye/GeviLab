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

    public void SpawnPlayer()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("GetPosition", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer);
        }
        else
        {
            InstantiateAvatar(positions.GetRandomPosition());
        }
    }

    [PunRPC]
    public void GetPosition(Player target)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("InstantiateAvatar", target, positions.GetRandomPosition());
        }
    }

    [PunRPC]
    public void InstantiateAvatar(Vector3 position)
    {
        GameObject newAvatar = PhotonNetwork.Instantiate(playerPrefab.name, position - Vector3.up, Quaternion.identity);
        newAvatar.SetActive(false);
        avatarID = newAvatar.GetComponent<PhotonView>().ViewID;
    }
}
