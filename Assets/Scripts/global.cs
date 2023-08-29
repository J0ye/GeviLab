using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class global
{
    public List<Questionaire> questionaires;

    public static global Instance()
    {
        if (instance == null)
        {
            instance = new global();
        }

        return instance;
    }


    private static global instance;

    private global()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("An instance of global already exists.");
        }
        questionaires = new List<Questionaire>();
    }
}
