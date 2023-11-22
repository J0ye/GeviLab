using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;

[RequireComponent(typeof(Recorder))]
public class VoiceControls : MonoBehaviour
{

    protected Recorder recorder;
    // Start is called before the first frame update
    void Start()
    {
        recorder = GetComponent<Recorder>();
    }

    public void SwitchMicrophoneEnabled()
    {
        SetTransmissionState(!recorder.TransmitEnabled);
    }

    public void EnableMicrophone()
    {
        SetTransmissionState(true);
    }
    
    public void DisableMicrophone()
    {
        SetTransmissionState(false);
    }

    public void SetTransmissionState(bool state)
    {
        recorder.TransmitEnabled = state;
    }
}
