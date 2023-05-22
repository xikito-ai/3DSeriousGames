using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARTimer : MonoBehaviour
{
    private Image timerBar;
    float timePerRound = 30f; // seconds per round (timer bar)
    float timeLeftInRound; // timer countdown

    public TMP_Text roundText;
    public TMP_Text roundSlideInText;
    private int rounds = 0;
    public GameObject roundCounter;

    public bool timeIsUp = false;

    public ARGameManager aRGameManager;
    private ARSpawner spawnerRef;

    public int Rounds { get => rounds; set => rounds = value; }

    private void Awake()
    {
        spawnerRef = aRGameManager.groundPlacementRef.PlacedObject.GetComponentInChildren<ARSpawner>();
        timerBar = GetComponent<Image>();        
    }

    void Update()
    {
        if (aRGameManager.groundPlacementRef.GroundSet())
        {
            //start the timer after ground is set
            spawnerRef = aRGameManager.groundPlacementRef.SpawnerRef;

            if (timeLeftInRound > 0)
            {
                // timer bar still running
                timeIsUp = false;

                //timer countdown
                timeLeftInRound -= Time.deltaTime;
                timerBar.fillAmount = timeLeftInRound / timePerRound;
            }
            else
            {
                // timer bar run out fully
                timeIsUp = true;

                // update score
                Debug.Log("Round over, " + "after round points: " + spawnerRef.friendMinusFoe);
                aRGameManager.UpdateScore(spawnerRef.friendMinusFoe);
                Debug.Log("Current score: " + aRGameManager.Score);

                // increase round counter
                rounds++;
                roundText.text = rounds.ToString();

                // dispay round counter
                StartCoroutine(DisplayRounds());

                // reset timer
                ResetTimer();
            }
        }
    }

    public void ResetTimer()
    {
        timeLeftInRound = timePerRound;
    }

    public void ResetRounds()
    {
        rounds = 0;
        roundText.text = rounds.ToString();
    }

    public IEnumerator DisplayRounds()
    {
        roundCounter.SetActive(true);
        roundSlideInText.text = "Round: " + rounds;

        yield return new WaitForSecondsRealtime(3f);
        roundCounter.SetActive(false);
    }
}
