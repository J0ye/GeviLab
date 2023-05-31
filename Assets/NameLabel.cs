using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class NameLabel : MonoBehaviourPun
{
    private TMP_Text label;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Transform child in transform)
        {
            child.TryGetComponent<TMP_Text>(out label);
        }
    }

    public void SyncName()
    {
        if (photonView.IsMine)
        {
            // The local player can set their name directly.
            label.text = PhotonNetwork.NickName;
            // Synchronize the player name across the network.
            print("New name");
            photonView.RPC("SyncPlayerName", RpcTarget.OthersBuffered, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    public void SyncPlayerName(string playerName)
    {
        label.text = playerName;
    }



    public void SetLabel(string newValue)
    {
        if(label)
        {
            label.text = newValue;
        }
        else
        {
            Debug.LogError("No label on avatar for " + newValue);
        }
    }
}
