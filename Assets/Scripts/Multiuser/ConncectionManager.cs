using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using TMPro;
using System.Linq;


/// <summary>
/// Controls connecting and sessions. Does not care for any player specific functions
/// </summary>
public class ConncectionManager : MonoBehaviourPunCallbacks
{
    public UnityEvent OnEnterRoom = new UnityEvent();
    public UnityEvent OnOtherPlayerEnterRoom = new UnityEvent();
    public TextMeshProUGUI statusOutput;
    public List<int> playerIDs = new List<int>();
    public List<string> players = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        WriteStatus("Connecting...");
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            players.Clear();
            Player[] tempPlayerArray = new Player[PhotonNetwork.CurrentRoom.Players.Count];
            playerIDs = PhotonNetwork.CurrentRoom.Players.Keys.ToList<int>();
            foreach (Player p in PhotonNetwork.CurrentRoom.Players.Values)
            {
                players.Add(p.NickName);
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = RandomValues.INSTANCE().GetRandomString();
        print("Nickname: " + PhotonNetwork.NickName);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        StartCoroutine(JoinStandardRoom());
    }

    public override void OnJoinedRoom()
    {
        print("in room");
        OnEnterRoom.Invoke();       
        WritePlayerCount();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ChangeRole", newPlayer, Student.roleName);
        }
        OnOtherPlayerEnterRoom.Invoke();
        WritePlayerCount();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        WritePlayerCount();
    }

    [PunRPC]
    public void ChangeRole(Role newRole)
    {
        GameState.instance.SetRole(newRole);
    }

    [PunRPC]
    public void ChangeRole(string roleName)
    {
        switch (roleName)
        {
            case Student.roleName:
                GameState.instance.SetRole(new Student());
                break;
            case GroupMember.roleName:
                GameState.instance.SetRole(new GroupMember());
                break;
            case Educator.roleName:
                GameState.instance.SetRole(new Educator());
                break;
        }
    }

    private IEnumerator JoinStandardRoom()
    {
        WriteStatus("3");
        for (int i = 3; i > 0; i--)
        {
            WriteStatus(i.ToString());
            yield return new WaitForSeconds(1f);
        }
        if (GameState.instance != null)
        {
            yield return new WaitWhile(() => !GameState.instance.setStateByPlayer);
        }
        print("joining");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
     
    private void WritePlayerCount()
    {
        string temp = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        temp += "; Players: " + PhotonNetwork.CurrentRoom.PlayerCount;
        WriteStatus(temp);
    }

    private void WriteStatus(string text)
    {
        if (statusOutput != null)
        {
            statusOutput.text = text;
        }
    }
}

static class NewPhotonPlayerMethods
{
    public static void CallByName(this Player player)
    {
        Debug.Log("And before came " + player.NickName);
    }
}