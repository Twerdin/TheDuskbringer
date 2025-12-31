using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Bileþenler")]
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private Transform playerTarget;

    [Header("Hareket Ayarlarý")]
    public float walkSpeed = 1.5f;  // Yürüme hýzý (Yavaþ)
    public float runSpeed = 4f;     // Koþma hýzý (Hýzlý)

    [Header("Menzil & Yapay Zeka")]
    public float detectionRange = 6f; // Oyuncuyu ne kadar uzaktan fark eder?
    public float stopDistance = 1.2f; // Saldýrmak için ne kadar yaklaþsýn?

    [Header("Devriye (Patrol) Ayarlarý")]
    public float patrolDuration = 2f; // Kaç saniye yürüsün?
    public float idleDuration = 2f;   // Kaç saniye beklesin?
    private float patrolTimer;
    private float idleTimer;
    private bool isMovingRight = true; // Þu an ne tarafa gidiyor?

    [Header("Saldýrý")]
    public float attackRate = 1f;
    public int damageAmount = 20;
    private float nextAttackTime = 0f;

    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Zamanlayýcýlarý sýfýrla
        patrolTimer = patrolDuration;
        idleTimer = idleDuration;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (isDead) return;

        // Oyuncu ile aradaki mesafeyi ölç
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        // --- DURUM MAKÝNESÝ (STATE MACHINE) ---

        if (distanceToPlayer < detectionRange)
        {
            // DURUM 1: OYUNCUYU GÖRDÜ (KOVALAMA & SALDIRI)
            EngagePlayer(distanceToPlayer);
        }
        else
        {
            // DURUM 2: OYUNCU UZAKTA (DEVRÝYE GEZ)
            PatrolRoutine();
        }
    }

    // --- DEVRÝYE MODU (Sakin Yürüme) ---
    void PatrolRoutine()
    {
        // Önce koþmayý kapat
        anim.SetBool("isRunning", false);
        anim.SetBool("isRunning", false); // Büyük/küçük harf hatasý ihtimaline karþý ikisi de

        // Bekleme Süresi (Idle)
        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            StopMovement();
            anim.SetBool("isWalking", false);
            return; // Bekliyorsa aþaðýya inme
        }

        // Yürüme Süresi
        if (patrolTimer > 0)
        {
            patrolTimer -= Time.deltaTime;
            MovePatrol();
            anim.SetBool("isWalking", true);
        }
        else
        {
            // Yürüme bitti, tekrar bekleme moduna geç ve yön deðiþtir
            idleTimer = idleDuration;
            patrolTimer = patrolDuration;
            FlipDirection(); // Yönü tersine çevir
        }
    }

    void MovePatrol()
    {
        float dir = isMovingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(dir * walkSpeed, rb.linearVelocity.y);

        // Yüzünü gittiði yöne dön
        transform.localScale = new Vector3(dir, 1, 1);
    }

    void FlipDirection()
    {
        isMovingRight = !isMovingRight;
    }

    // --- SAVAÞ MODU (Koþma & Saldýrý) ---
    void EngagePlayer(float distance)
    {
        // Devriye modunu kapat
        anim.SetBool("isWalking", false);

        if (distance > stopDistance)
        {
            // Oyuncuya doðru KOÞ
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * runSpeed, rb.linearVelocity.y);

            // Yüzünü oyuncuya dön
            if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < 0) transform.localScale = new Vector3(-1, 1, 1);

            anim.SetBool("isRunning", true);
        }
        else
        {
            // Çok yakýn, DUR ve SALDIR
            StopMovement();
            anim.SetBool("isRunning", false);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void StopMovement()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        if (playerHealth != null) playerHealth.TakeDamage(damageAmount);
    }

    public void Die()
    {
        isDead = true;
        anim.SetBool("IsDead", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalking", false);
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        this.enabled = false;
    }

    public void TakeDamage()
    {
        if (isDead) return;
        anim.SetTrigger("Hurt");
    }
}