using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string valName = "";

    public void SetVolume(float value)
    {
        audioMixer.SetFloat(valName, Mathf.Log10(value) * 20);
    }
}
