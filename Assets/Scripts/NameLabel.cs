using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class NameLabel : MonoBehaviourPun
{
    public bool writeDebugToConsole = false;
    private TMP_Text label;
    // Start is called before the first frame update
    void Awake()
    {
        print("1");
        SetLabelReference();
    }

    public void SyncName()
    {
        if (photonView.IsMine)
        {
            Print("Send request to sync name");
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
        LogCreator.instance.AddLog(playerName + " connected to session.");
        Print("Set name of " + gameObject.name + " to " + playerName);
    }



    public void SetLabelValue(string newValue)
    {
        if(label)
        {
            label.text = newValue;
            Print("New label is " + newValue);
        }
        else
        {
            Print("Error: No label");
        }
    }

    protected void SetLabelReference()
    {
        foreach (Transform child in transform)
        {
            if(child.TryGetComponent<TMP_Text>(out label))
            {
                Print("Set label to " + label.ToString());
                return;
            }
        }
    }

    protected void Print(string t)
    {
        if(writeDebugToConsole)
        {
            Debug.Log(t);
        }
    }
}
