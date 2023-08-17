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
    void Awake()
    {
        LoadUserData();
    }

    public void CreateNewUser(string userName, string userPassword)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string filename = userName + "LoginData";
        filename = Session.MakeValidFileName(filename);
        filename = "/" + filename + ".dat";
        FileStream file = File.Create(Application.persistentDataPath + filename);
        User newUser = new User(userName, userPassword);
        bf.Serialize(file, newUser);
        file.Close();
        Debug.Log("File " + filename + "saved to " + Application.persistentDataPath);
        OnRegister.Invoke();
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
        CreateNewUser(newNameInput.text, newPasswordInput.text);
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

    public bool Login(string loginName, string loginPassword)
    {
        // Temporary user build from input
        User temp = new User(loginName, loginPassword);

        foreach (User u in userData)
        {
            if (u.Compare(temp))
            {
                // There exists a user for this name and password. U represents the user data coresponding to user input
                Session.SetUser(u);
                OnLoginSuccsessfull.Invoke();
                WritePopUpMessage("Logged in to " + u.name);
                WritePopUpMessage("Data: " + Session.Instance().ToJSON());
                return true;
            }
        }

        Debug.Log("No login");
        OnLoginUnsuccsessfull.Invoke();
        WritePopUpMessage("No Login found for " + loginName);
        return false;
    }

    public void Login()
    {
        if(CheckInputField(nameInput, passwordInput))
        {
            Debug.Log("login input has wrong characters");
            return;
        }
        Login(nameInput.text, passwordInput.text);
    }

    public void Logout()
    {
        WritePopUpMessage("Loggin out from " + Session.Instance().user);
        WritePopUpMessage("Saving " + Session.Instance().ToJSON());
        Session.Instance().OnLogin += MessageEvent;
        SaveSessionData();
        if(Session.Instance().user == null)
        {
            Debug.LogWarning("No user is logged in. Wont run logout.");
            return;
        }

        Session.Instance().Clear();
        OnLogout.Invoke();
    }

    public void SaveSessionData()
    {
        Session.SaveSessionData();
    }

    protected void MessageEvent(object sender, EventArgs e)
    {
        WritePopUpMessage("Saved");
    }

    public void WritePopUpMessage(string text)
    {
        LogCreator.instance.AddLog(text);
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
