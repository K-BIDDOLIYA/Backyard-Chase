using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCassette : MonoBehaviour
{
    [Header("Cassette")]
    [SerializeField] private GameObject cassette;

    [Header("Dodge")]
    [SerializeField] private float dodgeDuration = 0.25f;

    [Header("Cassette Timer")]
    [SerializeField] private float cassetteTimeLimit = 10f;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    private bool hasCassette = true;
    private bool isDodging = false;
    private bool dodgeUsed = false;
    private bool cassetteProtected = false;
    private bool gameOver = false;

    private Coroutine cassetteTimerCoroutine;

    public bool HasCassette => hasCassette;
    public bool IsDodging => isDodging;
    public bool CassetteProtected => cassetteProtected;

    private void Awake()
    {
        hasCassette = true;
        isDodging = false;
        dodgeUsed = false;
        cassetteProtected = false;
        gameOver = false;

        // Game Over panel should be hidden at the beginning.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameOver)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.fKey.wasPressedThisFrame &&
            !isDodging)
        {
            StartCoroutine(Dodge());
        }
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        dodgeUsed = false;

        yield return new WaitForSeconds(dodgeDuration);

        isDodging = false;
    }

    public bool TryTakeCassette()
    {
        // Player doesn't have the cassette.
        if (!hasCassette)
            return false;

        // Cassette is temporarily protected.
        if (cassetteProtected)
        {
            Debug.Log("Cassette is protected!");
            return false;
        }

        // Dodge protects against one enemy.
        if (isDodging && !dodgeUsed)
        {
            dodgeUsed = true;

            Debug.Log("Dodged one baby!");

            return false;
        }

        // Cassette stolen.
        hasCassette = false;

        if (cassette != null)
        {
            cassette.SetActive(false);
        }

        Debug.Log("Cassette stolen! 10 second timer started.");

        // Start the invisible 10-second timer.
        StartCassetteTimer();

        return true;
    }

    private void StartCassetteTimer()
    {
        // Stop an old timer if one somehow exists.
        if (cassetteTimerCoroutine != null)
        {
            StopCoroutine(cassetteTimerCoroutine);
        }

        cassetteTimerCoroutine =
            StartCoroutine(CassetteTimer());
    }

    private IEnumerator CassetteTimer()
    {
        yield return new WaitForSeconds(cassetteTimeLimit);

        // If the player still doesn't have the cassette,
        // the timer has expired.
        if (!hasCassette && !gameOver)
        {
            GameOver();
        }

        cassetteTimerCoroutine = null;
    }

    public void GetCassetteBack()
    {
        // If the game is already over, don't give the cassette back.
        if (gameOver)
            return;

        hasCassette = true;

        if (cassette != null)
        {
            cassette.SetActive(true);
        }

        // Stop the 10-second countdown.
        if (cassetteTimerCoroutine != null)
        {
            StopCoroutine(cassetteTimerCoroutine);
            cassetteTimerCoroutine = null;
        }

        StartCoroutine(CassetteProtection());

        Debug.Log("Cassette returned to player!");
    }

    private IEnumerator CassetteProtection()
    {
        cassetteProtected = true;

        yield return new WaitForSeconds(10f);

        cassetteProtected = false;

        Debug.Log("Cassette protection ended.");
    }

    private void GameOver()
    {
        gameOver = true;

        // Stop player movement.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Show Game Over panel.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Debug.Log("GAME OVER - Cassette was not recovered in time!");
    }
}
