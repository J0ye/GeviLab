using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMember : Role
{
    public new const string roleName = "GroupMember";
    public override string GetRoleName()
    {
        return "GroupMember";
    }
    public override void SetRole()
    {
        //roleName = "GroupMember";
        playerAuthority = true;
        // Load group session room
    }
}
