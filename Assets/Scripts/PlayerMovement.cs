using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private Animator anim;

    // SpriteRenderer'a artýk yön çevirmek için ihtiyacýmýz yok, o yüzden sildim.

    [Header("Durumlar")]
    public bool isGrounded = false;
    private bool isJumping = false;
    private SpriteRenderer sr; // Bunu ekle

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>(); // Bunu da ekle
    }
    void Update()
    {
        Move();
        Jump();

        // Animasyon parametrelerini güncelle
        anim.SetBool("IsGrounded", isGrounded);

        anim.SetBool("isJumping", isJumping);

    }

    void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");

        // Unity 6: linearVelocity, Eski: velocity
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // --- EN BAÞTAKÝ HALÝ (Sadece Resim Döner) ---
        if (move > 0)
        {
            sr.flipX = false; // Saða bak (Resmi düzelt)
        }
        else if (move < 0)
        {
            sr.flipX = true; // Sola bak (Resmi ters çevir)
        }

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