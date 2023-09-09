using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ChangeVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip clipOne;
    public VideoClip clipTwo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeVideo()
    {
        if(vp.clip == clipOne)
        {
            vp.clip = clipTwo;
        }
        else
        {
            vp.clip = clipOne;
        }
    }

}
