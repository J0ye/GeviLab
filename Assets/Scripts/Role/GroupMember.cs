using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMember : Role
{
    public new const string roleName = "GroupMember";

    public override void SetRole()
    {
        playerAuthority = true;
        base.SetRole();
        // Load group session room
    }
}
