using System.Collections;
using System.Collections.Generic;

public class Role
{
    public const string roleName = "Role";
    public bool playerAuthority;
    public virtual void SetRole()
    {
        // Set the role state to this role and execute functions for changing the application
    }

    public virtual void RemoveRole()
    {
        // Remove role specific conditions and change application 
    }
}
