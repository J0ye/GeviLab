using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ImageQuestion", menuName = "Questionnair/new Image Question", order = 2)]
public class ScriptableImageQuestion : ScriptableQuestion
{
    public List<Sprite> imageOptions = new List<Sprite>();
    public bool setNativeSizeOnClick = false;
}
