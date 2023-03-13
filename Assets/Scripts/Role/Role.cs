using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role
{
    public const string roleName = "Role";
    public bool playerAuthority;
    /// <summary>
    /// Event for changing acording to new role. Will be called after new role has been set. I.E. Change player UI because access has changed.
    /// </summary>
    public event System.EventHandler OnSwitchRole;
    /// <summary>
    /// Is implemented by every child but also called, so the event gets raised.
    /// </summary>
    public virtual void SetRole()
    {
        OnSwitchRole.Invoke(this, new System.EventArgs());
        // Set the role state to this role and execute functions for changing the application
    }

    public virtual void RemoveRole()
    {
        // Remove role specific conditions and change application 
    }
}
