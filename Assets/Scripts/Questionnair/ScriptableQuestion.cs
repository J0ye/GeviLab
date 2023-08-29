using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Questionnair/new Question", order = 1)]
public class ScriptableQuestion : ScriptableObject
{
    public string titel = "No Titel";
    public string description = "No Description";
    public Sprite image;
    public List<int> correctOptions = new List<int>();
    public List<string> options = new List<string>();
}
