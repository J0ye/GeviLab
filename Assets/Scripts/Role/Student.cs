using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Role
{
    public new const string roleName = "Student";
    public override string GetRoleName()
    {
        return roleName;
    }

    public override void SetRole()
    {
        playerAuthority = false;
        // Allow single user session
    }
}
