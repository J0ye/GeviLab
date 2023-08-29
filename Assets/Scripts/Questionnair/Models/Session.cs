using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Session
{
    public User user;
    public Questionaire questionair;

    public event EventHandler OnLogin;
    public event EventHandler OnLogout;
    public event EventHandler OnSaved;

    protected static Session instance;
    #region Static Methods
    public static Session Instance()
    {
        if(instance == null)
        {
            instance = new Session();
            instance.user = new User();
            instance.questionair = new Questionaire();
        }
        return instance;
    }

    public static void SetUser(User newUser)
    {
        if (newUser.Compare(Instance().user))
        {
            return;
        }
        Instance().user = newUser;
        Instance().user.id = newUser.id;
        LoadSessionData();
        Debug.Log("Succesfull login to user: " + newUser.name + " | " + newUser.id);
        Instance().OnLogin?.Invoke(Instance(), EventArgs.Empty);
    }

    public static bool CheckUser()
    {
        if (Instance().user != null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Saves the data of the session into a file
    /// </summary>
    public static void SaveSessionData()
    {
        if(Instance().user == null)
        {
            Debug.Log("Did not save data. No user data");
            return;
        }
        string id = GetUserIDAsValidFileName();
        DirectoryInfo directory = Directory.CreateDirectory(Application.persistentDataPath + GetSessionFolderName());
        string filename = GetUserIDAsFileName(directory.FullName);
        // Writes json to file. Creates new file if needed
        File.WriteAllText(filename, Instance().ToJSON());
        Instance().OnSaved?.Invoke(Instance(), EventArgs.Empty);
    }

    /// <summary>
    /// Will seatch the persitent data path for a file corresponding to the user ID and load session data as JSON.
    /// </summary>
    /// <returns>True if session data was succsefull extracted</returns>
    public static bool LoadSessionData()
    {
        if (Instance().user == null)
        {
            Debug.LogWarning("Tried to load session data without a user.");
            return false;
        }
        string id = GetUserIDAsValidFileName() + ".json";

        DirectoryInfo directoryInfo = Directory.CreateDirectory(Application.persistentDataPath + GetSessionFolderName());
        if (directoryInfo.Exists)
        {
            foreach (var file in directoryInfo.GetFiles(id))
            {
                Guid temp = Instance().user.id; // The id gets overwritten by the json import 
                EventHandler temp2 = Instance().OnLogin;
                Debug.Log("File content");
                string txt = File.ReadAllText(file.FullName);
                Debug.Log(txt);
                FromJson(txt);
                Instance().user.id = temp; // Reset ID to temp from before json import
                Instance().OnLogin = temp2;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Creates a string of the users ID that can be used as a file name
    /// </summary>
    /// <returns>A sanatized string of the user ID</returns>
    public static string GetUserIDAsValidFileName()
    {
        return MakeValidFileName(Instance().user.id.ToString());
    }

    public static string GetUserIDAsFileName(string fulldirectoryName)
    {
        string ret = fulldirectoryName + @"\" + MakeValidFileName(Instance().user.id.ToString()) + ".json";

        if (Application.platform == RuntimePlatform.Android)
        {
            ret = fulldirectoryName + @"/" + MakeValidFileName(Instance().user.id.ToString()) + ".json";
            return ret;
        }
        return ret;
    }

    /// <summary>
    /// Creates a string that can be used as a file name
    /// </summary>
    /// <param name="name">The target string</param>
    /// <returns>A sanatized version of the input string</returns>
    public static string MakeValidFileName(string name)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

        return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
    }

    public static string GetSessionFolderName()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            return @"/Sessions";
        }

        return @"\Sessions";
    }
    #endregion

    public void Clear()
    {
        user = null;
        questionair = null;
        Instance().OnLogout?.Invoke(Instance(), EventArgs.Empty);
    }

    public void ClearAllLoginEvents()
    {
        if(OnLogin != null)
        {
            foreach (Delegate d in OnLogin.GetInvocationList())
            {
                OnLogin -= (EventHandler)d;
            }
        }
    }

    #region JSON Methods
    public string ToJSON()
    {
        string msg = JsonUtility.ToJson(this);
        return msg;
    }

    public static Session FromJson(string txt)
    {
        instance = JsonUtility.FromJson<Session>(txt);
        return instance;
    }
    #endregion
}
