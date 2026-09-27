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
    public float curveChangeInterval = 0.6f;

    [Header("Cassette")]
    public GameObject cassette;

    [Header("Look")]
    public bool rotateBaby = true;

    private Rigidbody2D rb;

    private bool hasCassette = false;

    private float curveDirection;
    private float currentCurveStrength;
    private float nextCurveChange;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ChooseNewCurve();

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

        nextCurveChange =
            Time.time +
            Random.Range(
                0.1f,
                curveChangeInterval
            );
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            UpdateBossAnimation();
            return;
        }

        if (Time.time >= nextCurveChange)
        {
            ChooseNewCurve();

            nextCurveChange =
                Time.time +
                Random.Range(
                    curveChangeInterval * 0.6f,
                    curveChangeInterval * 1.4f
                );
        }

        if (hasCassette)
        {
            RunAway();
        }
        else
        {
            ChasePlayer();
        }

        UpdateBossAnimation();
    }

    private void ChooseNewCurve()
    {
        curveDirection =
            Random.Range(0, 2) == 0
            ? -1f
            : 1f;

        currentCurveStrength =
            Random.Range(
                curveStrength * 0.35f,
                curveStrength
            );
    }

    private void ChasePlayer()
    {
        Vector2 toPlayer =
            ((Vector2)player.position - rb.position)
            .normalized;

        Vector2 sideways =
            new Vector2(
                -toPlayer.y,
                toPlayer.x
            );

        Vector2 direction =
            toPlayer +
            sideways *
            curveDirection *
            currentCurveStrength;

        direction.Normalize();

        float speed =
            babyType == BabyType.Boss
            ? bossSpeed
            : normalSpeed;

        rb.linearVelocity =
            direction * speed;

        RotateTowardsDirection(toPlayer);
    }

    private void RunAway()
    {
        Vector2 awayFromPlayer =
            (rb.position -
             (Vector2)player.position)
            .normalized;

        Vector2 sideways =
            new Vector2(
                -awayFromPlayer.y,
                awayFromPlayer.x
            );

        Vector2 direction =
            awayFromPlayer +
            sideways *
            curveDirection *
            (currentCurveStrength * 0.5f);

        direction.Normalize();

        rb.linearVelocity =
            direction * cassetteCarrierSpeed;

        RotateTowardsDirection(direction);
    }

    private void RotateTowardsDirection(Vector2 direction)
    {
        return;
    }

    private void OnCollisionEnter2D(
        Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        PlayerCassette playerCassette =
            collision.gameObject
                .GetComponent<PlayerCassette>();

        if (playerCassette == null)
            return;

        if (hasCassette)
        {
            ReturnCassetteToPlayer(playerCassette);
            return;
        }

        TryTakeCassette(playerCassette);
    }

    private void TryTakeCassette(
        PlayerCassette playerCassette)
    {
        if (hasCassette)
            return;

        bool stolen =
            playerCassette.TryTakeCassette();

        if (!stolen)
            return;

        TakeCassette();
    }

    private void TakeCassette()
    {
        hasCassette = true;

        if (cassette != null)
        {
            cassette.SetActive(true);
        }

        Debug.Log(
            gameObject.name +
            " stole the cassette!"
        );
    }

    private void ReturnCassetteToPlayer(
        PlayerCassette playerCassette)
    {
        hasCassette = false;

        if (cassette != null)
        {
            cassette.SetActive(false);
        }
        playerCassette.GetCassetteBack();

        Debug.Log(
            gameObject.name +
            " lost the cassette to the player!"
        );
    }

    private void UpdateBossAnimation()
    {
        if (animator == null || rb == null)
            return;

        Vector2 velocity = rb.linearVelocity;

        int direction = 0;

        if (velocity.sqrMagnitude < 0.01f)
        {
            direction = 0;
        }
        else if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            direction = 3; 

            if (spriteRenderer != null)
            {
                if (velocity.x < 0f)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }
        }
        else
        {
            if (velocity.y > 0f)
            {
                direction = 1; 
            }
            else
            {
                direction = 2; 
            }
        }

        animator.SetInteger("BossMovement", direction);
    }
}

