using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class VideoDecider : MonoBehaviour
{
    // Constants
    float moveSpeed = 2;
    Vector3 narratorInsideSpot = new Vector3(10.5f, 0, 0);
    Vector3 narratorOutsideSpot = new Vector3(-10.5f, 0, 0);
    //Publicly Set
    public VideoPlayer vp;

    public GameObject narratorBox;
    public VideoPlayer narratorVp;

    public int eatsClicked;
    public int drinksClicked;
    public int sleepsClicked;
    public int exercisesClicked;
    public int artsClicked;
    public int mirrorsClicked;

    public GameObject eatButton;
    public GameObject drinkButton;
    public GameObject sleepButton;
    public GameObject exerciseButton;
    public GameObject artButton;
    public GameObject mirrorButton;
    public GameObject escapeButton;
    public GameObject pillsAccept;
    public GameObject pillsReject;

    public VideoClip happyIdle;
    public VideoClip sadIdle;
    public VideoClip existentialIdle;

    public VideoClip eatOne;
    public VideoClip eatTwo;
    public VideoClip eatThree;
    public VideoClip drinkOne;
    public VideoClip drinkTwo;
    public VideoClip drinkThree;
    public VideoClip sleepOne;
    public VideoClip sleepTwo;
    public VideoClip sleepThree;
    
    public VideoClip chokeDeath;
    
    public VideoClip exerciseOne;
    public VideoClip exerciseTwo;
    public VideoClip exerciseThree;
    public VideoClip artOne;
    public VideoClip artTwo;
    public VideoClip artThree;
    public VideoClip mirrorOne;
    public VideoClip mirrorTwo;

    public VideoClip alcohol;
    public VideoClip escape;

    public VideoClip pillsReveal;
    public VideoClip pillsCloseup;
    public VideoClip pillsRefusaal;
    public VideoClip pillsAcceptance;

    public VideoClip trueEnding;

    public VideoClip deathNarrator;

    public int totalStars;
    public ArrayList endingsSeen;


    // Internally Used
    UnityEvent delegateMethod;

    bool narratorSlidingIn = false;
    bool narratorSlidingOut = false;

    // Start is called before the first frame update
    void Start()
    {
        DisableButtons();
        delegateMethod = new UnityEvent();
        endingsSeen = new ArrayList();
        eatsClicked = 0;
        drinksClicked = 0;
        sleepsClicked = 0;
        exercisesClicked = 0;
        artsClicked = 0;
        mirrorsClicked = 0;
        EnableButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (narratorSlidingIn)
        {
            narratorBox.transform.position = Vector3.MoveTowards(narratorBox.transform.position, narratorInsideSpot, moveSpeed * Time.deltaTime);
            if (narratorBox.transform.position.x - narratorInsideSpot.x <= 0.2)
            {
                narratorSlidingIn = false;
                narratorVp.Play();
                narratorVp.loopPointReached += NarratorReset;
            }
        }

        if (narratorSlidingOut)
        {
            narratorBox.transform.position = Vector3.MoveTowards(narratorBox.transform.position, narratorOutsideSpot, moveSpeed * 1.3f * Time.deltaTime);
            if (narratorBox.transform.position.x - narratorOutsideSpot.x <= 0.2)
            {
                narratorSlidingOut = false;
                PlayIdle();
            }
        }
    }

    void PlayVideo(VideoClip newClip)
    {
        DisableButtons();
        vp.isLooping = false;
        vp.clip = newClip;
        vp.Play();
        vp.loopPointReached += InvokeDelegateMethod;
    }

    void InvokeDelegateMethod(UnityEngine.Video.VideoPlayer vp)
    {
        delegateMethod.Invoke();
    }

    void PlayIdle()
    {
        EnableButtons();
        VideoClip idleClip = happyIdle;
        if (eatsClicked >= 2 || drinksClicked >= 2 || sleepsClicked >= 2)
        {
            idleClip = sadIdle;
        }
        if (exercisesClicked >= 2 || artsClicked >= 2 || mirrorsClicked >= 2)
        {
            idleClip = existentialIdle;
        }
        vp.clip = idleClip;
        vp.Play();
        vp.isLooping = true;
    }

    void DisableButtons()
    {
        eatButton.SetActive(false);
        drinkButton.SetActive(false);
        sleepButton.SetActive(false);
        exerciseButton.SetActive(false);
        artButton.SetActive(false);
        mirrorButton.SetActive(false);
    }

    void EnableButtons()
    {
        eatButton.SetActive(true);
        drinkButton.SetActive(true);
        sleepButton.SetActive(true);
        if (eatsClicked + drinksClicked + sleepsClicked >= 2)
        {
            exerciseButton.SetActive(true);
            artButton.SetActive(true);
            mirrorButton.SetActive(true);
        }
    }

    public void EatClick()
    {
        switch(eatsClicked)
        {
            case 0:
                eatsClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(eatOne);
                break;
            case 1:
                eatsClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(eatTwo);
                break;
            case 2:
                delegateMethod.AddListener(EatDeathReset);
                PlayVideo(eatThree);
                break;
        }
    }

    void EatDeathReset()
    {
        if (!endingsSeen.Contains("overeat"))
        {
            totalStars += 1;
            endingsSeen.Add("overeat");
        }
        PlayNarrator(deathNarrator);
        ResetClicks();
    }

    public void DrinkClick()
    {
        if (artsClicked >= 2 || mirrorsClicked >= 2)
        {
            delegateMethod.AddListener(PlayIdle);
            PlayVideo(alcohol);
            return;
        }
        switch (drinksClicked)
        {
            case 0:
                drinksClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(drinkOne);
                break;
            case 1:
                drinksClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(drinkTwo);
                break;
            case 2:
                delegateMethod.AddListener(DrinkDeathReset);
                PlayVideo(drinkThree);
                break;
        }
    }

    void DrinkDeathReset()
    {
        if (!endingsSeen.Contains("overdrink"))
        {
            totalStars += 1;
            endingsSeen.Add("overdrink");
        }
        PlayNarrator(deathNarrator);
        ResetClicks();
    }

    public void SleepClick()
    {
        switch (sleepsClicked)
        {
            case 0:
                sleepsClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(sleepOne);
                break;
            case 1:
                sleepsClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(sleepTwo);
                break;
            case 2:
                delegateMethod.AddListener(SleepDeathReset);
                PlayVideo(sleepThree);
                break;
        }
    }

    void SleepDeathReset()
    {
        if (!endingsSeen.Contains("oversleep"))
        {
            totalStars += 1;
            endingsSeen.Add("oversleep");
        }
        PlayNarrator(deathNarrator);
        ResetClicks();
    }

    void ResetClicks()
    {
        eatsClicked = 0;
        drinksClicked = 0;
        sleepsClicked = 0;
        exercisesClicked = 0;
        artsClicked = 0;
        mirrorsClicked = 0;
    }

    void PlayNarrator(VideoClip clipToPlay)
    {
        narratorVp.clip = clipToPlay;
        PlayIdle();
    }

    void NarratorReset(UnityEngine.Video.VideoPlayer vp)
    {
        narratorSlidingOut = true;
    }
}
