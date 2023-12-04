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
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android || forceVRState)
        {
            SetStateToVR();
            LogCreator.instance.AddLog("is android");
        }
        else
        {
            SetStateToDesktop();
        }
        // Deactivate controls because no connection yet
        videoPlayerUIDesktop.SetActive(false);
        videoPlayerUIXR.SetActive(false);
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
    public void SetActiveVideoPlayerControls(bool val)
    {
        bool withAuthority = val && roleState.playerAuthority; // Only activate player ui if user has authority
        if(val) LogCreator.instance.AddLog("Role " + roleState.GetRoleName() +  " has authority: " + roleState.playerAuthority);
        if (isVR)
        {
            videoPlayerUIXR.SetActive(withAuthority); // enable Player controls in VR 
            videoPlayerUIDesktop.SetActive(false);
        }
        else
        {
            videoPlayerUIDesktop.SetActive(withAuthority); // enable Player controls in Desktop
            videoPlayerUIXR.SetActive(false);
        }
    }

    public void SetActivePlayerControls(bool val)
    {
        bool withAuthority = val && roleState.playerAuthority && NetworkAvatarControls.instance.currentEnvironment.usePlayerUIInOrigin; // Only activate player ui if user has authority and the environment needs the player
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

    public void SwitchButtonFunctionsXR(NetworkVideoPlayerControls? target)
    {
        int index = 0;
        foreach(Transform child in videoPlayerUIXR.transform)
        {
            XRUIInteractable temp;
            if(child.TryGetComponent<XRUIInteractable>(out temp))
            {
                temp.OnInteract.RemoveAllListeners();
                switch (index)
                {
                    case 0:
                        temp.OnInteract.AddListener(target.JumpBackward);
                        LogCreator.instance.AddLog("Added backwards for " + target.transform.parent.name);
                        break;
                    case 1:
                        temp.OnInteract.AddListener(target.Pause);
                        LogCreator.instance.AddLog("Added pause for " + target.transform.parent.name);
                        break;
                    case 2:
                        temp.OnInteract.AddListener(target.JumpForward);
                        LogCreator.instance.AddLog("Added forward for " + target.transform.parent.name);
                        break;
                    default:
                        // Do nothing?
                        break;
                }
                index++;
            }
        }
    }

    private void UpdateDisplay()
    {
        playerCamera.SetActive(!isVR);
        cameraRig.SetActive(isVR);
        SetActivePlayerControls(true);
    }

    #region Role logic

    protected void UpdateAccess()
    {
        if (videoPlayerUIDesktop != null)
        {
            // Only update the UI if the user has player authority and isnt in VR
            videoPlayerUIDesktop.SetActive(roleState.playerAuthority && !isVR);
            videoPlayerUIXR.SetActive(roleState.playerAuthority && !isVR);
            // The UI will not be displayed, if the user is in VR or does not have authority (is not master client)
        }
    }
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
        UpdateAccess();
        SetActivePlayerControls(true);
        LogCreator.instance.AddLog("Role: " + roleState.ToString());
    }
    #endregion
}