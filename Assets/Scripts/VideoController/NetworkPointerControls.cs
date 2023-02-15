using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

[RequireComponent(typeof(LineRenderer))]
public class NetworkPointerControls : MonoBehaviourPunCallbacks
{
    public KeyCode activationKey = KeyCode.Mouse1;
    public LayerMask layers = new LayerMask();
    [Header("Point of Interest Settings")]
    public GameObject poiPrefab;

    protected LineRenderer line;
    protected RaycastHit hit;
    protected GameObject lastCreatedPOI;
    // Start is called before the first frame update
    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(PhotonNetwork.InRoom)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetKey(activationKey) && Physics.Raycast(ray, out hit, layers))
            {
                SetLineEnd(hit.point);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    AddPointOfInterest(hit);
                }

                if (Input.mouseScrollDelta != Vector2.zero)
                {
                    ScalePointOfIntereset(Input.mouseScrollDelta.y);
                }
            }
            else
            {
                ResetLineRenderer();
            }
        }
    }

    protected void AddPointOfInterest(RaycastHit target)
    {
        // Add Point Of Interest
        GameObject poi = PhotonNetwork.Instantiate(poiPrefab.name, target.point, Quaternion.identity);
        lastCreatedPOI = poi;
    }

    protected void ScalePointOfIntereset(float val)
    {
        if(lastCreatedPOI != null)
        {
            float scale = lastCreatedPOI.transform.localScale.x;
            lastCreatedPOI.transform.DOScale(scale + (val * Time.deltaTime), Time.deltaTime);
        }
    }

    protected virtual void SetLineEnd(Vector3 position)
    {
        line.positionCount = 2;

        line.SetPosition(0, transform.position - new Vector3(0f, 0.5f, 0f));
        line.SetPosition(1, position);

        this.BroadcastRPC("SetLineEnd", NetworkAvatarControls.instance.avatarID, position);
    }

    protected void ResetLineRenderer()
    {
        if(line.positionCount > 0)
        {
            line.positionCount = 0;
            this.BroadcastRPC("ResetLineRenderer", NetworkAvatarControls.instance.avatarID);
        }
    }

    [PunRPC]
    public void SetLineEnd(int id, Vector3 position)
    {
        try
        {
            LineRenderer lr;
            PhotonView view = PhotonNetwork.GetPhotonView(id);
            if (view.TryGetComponent<LineRenderer>(out lr))
            {
                lr.positionCount = 2;

                lr.SetPosition(0, view.transform.position);
                lr.SetPosition(1, position);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Pointer not working. Could not set line because: " + e);
        }
    }

    [PunRPC]
    public void ResetLineRenderer(int id)
    {
        try
        {
            LineRenderer lr;
            PhotonView view = PhotonNetwork.GetPhotonView(id);
            if (view.TryGetComponent<LineRenderer>(out lr))
            {
                lr.positionCount = 0;
            }
        }
        catch(System.Exception e)
        {
            Debug.LogWarning("Pointer not working. Could not reset line because: " + e);
        }
    }
}
