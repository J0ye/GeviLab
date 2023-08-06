using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNavigation : MonoBehaviour
{
    public List<Location> locations;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    // BEGIN: ed8c6549bwf9
    void Update()
    {
        if (locations == null)
            locations = Locations.GetLocations();
        if (locations != null)
        {   
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                transform.position = locations[0].locationGameObject.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                transform.position = locations[1].locationGameObject.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                transform.position = locations[2].locationGameObject.transform.position;
            }
        }
    }
    // END: ed8c6549bwf9
}
