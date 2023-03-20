using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Events;

public class LoginManager : MonoBehaviour
{
    [Header("Login")]
    public InputField nameInput;
    public InputField passwordInput;

    [Header("Register")]
    public InputField newNameInput;
    public InputField newPasswordInput;
    public InputField repeatPassword;

    public UnityEvent OnLoginSuccsessfull = new UnityEvent();
    public UnityEvent OnLoginUnsuccsessfull = new UnityEvent();
    public UnityEvent OnRegister = new UnityEvent();
    public UnityEvent OnLogout = new UnityEvent();

    protected List<User> userData;
    // Start is called before the first frame update
    void Start()
    {
        LoadUserData();
    }

    public void CreateNewUser()
    {
        if(CheckInputField(newNameInput, newPasswordInput) ||
            newPasswordInput.text != repeatPassword.text)
        {
            Debug.Log("name: " + newNameInput.text + " , passwort: " + newPasswordInput.text + " | " + repeatPassword.text);
            Debug.Log("New user credentials are not correct.");
            return;
        }
        BinaryFormatter bf = new BinaryFormatter();
        string filename = newNameInput.text + "LoginData";
        filename = Session_Thermo.MakeValidFileName(filename);
        filename = "/" + filename + ".dat";
        FileStream file = File.Create(Application.persistentDataPath + filename);
        User newUser = new User(newNameInput.text, newPasswordInput.text);
        bf.Serialize(file, newUser);
        file.Close();
        Debug.Log("File " + filename + "saved to " + Application.persistentDataPath);
        OnRegister.Invoke();
        LoadUserData();
    }

    public void LoadUserData()
    {
        userData = new List<User>();
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
        BinaryFormatter bf = new BinaryFormatter();
        foreach (var file in directoryInfo.GetFiles("*.dat"))
        {
            FileStream openFile = File.Open(file.FullName, FileMode.Open);
            User newUserData = (User)bf.Deserialize(openFile);
            openFile.Close();
            userData.Add(newUserData);
            Debug.Log("File loaded from " + file.FullName);
            WritePopUpMessage("Loaded " + newUserData.name + " from " + file.FullName);
        }
    }

    public void Login()
    {
        if(CheckInputField(nameInput, passwordInput))
        {
            Debug.Log("login input has wrong characters");
            return;
        }

        // Temporary user build from input
        User temp = new User(nameInput.text, passwordInput.text);

        foreach(User u in userData)
        {
            if(u.Compare(temp))
            {
                // There exists a user for this name and password. U represents the user data coresponding to user input
                Session_Thermo.SetUser(u);
                OnLoginSuccsessfull.Invoke();
                WritePopUpMessage("Logged in to " + u.name, Color.green);
                WritePopUpMessage("Data: " + Session_Thermo.Instance().ToJSON(), Color.green);
                return;
            }
        }

        Debug.Log("No login");
        OnLoginUnsuccsessfull.Invoke();
        WritePopUpMessage("No Login found for " + nameInput.text, Color.red);
    }

    public void Logout()
    {
        WritePopUpMessage("Loggin out from " + Session_Thermo.Instance().user, Color.green);
        WritePopUpMessage("Saving " + Session_Thermo.Instance().ToJSON());
        Session_Thermo.Instance().OnLogin += MessageEvent;
        SaveSessionData();
        if(Session_Thermo.Instance().user == null)
        {
            Debug.LogWarning("No user is logged in. Wont run logout.");
            return;
        }

        Session_Thermo.Instance().Clear();
        OnLogout.Invoke();
    }

    public void SaveSessionData()
    {
        Session_Thermo.SaveSessionData();
    }

    protected void MessageEvent(object sender, EventArgs e)
    {
        WritePopUpMessage("Saved", Color.red);
    }

    public void WritePopUpMessage(string text)
    {
        PopUpManager pm;
        if (TryGetComponent<PopUpManager>(out pm))
        {
            pm.WriteMessage(text);
        }
        else
        {
            Debug.LogWarning("Could not write message. There is no PopUpManager on " + gameObject.name);
        }
    }

    public void WritePopUpMessage(string text, Color color)
    {
        PopUpManager pm;
        if(TryGetComponent<PopUpManager>(out pm))
        {
            pm.WriteMessage(text, color);
        }
        else
        {
            Debug.LogWarning("Could not write message. There is no PopUpManager on " + gameObject.name);
        }
    }

    private bool CheckInputField(InputField target)
    {
        if(String.IsNullOrWhiteSpace(target.text))
        {
            return true;
        }

        return false;
    }

    private bool CheckInputField(InputField target1, InputField target2)
    {
        if (String.IsNullOrWhiteSpace(target1.text) ||
            String.IsNullOrWhiteSpace(target2.text))
        {
            return true;
        }

        return false;
    }
}
