using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public enum PowerUpType
{
    DoubleScore,
    BallSpeedUp,
    SlowMotion
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    public Image backgroundImage;
    public HealthManager healthManager;
    public Sprite bg1, bg2, bg3;

    public AudioSource musicSource;
    public AudioClip music1, music2, music3;

    private bool level2Unlocked, level3Unlocked, level4Unlocked;

    [Header("Pause Menu")]
    public GameObject pauseMenuPanel;

    [Header("Gameplay Controllers")]
    public GameObject[] gameplayScriptsToDisable;

    [Header("Power-Ups")]
    public BallController ball;

    private bool doubleScoreActive = false;
    private float originalBallSpeed;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        backgroundImage.sprite = bg1;
        musicSource.clip = music1;
        musicSource.Play();
        Application.targetFrameRate = 60;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void AddScore(int amount)
    {
        if (doubleScoreActive)
            amount *= 2;

        score += amount;
        scoreText.text = "Score: " + score;

        if (score >= 20 && !level2Unlocked) UnlockLevel2();
        if (score >= 40 && !level3Unlocked) UnlockLevel3();
        if (score >= 100 && !level4Unlocked) UnlockLevel4();
    }

    public int GetScore() => score;

    void UnlockLevel2()
    {
        level2Unlocked = true;
        backgroundImage.sprite = bg2;
        musicSource.clip = music2;
        musicSource.Play();
        PlayerPrefs.SetInt("SkinUnlocked_1", 1);
    }

    void UnlockLevel3()
    {
        level3Unlocked = true;
        backgroundImage.sprite = bg3;
        musicSource.clip = music3;
        musicSource.Play();
        PlayerPrefs.SetInt("SkinUnlocked_2", 1);
    }

    void UnlockLevel4()
    {
        level4Unlocked = true;
        PlayerPrefs.SetInt("SkinUnlocked_3", 1);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        foreach (GameObject obj in gameplayScriptsToDisable)
        {
            obj.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        foreach (GameObject obj in gameplayScriptsToDisable)
        {
            obj.SetActive(true);
        }
    }

    // =========================
    //        POWER-UPS
    // =========================
    public void ActivatePowerUp(PowerUpType type, float duration)
    {
        StartCoroutine(PowerUpRoutine(type, duration));
    }

    private IEnumerator PowerUpRoutine(PowerUpType type, float duration)
    {
        switch (type)
        {
            case PowerUpType.DoubleScore:
                doubleScoreActive = true;
                yield return new WaitForSeconds(duration);
                doubleScoreActive = false;
                break;

            case PowerUpType.BallSpeedUp:
                if (ball != null)
                {
                    originalBallSpeed = ball.jumpForce;
                    ball.jumpForce *= 1.5f;
                    yield return new WaitForSeconds(duration);
                    ball.jumpForce = originalBallSpeed;
                }
                break;

            case PowerUpType.SlowMotion:
                Time.timeScale = 0.2f;
                yield return new WaitForSecondsRealtime(duration);
                Time.timeScale = 1f;
                break;
        }
    }

}
