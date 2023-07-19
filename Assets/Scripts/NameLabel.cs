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
        SetLabelReference();
    }

    public void SyncName()
    {
        if (photonView.IsMine)
        {
            LogCreator.instance.AddLog("Name: " + PhotonNetwork.NickName);
            // The local player can set their name directly.
            SetLabelValue(PhotonNetwork.NickName);
            // Synchronize the player name across the network.
            photonView.RPC(nameof(SyncPlayerName), RpcTarget.OthersBuffered, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    public void SyncPlayerName(string playerName)
    {
        SetLabelValue(playerName);
    }



    public void SetLabelValue(string newValue)
    {
        if(label)
        {
            label.text = newValue;
        }
    }

    protected void SetLabelReference()
    {
        foreach (Transform child in transform)
        {
            child.TryGetComponent<TMP_Text>(out label);
        }
    }
}
