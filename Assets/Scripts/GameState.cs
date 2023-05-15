using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{
    [Tooltip("This is a reference to the main camera or player camera. The object with the camera component: 'playerCamera'")]
    public GameObject playerCamera;
    [Tooltip("This is a reference to the elements of the UI that are used to control video playback, i.e. Play, Pause, Forward, Backwards. Name: 'videoPlayerUI'")]
    public GameObject videoPlayerUIDesktop;
    public GameObject videoPlayerUIXR;
    public GameObject cameraRig;
    public MenuManager menu;
    public bool forceVRState = false;

    public static GameState instance;

    [Space]
    [Tooltip("This member is not used as a parameter, but rather to display speical informations about the State")]
    public string logOutput = "";

    public Role roleState { get; private set; }
    public bool setStateByPlayer { get; private set; }
    public bool isVR { get; private set; }

    /// <summary>
    /// Event for changing acording to new role. Will be called after new role has been set. I.E. Change player UI because access has changed.
    /// </summary>
    private UnityEvent OnSwitchRole = new UnityEvent();

    private POVCamera pov;
    private NetworkPointerControls pointerControls;
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

        playerCamera.TryGetComponent<POVCamera>(out pov);
        playerCamera.TryGetComponent<NetworkPointerControls>(out pointerControls);
        if (Application.platform == RuntimePlatform.Android || forceVRState)
        {
            SetStateToVR();
            print("is android");
        }
        else
        {
            SetStateToDesktop();
        }
        // Deactivate controls because no connection yet
        videoPlayerUIDesktop.SetActive(false);
        videoPlayerUIXR.SetActive(false);
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
        videoPlayerUIDesktop.SetActive(!isVR);
        videoPlayerUIXR.SetActive(isVR);
    }

    public void SetActivePlayerControls(bool val)
    {
        bool withAuthority = val && roleState.playerAuthority; // Only activate player ui if user has authority
        if(isVR)
        {
            videoPlayerUIXR.SetActive(withAuthority); // enable Player controls in VR 
            videoPlayerUIDesktop.SetActive(false);
        }
        else
        {
            videoPlayerUIDesktop.SetActive(withAuthority); // enable Player controls in Desktop
            // Enable pov camera script and desktop pointer
            if (pov != null) pov.enabled = val;
            if (pointerControls != null) pointerControls.enabled = val;
            videoPlayerUIXR.SetActive(false);
        }
    }

    public void AddListenerToOnSwitchRole(UnityAction call)
    {
        OnSwitchRole.AddListener(call);
    }

    /// <summary>
    /// This function removes every listener from the first 3 buttons in the menu list calld 'buttons' and assignes new function based on their index number.
    /// The first three buttons should be:
    /// 0 = backwards button
    /// 1 = Play/Pause
    /// 2 = Forward
    /// Note: This needs to be changed once video player controls change
    /// </summary>
    /// <param name="target">Target video player that is supposed to be controlled</param>
    public void SwitchButtonFunctionsInMenu(NetworkVideoPlayerControls? target)
    {

        // Iterate through the list
        for(int i = 0; i < 3; i++)
        {
            menu.buttons[i].onClick.RemoveAllListeners();
            if(target != null)
            {
                switch (i)
                {
                    case 0:
                        menu.buttons[i].onClick.AddListener(target.JumpBackward);
                        break;
                    case 1:
                        menu.buttons[i].onClick.AddListener(target.Pause);
                        break;
                    case 2:
                        menu.buttons[i].onClick.AddListener(target.JumpForward);
                        break;
                }
            }
        }
    }

    private void UpdateDisplay()
    {
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