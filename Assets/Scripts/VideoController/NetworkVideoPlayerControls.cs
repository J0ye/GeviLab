using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class NetworkVideoPlayerControls : MonoBehaviourPunCallbacks
{
    public float timeToPrepare = 1f;
    public GameObject videoPlayerUI;

    private VideoPlayer vp;
    private new AudioSource audio;

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
    }

    public override void OnConnectedToMaster()
    {
        vp.Prepare();
        vp.Play();
        audio = vp.GetComponent<AudioSource>();
        audio.mute = true;
        Invoke("Pause", timeToPrepare);
        Invoke("SwitchMuteAudio", timeToPrepare);
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            videoPlayerUI.SetActive(false);
            photonView.RPC("GetPlayerState", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer);
        }
    }

    [PunRPC]
    public void GetPlayerState(Player reciever)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetPlayerState", reciever, vp.frame, vp.time, vp.isPlaying);
        }
    }

    [PunRPC]
    public void SetPlayerState(long frame, double playBackTime, bool playing)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            vp.frame = frame;
            vp.time = playBackTime;
            if (!playing) vp.Pause();
            else vp.Play();
        }
    }

    #region Player Controls
    [PunRPC]
    public void Pause()
    {
        if(PhotonNetwork.IsMasterClient) this.BroadcastRPC("Pause");

        if (vp.isPlaying)
        {
            print("paused");
            vp.Pause();
        }
        else
        {
            vp.Play();
        }
    }

    [PunRPC]
    public void JumpForward()
    {
        if (PhotonNetwork.IsMasterClient) this.BroadcastRPC("JumpForward");

        float t = (float)vp.time;
        t += 10f;
        t = Mathf.Clamp(t, 0f, (float)vp.clip.length);
        vp.time = t;
    }

    [PunRPC]
    public void JumpBackward()
    {
        if (PhotonNetwork.IsMasterClient) this.BroadcastRPC("JumpBackward");

        float t = (float)vp.time;
        t -= 10f;
        t = Mathf.Clamp(t, 0f, (float)vp.clip.length);
        vp.time = t;
    }


    [PunRPC]
    public void SwitchMuteAudio()
    {
        if (PhotonNetwork.IsMasterClient) this.BroadcastRPC("SwitchAudioMute");
        audio.mute = !audio.mute;
    }
    #endregion
}


/// <summary>
/// Add new functions to each class that inherits MonoBehaviourPunCallbacks.
/// </summary>
static class NewMonoBehaviourPunCallbacksMethods
{

    /// <summary>
    /// Part of the extension functions provided by NewMonoBehaviourPunCallbacksMethods.
    /// Sends an RPC of a certain procedure to each OTHER player in this room.
    /// </summary>
    /// <param name="mbpc">Instance of the class calling this function. Should be used as this.BroadcastRPC()</param>
    /// <param name="procedureName">Name of the PunRPC as string</param>
    public static void BroadcastRPC(this MonoBehaviourPunCallbacks mbpc, string procedureName)
    {
        foreach (Player pl in PhotonNetwork.PlayerListOthers)
        {
            mbpc.photonView.RPC(procedureName, pl);
        }
    }

    /// <summary>
    /// Overloaded version of the broadcast to add parameter to the procedure call.
    /// </summary>
    /// <param name="mbpc"></param>
    /// <param name="procedureName"></param>
    /// <param name="parameter">A list of parameters. Should be divided by ','.</param>
    public static void BroadcastRPC(this MonoBehaviourPunCallbacks mbpc, string procedureName, params object[] parameter)
    {        
        foreach (Player pl in PhotonNetwork.PlayerListOthers)
        {
            if(mbpc.photonView != null)
            {
                mbpc.photonView.RPC(procedureName, pl, parameter);
            }
            else
            {
                Debug.LogWarning("No view?");
            }
        }
    }
}
