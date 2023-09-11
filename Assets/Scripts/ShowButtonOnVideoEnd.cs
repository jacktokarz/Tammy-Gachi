using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ShowButtonOnVideoEnd : MonoBehaviour
{
    public GameObject proceedButton;
    public VideoPlayer video;


    void Awake()
    {
        proceedButton.SetActive(false);
        video.Play();
        video.loopPointReached += CheckOver;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        proceedButton.SetActive(true);
    }
}
