using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{
    public VideoElementControls vec;
    public VideoElementControls xRVec;
    public GameObject cameraRig;
    public bool forceVRState = false;

    public static GameState instance;

    [Space]
    [Tooltip("This member is not used as a parameter, but rather to display speical informations about the State")]
    public string logOutput = "";

    public Role roleState { get; private set; }
    public bool setStateByPlayer { get; private set; }


    /// <summary>
    /// Event for changing acording to new role. Will be called after new role has been set. I.E. Change player UI because access has changed.
    /// </summary>
    private UnityEvent OnSwitchRole = new UnityEvent();
    public bool isVR { get; private set; }
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
        SetRole(new Educator());

        if (Application.platform == RuntimePlatform.Android || forceVRState)
        {
            SetStateToVR();
            Destroy(vec.playerCamera);
            print("is anroid");
        }
        else
        {
            SetStateToDesktop();
        }
    }

    private void Start()
    {
        ConncectionManager.instance.OnEnterRoom.AddListener(() => SetActivePlayerControls());
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
    public void SetActivePlayerControls()
    {
        xRVec.AblePlayerControls(isVR);
        vec.AblePlayerControls(!isVR);
    }

    public void SetActivePlayerControls(bool val)
    {
        if(isVR)
        {
            xRVec.AblePlayerControls(val);
            vec.AblePlayerControls(false);
        }
        else
        {
            vec.AblePlayerControls(val);
            xRVec.AblePlayerControls(false);
        }
    }

    public void AddListenerToOnSwitchRole(UnityAction call)
    {
        OnSwitchRole.AddListener(call);
    }

    private void UpdateDisplay()
    {
        vec.playerCamera.SetActive(!isVR);
        cameraRig.SetActive(isVR);
        SetActivePlayerControls(true);
        WriteToLogOutput("Programm is displayed in VR: " + isVR);
    }

    private void WriteToLogOutput(string txt)
    {
        logOutput = txt;
    }

    #region Role logic
    /// <summary>
    /// The rpc for this function is in the connection manager script
    /// </summary>
    /// <param name="newRole"></param>
    public void SetRole(Role newRole)
    {
        if(roleState != null) roleState.RemoveRole();
        roleState = newRole;
        roleState.SetRole();
        OnSwitchRole.Invoke();
        SetActivePlayerControls(true);
        WriteToLogOutput("Role: " + roleState.ToString());
    }
    #endregion
}