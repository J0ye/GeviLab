using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerControls : MonoBehaviour
{

    private VideoPlayer vp;
    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
    }

    public void Pause()
    {
        if(vp.isPlaying)
        {
            vp.Pause();
        }
        else
        {
            vp.Play();
        }
    }

    public void JumpForward()
    {
        float t = (float)vp.time;
        t += 10f;
        t = Mathf.Clamp(t, 0f, (float)vp.clip.length);
        vp.time = t;
    }

    public void JumpBackward()
    {
        float t = (float)vp.time;
        t -= 10f;
        t = Mathf.Clamp(t, 0f, (float)vp.clip.length);
        vp.time = t;
    }
}
