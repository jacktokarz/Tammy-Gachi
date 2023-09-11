using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ChangeVideo : MonoBehaviour
{
    public VideoPlayer vp;

    public void changeVideo(VideoClip newClip)
    {
            vp.clip = newClip;
    }

}
