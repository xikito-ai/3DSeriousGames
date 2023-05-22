using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARGameManager : MonoBehaviour
{
    // references
    public ARTapToPlaceObjects groundPlacementRef;
    private ARSpawner spawnerRef;
    private ARPlayer aRplayer;
    private ARWater aRWaterRef;

    // player
    private int score;
    private int highScore;
    private int dailyFat;

    // game UI
    public TMP_Text scoreText;
    public TMP_Text roundsText;
    public TMP_Text highScoreText;
    public TMP_Text roundText;
    public TMP_Text roundSlideInText;
    public GameObject roundCounter;

    // timer
    public Image timerBar;
    float timePerRound = 15f; // seconds per round
    float timeLeftInRound; // timer countdown
    private int rounds = 0;
    public bool timeIsUp = false;

    public Image fadeImage;

    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private AudioSource gameoverSound;

    private bool firstStartOfGame = true;

    public int Score { get => score; set => score = value; }

    private void Awake()
    {
        spawnerRef = FindObjectOfType<ARSpawner>();
        aRplayer = FindObjectOfType<ARPlayer>();
        aRWaterRef = FindObjectOfType<ARWater>();
    }

    void Update()
    {
        if (groundPlacementRef.GroundSet())
        {
            // start the game after ground is set
            spawnerRef = groundPlacementRef.SpawnerRef;

            if (firstStartOfGame)
            {
                // at the start of the game wait a bit to let the game pieces spawn first
                Debug.Log("Game Start");
                StartCoroutine(StartDelaySequence());
            }

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
                UpdateScore(spawnerRef.friendMinusFoe);

                //increase round counter
                rounds++;
                roundText.text = rounds.ToString() + " / 6";

                // dispay round counter
                StartCoroutine(DisplayRounds());

                // reset timer
                ResetTimer();
            }

            CheckDayOver();

            CheckGameOver();
        }
    }

    private void CheckDayOver()
    {
        if (rounds >= 6)
        {
            // after six rounds => simulated one full day
            Debug.Log("Day Over");
            NewGame();
        }
    }

    public IEnumerator StartDelaySequence()
    {
        yield return new WaitForSeconds(3f); // let the game pieces spawn first

        firstStartOfGame = false; // after game started once, set this to false to not inititate this again during the game
    }

    public void NewGame()
    {
        Time.timeScale = 1f;

        bgMusic.Play();

        ClearScene();

        firstStartOfGame = true;

        score = 0;
        scoreText.text = Score.ToString();

        // update highscore display
        highScoreText.text = PlayerPrefs.GetInt("ARHighscore", 0).ToString();

        // reset round timer
        ResetTimer();

        // reset round counter
        ResetRounds();

        // reset water counter
        aRWaterRef.ResetWaterCounter();
    }

    private void ClearScene()
    {
        ARGamePiece[] piecesOnGround = FindObjectsOfType<ARGamePiece>();

        foreach (ARGamePiece piece in piecesOnGround)
        {
            Destroy(piece.gameObject);
        }

        groundPlacementRef.SpawnerRef.spawnedPieces.Clear();
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();

        // update highscore if score is higher
        if (score > PlayerPrefs.GetInt("ARHighscore", 0))
        {
            highScore = Score;
            PlayerPrefs.SetInt("ARHighscore", highScore);
        }
    }

    private IEnumerator GameOverSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        gameoverSound.Play();

        // Fade to white
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        NewGame();

        elapsed = 0f;

        // Fade back in
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }

    private void CheckGameOver()
    {
        if (score < 0)
        {
            Debug.Log("Game Over!");

            StartCoroutine(GameOverSequence());
        }
    }

    public void ResetTimer()
    {
        timeLeftInRound = timePerRound;
    }

    public void ResetRounds()
    {
        rounds = 0;
        roundText.text = rounds.ToString() + " / 6";
    }

    public IEnumerator DisplayRounds()
    {
        roundCounter.SetActive(true);
        roundSlideInText.text = "Round: " + rounds;

        yield return new WaitForSecondsRealtime(3f);
        roundCounter.SetActive(false);
    }
}
