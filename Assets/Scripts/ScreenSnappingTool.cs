using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenSnappingTool : MonoBehaviour
{
    public LayerMask snapLayer; // Layer to snap to
    public float raycastMaxRange = 1000f;  // Maximum range of the raycast
    public Vector3 positionOffset = Vector3.zero;  // Offset for the final position


    private BoxCollider boxCollider; // The box collider on this GameObject

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        //SnapCorners();
    }

    [ContextMenu("Snap Corners")]
    private void SnapCorners()
    {
        // Create a new list to store hit points
        List<Vector3> hitPoints = new List<Vector3>();

        // Raycast each corner
        foreach (Vector3 corner in GetCorners())
        {
            Ray ray = new Ray(corner, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, snapLayer))
            {
                hitPoints.Add(hit.point);
            }
        }

        // Only adjust the position if all corners hit the spherical surface
        if (hitPoints.Count == 4)
        {
            Vector3 newCenter = (hitPoints[0] + hitPoints[1] + hitPoints[2] + hitPoints[3]) / 4;
            transform.position = newCenter + positionOffset;
        }

    }

    private Vector3[] GetCorners()
    {
        if (boxCollider != null)
        {
            // Calculate the corners of the box collider
            Vector3[] corners = new Vector3[4]
            {
            transform.TransformPoint(boxCollider.center + new Vector3(boxCollider.size.x, boxCollider.size.y, 0) * 0.5f),
            transform.TransformPoint(boxCollider.center + new Vector3(-boxCollider.size.x, boxCollider.size.y, 0) * 0.5f),
            transform.TransformPoint(boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, 0) * 0.5f),
            transform.TransformPoint(boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, 0) * 0.5f)
            };
            return corners;
        }
        return null;
    }

    void OnDrawGizmos()
    {
        // Define a color for the rays (red) and the corners (blue)
        Color rayColor = Color.red;
        Color cornerColor = Color.blue;

        // Draw the rays
        foreach (Vector3 corner in GetCorners())
        {
            Gizmos.color = rayColor;
            Gizmos.DrawRay(corner, transform.forward * raycastMaxRange);
        }

        // Draw the corners
        foreach (Vector3 corner in GetCorners())
        {
            Gizmos.color = cornerColor;
            Gizmos.DrawWireSphere(corner, 0.1f);
        }
    }
}
