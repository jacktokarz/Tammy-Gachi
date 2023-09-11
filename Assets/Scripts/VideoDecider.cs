using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Events;

public class VideoDecider : MonoBehaviour
{
    // Constants
    float moveSpeed = 1.8f;
    Vector3 narratorInsideSpot = new Vector3(-0.4f, 0, 0.9f);
    Vector3 narratorOutsideSpot = new Vector3(-1.3f, 0, 0.9f);


    //Publicly Set
    public VideoPlayer vp;

    public GameObject narratorBox;
    public VideoPlayer narratorVp;

    public GameObject eatButton;
    public GameObject drinkButton;
    public GameObject sleepButton;
    public GameObject exerciseButton;
    public GameObject artButton;
    public GameObject mirrorButton;
    public GameObject escapeButton;
    public GameObject pillsButton;
    public GameObject pillsAccept;
    public GameObject pillsReject;
    public GameObject trueEndingButton;

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
    public VideoClip artOne;
    public VideoClip artTwo;
    public VideoClip artThree;
    public VideoClip mirrorOne;
    public VideoClip mirrorTwo;

    public VideoClip alcohol;
    public VideoClip escape;

    public VideoClip pillsReveal;
    public VideoClip pillsCloseup;
    public VideoClip pillsRefusal;
    public VideoClip pillsAcceptance;

    public VideoClip trueEnding;

    public VideoClip deathNarrator;
    public VideoClip escapeNarrator;
    public VideoClip complacentNarrator;
    public VideoClip insanityNarrator;

    public GameObject starsHolder;
    public Image star1;
    public Image star2;
    public Image star3;
    public Image star4;
    public Image star5;
    public Sprite goldStar;


    // Internally Used
    UnityEvent delegateMethod;
    int totalStars;
    ArrayList endingsSeen;
    int eatsClicked;
    int drinksClicked;
    int sleepsClicked;
    int exercisesClicked;
    int artsClicked;
    int mirrorsClicked;
    bool narratorSlidingIn = false;
    bool narratorSlidingOut = false;

    // Start is called before the first frame update
    void Start()
    {
        DisableButtons();
        narratorBox.transform.position = narratorOutsideSpot;
        delegateMethod = new UnityEvent();
        endingsSeen = new ArrayList();
        trueEndingButton.SetActive(false);
        pillsAccept.SetActive(false);
        pillsReject.SetActive(false);
        eatsClicked = 0;
        drinksClicked = 0;
        sleepsClicked = 0;
        exercisesClicked = 0;
        artsClicked = 0;
        mirrorsClicked = 0;
        PlayIdle();
    }

    // Update is called once per frame
    void Update()
    {
        if (narratorSlidingIn)
        {
            narratorBox.transform.position = Vector3.MoveTowards(narratorBox.transform.position, narratorInsideSpot, moveSpeed * Time.deltaTime);
            if (Mathf.Abs(narratorBox.transform.position.x - narratorInsideSpot.x) <= 0.02f)
            {
                Debug.Log("no more sliding");
                narratorSlidingIn = false;
                narratorVp.Play();
                narratorVp.loopPointReached += NarratorReset;
            }
        }

        if (narratorSlidingOut)
        {
            narratorBox.transform.position = Vector3.MoveTowards(narratorBox.transform.position, narratorOutsideSpot, moveSpeed * 1.1f * Time.deltaTime);
            if (narratorBox.transform.position.x - narratorOutsideSpot.x <= 0.02f)
            {
                narratorSlidingOut = false;
                PlayIdle();
            }
        }
    }

    void PlayVideo(VideoClip newClip)
    {
        Debug.Log("playing vid");
        DisableButtons();
        vp.isLooping = false;
        vp.clip = newClip;
        vp.Play();
        vp.loopPointReached += InvokeDelegateMethod;
    }

    void InvokeDelegateMethod(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("invoking");
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
        vp.loopPointReached -= InvokeDelegateMethod;
        vp.Play();
        vp.isLooping = true;
    }

    void DisableButtons()
    {
        starsHolder.SetActive(false);
        eatButton.SetActive(false);
        drinkButton.SetActive(false);
        sleepButton.SetActive(false);
        exerciseButton.SetActive(false);
        artButton.SetActive(false);
        mirrorButton.SetActive(false);
        escapeButton.SetActive(false);
        pillsButton.SetActive(false);
    }

    void EnableButtons()
    {
        starsHolder.SetActive(true);
        eatButton.SetActive(true);
        drinkButton.SetActive(true);
        sleepButton.SetActive(true);
        if ((eatsClicked + drinksClicked + sleepsClicked >= 2) || totalStars >= 2)
        {
            exerciseButton.SetActive(true);
            artButton.SetActive(true);
            mirrorButton.SetActive(true);
        }
        if (exercisesClicked == 2)
        {
            exerciseButton.SetActive(false);
            escapeButton.SetActive(true);
        }
        if (mirrorsClicked == 2)
        {
            mirrorButton.SetActive(false);
        }
        if (artsClicked == 3)
        {
            artButton.SetActive(false);
        }
        if (artsClicked >= 2 || mirrorsClicked >= 2)
        {
            pillsButton.SetActive(true);
        }
    }

    public void EatClick()
    {
        delegateMethod.RemoveAllListeners();
        switch (eatsClicked)
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
            IncrementStars();
            endingsSeen.Add("overeat");
        }
        PlayNarrator(deathNarrator);
        ResetClicks();
    }

    public void DrinkClick()
    {
        delegateMethod.RemoveAllListeners();
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
        Debug.Log("reset drink");
        if (!endingsSeen.Contains("overdrink"))
        {
            IncrementStars();
            endingsSeen.Add("overdrink");
        }
        PlayNarrator(deathNarrator);
        ResetClicks();
    }

    public void SleepClick()
    {
        delegateMethod.RemoveAllListeners();
        if (eatsClicked > 1)
        {
            delegateMethod.AddListener(ChokeDeathReset);
            PlayVideo(chokeDeath);
            return;
        }
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

    void ChokeDeathReset()
    {
        if (!endingsSeen.Contains("chokeInSleep"))
        {
            IncrementStars();
            endingsSeen.Add("chokeInSleep");
        }
        PlayNarrator(deathNarrator);
        ResetClicks();
    }

    void SleepDeathReset()
    {
        if (!endingsSeen.Contains("oversleep"))
        {
            IncrementStars();
            endingsSeen.Add("oversleep");
        }
        PlayNarrator(insanityNarrator);
        ResetClicks();
    }

    public void ExerciseClick()
    {
        delegateMethod.RemoveAllListeners();
        switch (exercisesClicked)
        {
            case 0:
                exercisesClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(exerciseOne);
                break;
            case 1:
                exercisesClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(exerciseTwo);
                break;
        }
    }

    public void ArtClick()
    {
        delegateMethod.RemoveAllListeners();
        switch (artsClicked)
        {
            case 0:
                artsClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(artOne);
                break;
            case 1:
                artsClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(artTwo);
                break;
            case 2:
                artsClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(artThree);
                break;
        }
    }

    public void MirrorClick()
    {
        delegateMethod.RemoveAllListeners();
        switch (mirrorsClicked)
        {
            case 0:
                mirrorsClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(mirrorOne);
                break;
            case 1:
                mirrorsClicked += 1;
                delegateMethod.AddListener(PlayIdle);
                PlayVideo(mirrorTwo);
                break;
        }
    }

    public void PillsClick()
    {
        delegateMethod.RemoveAllListeners();
        delegateMethod.AddListener(PillChoice);
        PlayVideo(pillsReveal);
    }

    void PillChoice()
    {
        pillsAccept.SetActive(true);
        pillsReject.SetActive(true);
        vp.clip = pillsCloseup;
        vp.isLooping = false;
        vp.Play();
    }

    public void PillAcceptClick()
    {
        pillsAccept.SetActive(false);
        pillsReject.SetActive(false);
        delegateMethod.RemoveAllListeners();
        delegateMethod.AddListener(PillAcceptDeath);
        PlayVideo(pillsAcceptance);
    }

    void PillAcceptDeath()
    {
        if (!endingsSeen.Contains("pillAccept"))
        {
            IncrementStars();
            endingsSeen.Add("pillAccept");
        }
        PlayNarrator(complacentNarrator);
        ResetClicks();
    }

    public void PillRejectClick()
    {
        pillsAccept.SetActive(false);
        pillsReject.SetActive(false);
        delegateMethod.RemoveAllListeners();
        delegateMethod.AddListener(PillRejectDeath);
        PlayVideo(pillsRefusal);
    }

    void PillRejectDeath()
    {
        if(!endingsSeen.Contains("pillReject"))
        {
            IncrementStars();
            endingsSeen.Add("pillReject");
        }
        PlayNarrator(insanityNarrator);
        ResetClicks();
    }

    public void EscapeClick()
    {
        delegateMethod.RemoveAllListeners();
        delegateMethod.AddListener(EscapeDeath);
        PlayVideo(escape);
    }

    void EscapeDeath()
    {
        if (!endingsSeen.Contains("escape"))
        {
            IncrementStars();
            endingsSeen.Add("escape");
        }
        PlayNarrator(escapeNarrator);
        ResetClicks();
    }

    public void TrueEndingClick()
    {
        delegateMethod.RemoveAllListeners();
        PlayVideo(trueEnding);
    }

    void IncrementStars()
    {
        totalStars += 1;
        Image starToChange = star1;
        if(totalStars == 2)
        {
            starToChange = star2;
        }
        else if(totalStars == 3)
        {
            starToChange = star3;
        }
        else if(totalStars == 4)
        {
            starToChange = star4;
        }
        else if(totalStars == 5)
        {
            starToChange = star5;
            trueEndingButton.SetActive(true);
        }
        starToChange.sprite = goldStar;
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
        Debug.Log("play narrator");
        narratorSlidingIn = true;
        narratorVp.clip = clipToPlay;
    }

    void NarratorReset(UnityEngine.Video.VideoPlayer vp)
    {
        narratorSlidingOut = true;
    }
}
