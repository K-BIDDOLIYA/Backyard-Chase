using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BabyGang : MonoBehaviour
{
    public enum BabyType
    {
        Normal,
        Boss
    }

    [Header("Baby Type")]
    public BabyType babyType = BabyType.Normal;

    [Header("Target")]
    public Transform player;

    [Header("Chase Speed")]
    public float normalSpeed = 3.5f;
    public float bossSpeed = 5f;

    [Header("Cassette Carrier")]
    public float cassetteCarrierSpeed = 2.2f;

    [Header("Curved Movement")]
    public float curveStrength = 1.2f;
    public float curveFrequency = 2f;

    [Header("Cassette")]
    public GameObject cassette;

    private Rigidbody2D rb;

    private bool hasCassette = false;

    private float curveOffset;

    public bool HasCassette => hasCassette;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        curveOffset = Random.Range(0f, 100f);

        if (cassette != null)
        {
            cassette.SetActive(false);
        }
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (hasCassette)
        {
            RunAway();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector2 toPlayer =
            ((Vector2)player.position - rb.position).normalized;

        Vector2 sideways = new Vector2(
            -toPlayer.y,
            toPlayer.x
        );

        float curve =
            Mathf.Sin((Time.time + curveOffset) * curveFrequency)
            * curveStrength;

        Vector2 direction =
            (toPlayer + sideways * curve).normalized;

        float speed =
            babyType == BabyType.Boss
            ? bossSpeed
            : normalSpeed;

        rb.linearVelocity = direction * speed;
    }

    private void RunAway()
    {
        Vector2 awayFromPlayer =
            (rb.position - (Vector2)player.position).normalized;

        Vector2 sideways = new Vector2(
            -awayFromPlayer.y,
            awayFromPlayer.x
        );

        float curve =
            Mathf.Sin((Time.time + curveOffset) * curveFrequency)
            * (curveStrength * 0.5f);

        Vector2 direction =
            (awayFromPlayer + sideways * curve).normalized;

        rb.linearVelocity =
            direction * cassetteCarrierSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        PlayerCassette playerCassette =
            collision.gameObject.GetComponent<PlayerCassette>();

        if (playerCassette == null)
        {
            return;
        }

        TryTakeCassette(playerCassette);
    }

    private void TryTakeCassette(PlayerCassette playerCassette)
    {
        if (hasCassette)
        {
            return;
        }

        bool stolen = playerCassette.TryTakeCassette();

        if (!stolen)
        {
            return;
        }

        TakeCassette();
    }

    private void TakeCassette()
    {
        hasCassette = true;

        if (cassette != null)
        {
            cassette.SetActive(true);
        }

        Debug.Log(gameObject.name + " stole the cassette!");
    }
}

