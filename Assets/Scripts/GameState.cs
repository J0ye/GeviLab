using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public VideoElementControls vec;
    public GameObject cameraRig;

    public static GameState instance;

    public bool setStateByPlayer { get; private set; }
    private bool isVR = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        UpdateDisplay();
    }

    public void SetStateToVR()
    {
        isVR = true;
        UpdateDisplay();
        setStateByPlayer = true;
    }

    public void SetStateToDesktop()
    {
        isVR = false;
        UpdateDisplay();
        setStateByPlayer = true;
    }

    public void SwitchState()
    {
        isVR = !isVR;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        vec.playerCamera.SetActive(!isVR);
        cameraRig.SetActive(isVR);
    }
}