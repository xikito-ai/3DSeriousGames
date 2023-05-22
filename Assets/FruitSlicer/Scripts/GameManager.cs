using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public TMP_Text cholesterolText;
    public TMP_Text dailyFatText;
    public Text highScoreText;
    public Image cholesterolBar;
    public Image dailyFatBar;

    //settings menu
    public TMP_Text heightText;
    public TMP_Text weightText;
    public TMP_Text ageText;
    public TMP_Text dailyCalories;

    public Image fadeImage;

    private Blade blade;
    private Spawner spawner;
    private Player player;

    private int highScore;
    private int score;
    private int cholesterol;
    private int dailyFat;

    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private AudioSource bombSound;

    private void Awake()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        CheckGameOver();

        //update player info in the settings menu
        heightText.text = FindObjectOfType<Player>().height.ToString();
        weightText.text = FindObjectOfType<Player>().weight.ToString();
        ageText.text = FindObjectOfType<Player>().age.ToString();
        dailyCalories.text = FindObjectOfType<Player>().dailyCalories.ToString();
    }

    public void NewGame()
    {
        Time.timeScale = 1f;

        bgMusic.Play();

        ClearScene();

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();

        cholesterol = 200;
        cholesterolText.text = cholesterol.ToString() + "mg";
        cholesterolBar.fillAmount = 1;
        cholesterolBar.color = Color.green;

        dailyFat = player.dailyFat;
        dailyFatText.text = player.dailyFat.ToString() + "g";
        dailyFatBar.fillAmount = 1;
        dailyFatBar.color = Color.green;

        //update highscore
        highScoreText.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits) {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs) {
            Destroy(bomb.gameObject);
        }
    }

    public void UpdatePoints(int scorePoints, int cholesterolInMg, int dailyFatInG)
    {
        score += scorePoints;
        scoreText.text = score.ToString();

        cholesterol -= cholesterolInMg;
        cholesterolText.text = cholesterol.ToString() + "mg";
        cholesterolBar.fillAmount = (float)cholesterol / 200;
        Debug.Log("chol fill: " + cholesterolBar.fillAmount);
        if(cholesterol > (200/2))
        {
            cholesterolBar.color = Color.green;
        }
        else if (cholesterol >= (200/4))
        {
            cholesterolBar.color = Color.yellow;
        } else
        {
            cholesterolBar.color = Color.red;
        }

        dailyFat -= dailyFatInG;
        dailyFatText.text = dailyFat.ToString() + "g";
        dailyFatBar.fillAmount = (float)dailyFat / player.dailyFat;
        Debug.Log("fat fill: " + dailyFatBar.fillAmount);
        if (dailyFat > (player.dailyFat / 2))
        {
            dailyFatBar.color = Color.green;
        }
        else if (dailyFat >= (player.dailyFat / 4))
        {
            dailyFatBar.color = Color.yellow;
        }
        else
        {
            dailyFatBar.color = Color.red;
        }

        //update highscore if score is higher
        if (score > PlayerPrefs.GetInt("Highscore", 0))
        {
            highScore = score;
            PlayerPrefs.SetInt("Highscore", highScore);
        }
    }

    public void IncreaseScore(int scorePoints)
    {
        score += scorePoints;
        scoreText.text = score.ToString();
    }
    
    public void Explode()
    {
        blade.enabled = false;
        spawner.enabled = false;

        //StartCoroutine(ExplodeSequence());
        bombSound.Play();
        StartCoroutine(GameOverSequence());
        Debug.Log("explode");
    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        bombSound.Play();

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

    //fade-in and fade-out and then restarts the game
    private IEnumerator GameOverSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

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
        if(score < 0 || cholesterol < 0 || dailyFat < 0)
        {
            Debug.Log("Game Over!");

            //NewGame();
            StartCoroutine(GameOverSequence()); //a yellow
        }
    }
}
