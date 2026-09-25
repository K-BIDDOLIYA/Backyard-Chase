using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCassette : MonoBehaviour
{
    [Header("Cassette")]
    public GameObject cassette;

    [Header("Dodge")]
    public float dodgeDuration = 0.25f;

    private bool hasCassette = true;
    private bool isDodging = false;
    private bool dodgeUsed = false;

    public bool HasCassette => hasCassette;
    public bool IsDodging => isDodging;

    void Start()
    {
        hasCassette = true;

        if (cassette != null)
        {
            cassette.SetActive(true);
        }
    }

    void Update()
    {
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
        if (!hasCassette)
        {
            return false;
        }

        // Dodge protects against ONE enemy.
        if (isDodging && !dodgeUsed)
        {
            dodgeUsed = true;

            Debug.Log("Dodged one baby!");

            return false;
        }

        // Cassette gets stolen.
        hasCassette = false;

        if (cassette != null)
        {
            cassette.SetActive(false);
        }

        Debug.Log("Cassette stolen!");

        return true;
    }

    public void GetCassetteBack()
    {
        hasCassette = true;

        if (cassette != null)
        {
            cassette.SetActive(true);
        }
    }
}


