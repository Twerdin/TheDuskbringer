using UnityEngine;

public class DemonController : MonoBehaviour
{
    [Header("Bileþenler")]
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private Transform playerTarget;

    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Faz 2 (Flame/Öfke) Ayarlarý")]
    public bool isPhase2 = false; // Faz 2'ye geçti mi?
    private bool isShouting = false; // Þu an baðýrýyor mu? (Hareket etmesin diye)

    [Header("Hareket Ayarlarý")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 4f;

    [Header("Görsel Ayarlar")]
    public bool spriteFaceLeft = false;

    [Header("Menzil & Yapay Zeka")]
    public float detectionRange = 6f;
    public float stopDistance = 1.2f;

    [Header("Devriye (Patrol) Ayarlarý")]
    public float patrolDuration = 2f;
    public float idleDuration = 2f;
    private float patrolTimer;
    private float idleTimer;
    private bool isMovingRight = true;

    [Header("Saldýrý")]
    public float attackRate = 1f;
    public int damageAmount = 20;
    private float nextAttackTime = 0f;

    // --- ANIMASYON ÝSÝMLERÝ (Senin Animator Tablona Göre Ayarlandý) ---
    [Header("Animasyon Parametre Ýsimleri")]
    public string walkAnimName = "isWalking";
    public string runAnimName = "isRunning";
    public string attackTriggerName = "AttackTrigger";
    public string attackIntName = "AttackInt";
    public string hurtTriggerName = "Hurt";
    public string deadBoolName = "IsDead";

    // YENÝ EKLENENLER (FLAME MODU ÝÇÝN)
    public string shoutTriggerName = "ShoutTrig";
    public string enragedBoolName = "isEnraged";

    [Header("Ölüm Ayarlarý")]
    public float deathYOffset = -0.15f;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
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
        // Öldüyse veya þu an "Shout" (Baðýrma) animasyonundaysa hareket etmesin
        if (isDead || isShouting)
        {
            StopMovement();
            return;
        }

        if (playerTarget == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer < detectionRange)
        {
            EngagePlayer(distanceToPlayer);
        }
        else
        {
            PatrolRoutine();
        }
    }

    // --- DEVRÝYE MODU ---
    void PatrolRoutine()
    {
        anim.SetBool(runAnimName, false);

        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            StopMovement();
            anim.SetBool(walkAnimName, false);
            return;
        }

        if (patrolTimer > 0)
        {
            patrolTimer -= Time.deltaTime;
            MovePatrol();
            anim.SetBool(walkAnimName, true);
        }
        else
        {
            idleTimer = idleDuration;
            patrolTimer = patrolDuration;
            FlipDirection();
        }
    }

    void MovePatrol()
    {
        float dir = isMovingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(dir * walkSpeed, rb.linearVelocity.y);

        float scaleMultiplier = spriteFaceLeft ? -1 : 1;
        transform.localScale = new Vector3(dir * scaleMultiplier, 1, 1);
    }

    void FlipDirection()
    {
        isMovingRight = !isMovingRight;
    }

    // --- SAVAÞ MODU ---
    void EngagePlayer(float distance)
    {
        anim.SetBool(walkAnimName, false);

        if (distance > stopDistance)
        {
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * runSpeed, rb.linearVelocity.y);

            float scaleMultiplier = spriteFaceLeft ? -1 : 1;
            if (direction.x > 0)
                transform.localScale = new Vector3(1 * scaleMultiplier, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1 * scaleMultiplier, 1, 1);

            anim.SetBool(runAnimName, true);
        }
        else
        {
            StopMovement();
            anim.SetBool(runAnimName, false);

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

    // --- SALDIRI SÝSTEMÝ (GÜNCELLENDÝ) ---
    void Attack()
    {
        if (isPhase2)
        {
            // --- FAZ 2: FLAME MODU ---
            // 1, 2 veya 3 numaralý saldýrýlardan birini rastgele seç
            int randomAttack = Random.Range(1, 4);
            anim.SetInteger(attackIntName, randomAttack);
        }
        else
        {
            // --- FAZ 1: NORMAL MOD ---
            // Normal saldýrý için 0 gönderiyoruz (Senin Animator'da 0 normal ise)
            anim.SetInteger(attackIntName, 0);
        }

        // Tetiði çek ve saldýrýyý baþlat
        anim.SetTrigger(attackTriggerName);

        // Oyuncuya hasar ver (Animasyon event kullanmýyorsan anýnda hasar verir)
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    // --- HASAR ALMA VE FAZ GEÇÝÞÝ ---
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        // --- FAZ 2 KONTROLÜ ---
        // Eðer can yarýdan azsa VE henüz Faz 2'ye girmediysek
        if (currentHealth <= (maxHealth / 2) && !isPhase2)
        {
            EnterPhase2();
        }
        else
        {
            // Eðer Faz deðiþimi olmuyorsa normal "Hurt" animasyonu oynat
            // (Faz deðiþtirirken Hurt oynatýrsak Shout animasyonu bozulabilir)
            if (!isShouting)
            {
                anim.SetTrigger(hurtTriggerName);
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- BOSS ÇILDIRMA FONKSÝYONU ---
    void EnterPhase2()
    {
        isPhase2 = true;
        isShouting = true; // Hareket etmeyi durdur
        StopMovement();

        Debug.Log("BOSS ÇILDIRDI! (Flame Mode)");

        // 1. Shout (Baðýrma) Trigger'ýný tetikle
        anim.SetTrigger(shoutTriggerName);

        // 2. Animator'a artýk öfkeli (Flame) modda olduðumuzu bildir
        anim.SetBool(enragedBoolName, true);

        // 3. Baðýrma süresi kadar (örn: 1.5 saniye) kilitli kalmasýný saðla
        // Bu süreyi animasyonunun uzunluðuna göre deðiþtirebilirsin
        Invoke("FinishShout", 1.5f);
    }

    void FinishShout()
    {
        isShouting = false; // Artýk hareket edebilir ve saldýrabilir
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        anim.SetBool(deadBoolName, true);
        anim.SetBool(runAnimName, false);
        anim.SetBool(walkAnimName, false);

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + deathYOffset,
            transform.position.z
        );

        Destroy(gameObject, 2.5f);
        this.enabled = false;
    }
}