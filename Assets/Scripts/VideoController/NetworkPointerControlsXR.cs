using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Zinnia.Action;
using System;

[RequireComponent(typeof(LineRenderer))]
public class NetworkPointerControlsXR : NetworkPointerControls
{
    public BooleanAction activationTrigger;
    public BooleanAction actionTrigger;

    protected XRController xRController;

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent<XRController>(out xRController);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            try
            {
                Ray ray = new Ray(transform.position, transform.forward);
                if (activationTrigger.IsActivated && Physics.Raycast(ray, out hit, layers))
                {
                    SetLineEnd(hit.point);
                    if (actionTrigger.IsActivated)
                    {
                        AddPointOfInterest(hit);
                    }

                    if (Input.mouseScrollDelta != Vector2.zero)
                    {
                        ScalePointOfIntereset(Input.mouseScrollDelta.y);
                    }
                }
                else if (xRController == null)
                {
                    ResetLineRenderer();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("There was an Error in the Pointer Conrol Class. Error: " + e);
            }
        }
    }

    protected override void SetLineEnd(Vector3 position)
    {
        line.positionCount = 2;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, position);

        //this.BroadcastRPC("SetLineEnd", NetworkAvatarControls.instance.avatarID, position);
        photonView.RPC("SetLineEnd", RpcTarget.All, NetworkAvatarControls.instance.avatarID, position);
    }
}
