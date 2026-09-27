using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        movement = Vector2.zero;

        if (Keyboard.current != null)
        {
            // Movement
            if (Keyboard.current.wKey.isPressed)
                movement.y += 1f;

            if (Keyboard.current.sKey.isPressed)
                movement.y -= 1f;

            if (Keyboard.current.aKey.isPressed)
                movement.x -= 1f;

            if (Keyboard.current.dKey.isPressed)
                movement.x += 1f;


            // Animation direction
            int direction = 0;

            if (Keyboard.current.wKey.isPressed)
            {
                direction = 1;
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                direction = 2;
            }
            else if (Keyboard.current.aKey.isPressed ||
                    Keyboard.current.dKey.isPressed)
            {
                direction = 3;
            }

            if (animator != null)
            {
                animator.SetInteger(
                    "MoveDirection",
                    direction
                );
            }


            // Flip sprite left/right
            if (spriteRenderer != null)
            {
                if (Keyboard.current.dKey.isPressed)
                {
                    spriteRenderer.flipX = true;
                }
                else if (Keyboard.current.aKey.isPressed)
                {
                    spriteRenderer.flipX = false;
                }
            }
        }

        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }
}
