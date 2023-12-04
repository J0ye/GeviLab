using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Educator : Role
{
    public new const string roleName = "Educator";
    public override string GetRoleName()
    {
        return "Educator";
    }

    public override void SetRole()
    {
        //roleName 
        playerAuthority = true;
        // manage session
    }
}
