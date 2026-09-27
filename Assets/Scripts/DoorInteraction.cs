using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private float interactionTime = 2f;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private Transform circle;

    private bool playerInside;
    private float timer;
    private bool opened;

    private void Start()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
            countdownText.text = "";
        }
    }

    private void Update()
    {
        if (!playerInside || opened)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.isPressed)
        {
            timer += Time.deltaTime;

            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(true);

                float remaining = Mathf.Max(
                    0f,
                    interactionTime - timer
                );

                countdownText.text = remaining.ToString("0.0");
            }

            if (timer >= interactionTime)
            {
                OpenDoor();
            }
        }
        else
        {
            timer = 0f;

            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(false);
                countdownText.text = "";
            }
        }
    }

    private void OpenDoor()
    {
        opened = true;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
            countdownText.text = "";
        }

        if (circle != null)
        {
            Vector3 rotation = circle.eulerAngles;
            rotation.z = 90f;
            circle.eulerAngles = rotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            timer = 0f;

            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(false);
                countdownText.text = "";
            }
        }
    }
}
