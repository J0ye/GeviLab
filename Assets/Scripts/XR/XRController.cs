using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

[RequireComponent(typeof(LineRenderer))]
public class XRController : MonoBehaviour
{
    public LayerMask layers = new LayerMask();
    [Tooltip("Determines the length of the ray, which is displayed as long as no valid target is present.")]
    public float cursorLength = 1f;
    public AnimationCurve lineCurve = new AnimationCurve();

    public BooleanAction actionTrigger = new BooleanAction();

    protected LineRenderer lr;
    protected XRUIInteractable target; 
    protected RaycastHit hit;
    protected bool actionTriggered;

    protected void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        InteractWithUI();
    }

    public void InteractWithUI()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, layers))
        {
            if (hit.collider.gameObject.TryGetComponent<XRUIInteractable>(out target))
            {
                PaintConnectionToTarget();

                if (actionTrigger.IsActivated && !actionTriggered)
                {
                    actionTriggered = true;
                    target.SendMessage("Interact");
                }
                else if (!actionTrigger.IsActivated && actionTriggered)
                {
                    actionTriggered = false;
                }
            }
            else
            {
                SetTargetNull();
            }
        }
        else
        {
            SetTargetNull();
        }
    }

    protected void PaintConnectionToTarget()
    {
        lr.positionCount = 10;
        lr.SetPosition(0, transform.position);
        Vector3 directionToTarget = target.transform.position - transform.position;
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        for(int i = 1; i < lr.positionCount; i++)
        {
            Vector3 newPos;
            if (lineCurve.length > i)
            {
                newPos = directionToTarget * lineCurve[i].time;
                newPos += transform.up * lineCurve[i].value;
            }
            else
            {
                newPos = directionToTarget * (i + 1 / lr.positionCount);
            }
            newPos += transform.position;
            float distToNewPos = Vector3.Distance(transform.position, newPos);

            if(i == lr.positionCount -1 || distToNewPos > distanceToTarget)
            {

                lr.SetPosition(i, target.transform.position);
            }
            else
            {
                lr.SetPosition(i, newPos);
            }
        }
    }

    protected virtual void ResetLineRenderer()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.forward * cursorLength);
    }

    protected void SetTargetNull()
    {
        target = null;
        ResetLineRenderer();
    }

    public void OnDrawGizmos()
    {
        if (target == null)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }

}
