using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PointerController : MonoBehaviour
{
    public KeyCode activationKey = KeyCode.Mouse1;
    public LayerMask layers = new LayerMask();

    private LineRenderer line;
    private RaycastHit hit;
    // Start is called before the first frame update
    void Awake()
    {
        line = GetComponent<LineRenderer>();
        //ResetLineRenderer();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetKey(activationKey) && Physics.Raycast(ray, out hit, layers))
        {
            SetLineEnd(hit.point);
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                AddPointOfInterest(hit);
            }
        }
        else
        {
            ResetLineRenderer();
        }
    }

    private void AddPointOfInterest(RaycastHit target)
    {
        // Point Of Interest
        GameObject poi = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        poi.transform.position = hit.point;
        poi.transform.parent = hit.transform;
    }

    private void SetLineEnd(Vector3 position)
    {
        line.positionCount = 2;

        line.SetPosition(0, transform.position - new Vector3(0f, 0.5f, 0f));
        line.SetPosition(1, position);
    }

    private void ResetLineRenderer()
    {
        line.positionCount = 0;
    }
}
