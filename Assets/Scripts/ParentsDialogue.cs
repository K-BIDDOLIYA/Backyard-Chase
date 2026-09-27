using UnityEngine;
using TMPro;
using System.Collections;

public class ParentsDialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private float secondLineDelay = 5f;

    [SerializeField] private Color firstLineColor = Color.red;
    [SerializeField] private Color secondLineColor = Color.blue;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerCassette cassette = other.GetComponent<PlayerCassette>();

        if (cassette == null)
            return;

        if (!cassette.HasCassette)
            return;

        triggered = true;
        StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        dialogueText.gameObject.SetActive(true);

        dialogueText.color = firstLineColor;
        dialogueText.text = "Dad, watch this! The baby is not what we think.";

        yield return new WaitForSeconds(secondLineDelay);

        dialogueText.color = secondLineColor;
        dialogueText.text = "Timmy, stop your mischiefs, he's just a baby.";

        yield return new WaitForSeconds(2f);

        dialogueText.gameObject.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(true);

        if (UIManager.Instance != null)
            UIManager.Instance.GameWon();
    }
}
