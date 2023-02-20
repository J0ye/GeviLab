using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public VideoElementControls vec;
    public GameObject cameraRig;
    public bool forceVRState = false;

    public static GameState instance;

    [Space]
    [Tooltip("This member is not used as a parameter, but rather to display speical informations about the State")]
    public string logOutput = "";

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

        if(Application.platform == RuntimePlatform.Android || forceVRState)
        {
            SetStateToVR();
            Destroy(vec.playerCamera);
            print("is anroid");
        }
        else
        {
            SetStateToDesktop();
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
        WriteToLogOutput("Programm is displayed in VR: " + isVR);
    }

    private void WriteToLogOutput(string txt)
    {
        logOutput = txt;
    }
}