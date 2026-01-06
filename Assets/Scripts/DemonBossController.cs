using UnityEngine;

public class DemonBossController : MonoBehaviour
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
    private bool isShouting = false; // Þu an baðýrýyor mu?

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

    [Header("Saldýrý Ayarlarý (Sýralý 1-2-3)")]
    public float attackRate = 1f;
    public int damageAmount = 20;
    private float nextAttackTime = 0f;

    [Tooltip("Kaç farklý saldýrý animasyonu var? (3 yazarsan 1-2-3-1-2-3 yapar)")]
    public int maxComboCount = 3;
    private int comboStep = 0; // Þu an kaçýncý saldýrýdayýz?

    // --- ANIMASYON ÝSÝMLERÝ ---
    [Header("Animasyon Ýsimleri")]
    public string walkAnimName = "isWalking";
    public string runAnimName = "isRunning";
    public string attackTriggerName = "AttackTrigger";
    public string attackIntName = "AttackInt";
    public string hurtTriggerName = "Hurt";
    public string deadBoolName = "IsDead";

    // YENÝ EKLENENLER (FLAME MODU)
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

        // Baþlangýçta kombo 0 olsun (Ýlk vuruþ 1 olacak)
        comboStep = 0;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        // Öldüyse veya Baðýrýyorsa kýmýldamasýn
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

    // --- SALDIRI SÝSTEMÝ (ÝSTEDÝÐÝN SIRALI MANTIK) ---
    void Attack()
    {
        // Saldýrý adýmýný artýr (0 -> 1, 1 -> 2, 2 -> 3)
        comboStep++;

        // Eðer 3'ü geçerse tekrar 1'e dön
        if (comboStep > maxComboCount)
        {
            comboStep = 1;
        }

        // Animator'a sayýyý gönder (Hem Normal hem Flame mod bu sayýyý kullanýr)
        // Animator içinde: 
        // Normal moddaysan -> Attack1, Attack2, Attack3 oynar.
        // Enraged (Flame) moddaysan -> FlameAttack1, FlameAttack2, FlameAttack3 oynar.
        anim.SetInteger(attackIntName, comboStep);

        // Tetiði çek
        anim.SetTrigger(attackTriggerName);

        // Hasar ver
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

        // --- FAZ 2 KONTROLÜ (%50 Can) ---
        if (currentHealth <= (maxHealth / 2) && !isPhase2)
        {
            EnterPhase2();
        }
        else
        {
            // Eðer baðýrmýyorsa normal hasar animasyonu
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

    // --- BOSS ÇILDIRMA (PHASE 2) ---
    void EnterPhase2()
    {
        isPhase2 = true;
        isShouting = true; // Hareket kilidi
        StopMovement();

        Debug.Log("BOSS ÖFKELENDÝ! (Sýralama sýfýrlandý, 1'den baþlayacak)");

        // Faz deðiþtiðinde komboyu sýfýrla ki alevli ilk saldýrý kesinlikle "1" olsun
        comboStep = 0;

        // 1. Shout Trigger
        anim.SetTrigger(shoutTriggerName);

        // 2. Enraged Bool (Artýk alevli animasyonlar devreye girer)
        anim.SetBool(enragedBoolName, true);

        // 3. Baðýrma süresi bitince kilidi aç (1.5 sn)
        Invoke("FinishShout", 1.5f);
    }

    void FinishShout()
    {
        isShouting = false;
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