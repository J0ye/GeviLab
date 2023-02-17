using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerControlsXR : NetworkPointerControlsXR
{
    // Update is called once per frame
    protected override void Update()
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
        else
        {
            ResetLineRenderer();
        }
    }
    protected override void AddPointOfInterest(RaycastHit target)
    {
        // Add Point Of Interest
        GameObject poi = Instantiate(poiPrefab, target.point, Quaternion.identity);
        lastCreatedPOI = poi;
    }

    protected override void SetLineEnd(Vector3 position)
    {
        line.positionCount = 2;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, position);
    }

    protected override void ResetLineRenderer()
    {
        if (line.positionCount > 0)
        {
            line.positionCount = 0;
        }
    }
}
