using System;
using UnityEngine;



[Serializable]
public class User
{
    public Guid id;
    public string name;
    public string password;

    public User()
    {
        id = Guid.NewGuid();
        name = "empty";
        password = "empty";
    }

    public User(string newName, string newPassword)
    {
        id = Guid.NewGuid();
        name = newName;
        password = newPassword;
    }

    public bool Compare(User other)
    {
        if(other != null)
        {
            if (name == other.name && password == other.password)
            {
                return true;
            }
        }

        return false;
    }
}
