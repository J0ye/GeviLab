using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Educator : Role
{
    public new const string roleName = "Educator";

    public override void SetRole()
    {
        playerAuthority = true;
        
        // manage session
    }
}
