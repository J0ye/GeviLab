using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        LogCreator.instance.AddLog("Player entered");
        LogCreator.instance.AddLog(newPlayer.ToString());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        LogCreator.instance.AddLog(otherPlayer.NickName + " left");
    }
}
