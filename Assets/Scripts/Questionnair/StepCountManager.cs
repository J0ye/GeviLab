using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCountManager : MonoBehaviour
{
    public List<SwitchImage> steps = new List<SwitchImage>();
    // Start is called before the first frame update
    void Start()
    {
        FillList();
    }

    public void UpdateDisplay(int val)
    {
        val = Mathf.Clamp(val, 0, steps.Count - 1);
        int counter = 1;
        foreach(SwitchImage si in steps)
        {
            if(val < counter)
            {
                si.SetDisplayState(false);
            }
            counter++;
        }
    }

    protected void FillList()
    {
        foreach(Transform child in transform)
        {
            SwitchImage temp;
            if(child.TryGetComponent<SwitchImage>(out temp))
            {
                steps.Add(temp);
            }
        }
    }
}
