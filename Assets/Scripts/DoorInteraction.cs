using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DoorInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionTime = 2f;

    [Header("Interaction Range")]
    [SerializeField] private float interactionRange = 1.5f;

    [Header("Progress UI")]
    [SerializeField] private Image progressCircle;

    [Header("Door")]
    [SerializeField] private GameObject doorVisual;
    [SerializeField] private Collider2D doorCollider;

    private Transform player;
    private bool doorOpened = false;

    private float interactionProgress = 0f;

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        if (progressCircle != null)
        {
            progressCircle.fillAmount = 0f;
            progressCircle.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (doorOpened || player == null)
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        bool playerNearby =
            distance <= interactionRange;

        if (!playerNearby)
        {
            ResetInteraction();
            return;
        }

        if (Keyboard.current != null &&
            Keyboard.current.eKey.isPressed)
        {
            HoldDoor();
        }
        else
        {
            ResetInteraction();
        }
    }

    private void HoldDoor()
    {
        interactionProgress +=
            Time.deltaTime / interactionTime;

        interactionProgress =
            Mathf.Clamp01(interactionProgress);

        if (progressCircle != null)
        {
            progressCircle.gameObject.SetActive(true);

            progressCircle.fillAmount =
                interactionProgress;
        }

        if (interactionProgress >= 1f)
        {
            OpenDoor();
        }
    }

    private void ResetInteraction()
    {
        interactionProgress = 0f;

        if (progressCircle != null)
        {
            progressCircle.fillAmount = 0f;
            progressCircle.gameObject.SetActive(false);
        }
    }

    private void OpenDoor()
    {
        doorOpened = true;

        if (progressCircle != null)
        {
            progressCircle.fillAmount = 1f;
            progressCircle.gameObject.SetActive(false);
        }

        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        if (doorVisual != null)
        {
            doorVisual.SetActive(false);
        }

        Debug.Log("Door opened!");
    }
}
