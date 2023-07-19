using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zinnia.Action;

[RequireComponent(typeof(LineRenderer))]
public class XRController : MonoBehaviour
{
    public LayerMask layers = new LayerMask();
    [Tooltip("Determines the length of the ray, which is displayed as long as no valid target is present.")]
    public float cursorLength = 1f;
    public AnimationCurve lineCurve = new AnimationCurve();
    public int linePositionCount = 100;
    public float lineRenderDuration = 1f;

    public BooleanAction actionTrigger = new BooleanAction();

    [Header("UI Pointer Settings")]
    public GraphicRaycaster graphicRaycaster;
    public EventSystem eventSystem;

    protected LineRenderer lr;
    protected XRUIInteractable target; 
    protected RaycastHit hit;
    protected GameObject lrPlaceHolder;
    protected bool actionTriggered;

    protected void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        InteractWith3DUI();
    }

    public void InteractWith3DUI()
    {
        if (MakeRayCast(out hit))
        {
            if (hit.collider.gameObject.TryGetComponent<XRUIInteractable>(out XRUIInteractable temp))
            {
                try
                {
                    //This part crashes if the an annotation is opened
                    target = temp;
                    StartCoroutine(PaintConnectionToTarget(lineRenderDuration));
                    target.SendMessage(nameof(XRUIInteractable.StartHover));

                    if (actionTrigger.IsActivated && !actionTriggered)
                    {
                        actionTriggered = true;
                        target.SendMessage(nameof(XRUIInteractable.Interact));
                    }
                    else if (!actionTrigger.IsActivated && actionTriggered)
                    {
                        actionTriggered = false;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("XRInteractor crashed because " + e);
                }
            }
            else if(hit.collider.gameObject.TryGetComponent<Canvas>(out Canvas c))
            {
                target = temp;
                PaintConnectionToPoint(hit.point);
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

    public void InteractWith2DUI()
    {
        // Kann das weg???
        if(MakeRayCast(out hit))
        {

        }
    }

    protected bool MakeRayCast(out RaycastHit hitResult)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit temp;
        if (Physics.Raycast(ray, out temp, layers))
        {
            hitResult = temp;
            return true;
        }
        hitResult = temp;
        return false;
    }

    #region Line Renderer Logic

    public Vector3[] GetConnectionLineToTarget()
    {
        Vector3[] ret = new Vector3[lr.positionCount];
        // Set all positions to origin of controller
        for (int i = 0; i < lr.positionCount; i++)
        {
            ret[i] = transform.position;
        }
        Vector3 directionToTarget = target.transform.position - transform.position;
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        // Iterate over each point in the line renderer, starting from the second point (index 1)
        for (int i = 1; i < lr.positionCount; i++)
        {
            Vector3 newPos;

            // If the current index is less than the length of the lineCurve array
            if (lineCurve.length > i)
            {
                // Calculate a new position based on the time and value of the lineCurve at the current index
                // This position is relative to the direction to the target and adjusted vertically by the curve's value
                newPos = directionToTarget * lineCurve[i].time;
                newPos += transform.up * lineCurve[i].value;
            }
            else
            {
                // If there's no corresponding point in lineCurve, interpolate the position linearly between the current and target objects
                newPos = directionToTarget * (i + 1 / lr.positionCount);
            }

            // Translate the new position from being relative to the origin to being relative to the current object's position
            newPos += transform.position;

            // Calculate the distance from the current object to the new position
            float distToNewPos = Vector3.Distance(transform.position, newPos);

            // If the current index is the last point in the line renderer or if the distance to the new position is greater than the distance to the target
            if (i == lr.positionCount - 1 || distToNewPos > distanceToTarget)
            {
                // Set the line's point directly to the target's position
                ret[i] = target.transform.position;
            }
            else
            {
                // Otherwise, set the line's point to the new position
                lr.SetPosition(i, newPos);
                ret[i] = newPos;
            }

            // Iterate over the remaining points in the line renderer
            for (int j = i++; j < lr.positionCount; j++)
            {
                // Set each remaining point to the new position
                ret[j] = newPos;
            }
        }
        return ret;
    }
    protected void PaintConnectionToTarget()
    {
        lr.positionCount = linePositionCount;
        lr.SetPositions(GetConnectionLineToTarget());
    }

    protected void PaintConnectionToPoint(Vector3 point)
    {
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, point);
    }

    protected IEnumerator PaintConnectionToTarget(float duration)
    {
        lr.positionCount = linePositionCount;
        // Set all positions to origin of controller
        for (int i = 0; i < lr.positionCount; i++)
        {
            lr.SetPosition(i, transform.position);
        }
        Vector3 directionToTarget = target.transform.position - transform.position;
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        // Iterate over each point in the line renderer, starting from the second point (index 1)
        for (int i = 1; i < lr.positionCount; i++)
        {
            Vector3 newPos;

            // If the current index is less than the length of the lineCurve array
            if (lineCurve.length > i)
            {
                // Calculate a new position based on the time and value of the lineCurve at the current index
                // This position is relative to the direction to the target and adjusted vertically by the curve's value
                newPos = directionToTarget * lineCurve[i].time;
                newPos += transform.up * lineCurve[i].value;
            }
            else
            {
                // If there's no corresponding point in lineCurve, interpolate the position linearly between the current and target objects
                newPos = directionToTarget * (i + 1 / lr.positionCount);
            }

            // Translate the new position from being relative to the origin to being relative to the current object's position
            newPos += transform.position;

            // Calculate the distance from the current object to the new position
            float distToNewPos = Vector3.Distance(transform.position, newPos);

            // If the current index is the last point in the line renderer or if the distance to the new position is greater than the distance to the target
            if (i == lr.positionCount - 1 || distToNewPos > distanceToTarget)
            {
                // Set the line's point directly to the target's position
                lr.SetPosition(i, target.transform.position);
            }
            else
            {
                // Otherwise, set the line's point to the new position
                lr.SetPosition(i, newPos);
            }

            // Iterate over the remaining points in the line renderer
            for (int j = i++; j < lr.positionCount; j++)
            {
                // Set each remaining point to the new position
                lr.SetPosition(j, newPos);
            }
            yield return new WaitForSeconds(duration / lr.positionCount);
        }
    }
#if UNITY_EDITOR
    [ContextMenu("Add Keys To Curve")]
    public void AddKeysToCurve()
    {
        int totalKeys = linePositionCount; // Desired total number of keys

        // Create a new curve to avoid modifying the original
        AnimationCurve newCurve = new AnimationCurve();

        // Add keys to the new curve by interpolating the original curve
        for (int i = 0; i < totalKeys; i++)
        {
            // Calculate the time of the new key based on the desired total number of keys
            float time = i / (float)(totalKeys - 1);

            // Evaluate the original curve at the new key's time
            float value = lineCurve.Evaluate(time);

            // Add the new key to the curve
            Keyframe newKey = new Keyframe(time, value);
            newCurve.AddKey(newKey);
        }

        // Smooth the tangents of the curve to maintain its shape
        for (int i = 0; i < newCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(newCurve, i, 
                AnimationUtility.TangentMode.ClampedAuto);
            AnimationUtility.SetKeyRightTangentMode(newCurve, i, AnimationUtility.TangentMode.ClampedAuto);
        }

        // Update the curve
        lineCurve = newCurve;
    }
#endif
    protected void SetTargetNull()
    {
        if (target != null)
        {
            ResetLineRenderer(true);
            target.SendMessage(nameof(XRUIInteractable.EndHover));
            target = null;
        }
        ResetLineRenderer();
    }

    protected virtual void ResetLineRenderer()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.forward * cursorLength);
    }

    protected virtual void ResetLineRenderer(bool doAnimation)
    {
        if (!doAnimation) ResetLineRenderer();

        GameObject tempLRPrefab = Resources.Load<GameObject>("TempLineRenderer");
        if (lrPlaceHolder != null) Destroy(lrPlaceHolder);
        lrPlaceHolder  = Instantiate(tempLRPrefab, transform.position, transform.rotation);
        LineRenderer tempLR = lrPlaceHolder.GetComponent<LineRenderer>();
        tempLR.positionCount = lr.positionCount;
        Vector3[] temp = new Vector3[lr.positionCount];
        lr.GetPositions(temp);
        tempLR.SetPositions(temp);
        //Destroy(lrPlaceHolder, lineRenderDuration);
        lrPlaceHolder.GetComponent<LineRendererRemover>().removalTime = lineRenderDuration;
        lrPlaceHolder.GetComponent<LineRendererRemover>().StartAnimation();
    }
    #endregion

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
