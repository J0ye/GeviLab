using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(RectTransform))]
public class UIAnimation : MonoBehaviour
{
    public Vector3 targetPosition = Vector3.one;
    public float targetScale = 1f;
    public float duration;
    [Tooltip("The actual target position will be calculated with target vector + start position, if this is true.")]
    public bool relativeToStartPosition = true;
    [Tooltip("Is rpositioned to the target and than moves out to the startposition.")]
    public bool startOnTarget = false;
    public bool startOnAwake = false;

    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        if(startOnTarget)
        {
            Vector3 temp = transform.position;
            transform.position = targetPosition;
            targetPosition = temp;
        }
        if (relativeToStartPosition) targetPosition += transform.position;
        if (startOnAwake) StartAnimation();
    }

    public void StartAnimation()
    {
        transform.DOMove(targetPosition, duration);
        transform.DOScale(targetScale, duration);
    }
}
