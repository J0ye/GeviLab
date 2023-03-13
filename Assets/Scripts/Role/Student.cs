using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Role
{
    public new const string roleName = "Student";
    public override void SetRole()
    {
        playerAuthority = false;
        base.SetRole();
        // Allow single user session
    }
}
