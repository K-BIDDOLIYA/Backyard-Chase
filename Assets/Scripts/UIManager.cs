using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Start")]
    [SerializeField] private GameObject startButton;

    [Header("Restart Buttons")]
    [SerializeField] private GameObject restartButton1;
    [SerializeField] private GameObject restartButton2;

    [Header("Time Texts")]
    [SerializeField] private TMP_Text timeText1;
    [SerializeField] private TMP_Text timeText2;

    private float elapsedTime;
    private bool gameStarted;
    private bool gameFinished;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        elapsedTime = 0f;
        gameStarted = false;
        gameFinished = false;

        Time.timeScale = 0f;

        if (startButton != null)
            startButton.SetActive(true);

        if (restartButton1 != null)
            restartButton1.SetActive(false);

        if (restartButton2 != null)
            restartButton2.SetActive(false);

        SetTimeText();
    }

    private void Update()
    {
        if (!gameStarted)
            return;

        if (gameFinished)
            return;

        elapsedTime += Time.deltaTime;

        SetTimeText();
    }

    private void SetTimeText()
    {
        string time = "Time: " + elapsedTime.ToString("0.0") + "s";

        if (timeText1 != null)
            timeText1.text = time;

        if (timeText2 != null)
            timeText2.text = time;
    }

    public void StartGame()
    {
        if (gameStarted)
            return;

        gameStarted = true;
        gameFinished = false;

        if (startButton != null)
            startButton.SetActive(false);

        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void GameLost()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        float finalTime = elapsedTime;

        SetTimeText();

        if (restartButton1 != null)
            restartButton1.SetActive(true);

        if (restartButton2 != null)
            restartButton2.SetActive(true);

        Time.timeScale = 0f;
    }

    public void GameWon()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        float finalTime = elapsedTime;

        SetTimeText();

        if (restartButton1 != null)
            restartButton1.SetActive(true);

        if (restartButton2 != null)
            restartButton2.SetActive(true);

        Time.timeScale = 0f;
    }
}
