using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    // --- KRÝTÝK DEÐÝÞÝKLÝK BURADA ---
    // Bunu "public" yaptýk ki PlayerCombat scripti "Havadayýz!" diye görebilsin.
    public bool isGrounded = false;
    // --------------------------------

    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();

        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("isJumping", isJumping);
    }

    void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");
        // Unity 6 kullanýyorsan linearVelocity, eskiyse velocity kullan.
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (move > 0) sr.flipX = false;
        if (move < 0) sr.flipX = true;

        anim.SetBool("isRunning", move != 0);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            isJumping = true;
            isGrounded = false;
        }

        if (isGrounded)
        {
            isJumping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}