using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    [SerializeField] private TMP_Text countText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject winText;

    [Header("Level Flow")]
    [SerializeField] private GameObject portal; // Enable when conditions met

    [Header("Gameplay Settings")]
    [SerializeField] private float startingTime = 0f; // starts at zero and counts up

    public int Score { get; private set; }
    public bool GameOver { get; private set; }

    private float timer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start() => ResetState();

    public void Update()
    {
        if (GameOver) return;

        // Timer counts up forever
        timer += Time.deltaTime;
        UpdateTimeUI();
    }

    /// <summary>
    /// Reset gameplay state for a fresh start of the game.
    /// </summary>
    public void ResetState()
    {
        GameOver = false;
        Score = 0;
        timer = startingTime;

        if (winText) winText.SetActive(false);
        if (portal) portal.SetActive(false);

        UpdateScoreUI();
        UpdateTimeUI();
    }

    public void AddCoin(int amount = 3)
    {
        if (GameOver) return;

        Score += amount;
        UpdateScoreUI();

        AudioManager.instance.PlaySound(AudioManager.instance.coinCollect);
    }

    public void PlayerDied()
    {
        if (GameOver) return;
        GameOver = true;

        // Play death sound here

        SceneManager.LoadScene("DeathMenu");
    }

    public void LevelWon()
    {
        if (GameOver) return;
        GameOver = true;

        if (winText) winText.SetActive(true);
        // Play a Level Completed sound
    }

    public void UpdateScoreUI()
    {
        if (countText) countText.text = Score.ToString();
    }


    public void UpdateTimeUI()
    {
        if (!timeText) return;
        int secs = Mathf.FloorToInt(timer);
        timeText.text = secs.ToString();
    }
}
