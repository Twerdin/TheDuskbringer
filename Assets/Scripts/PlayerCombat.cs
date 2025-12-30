using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Saldýrý Ayarlarý")]
    public float attackOffset = 1.0f; // Karakterin ne kadar ÖNÜNE vursun?
    public float attackRange = 0.8f;  // Vuruþ dairesinin büyüklüðü
    public LayerMask enemyLayers;     // Düþman katmaný
    public int damageAmount = 20;     // Hasar miktarý

    [Header("Kombo Ayarlarý")]
    public float comboResetTime = 1.0f;
    private int comboStep = 0;
    private float comboTimer = 0f;

    [Header("Bileþenler")]
    private Animator anim;

    private float nextAttackTime = 0f;
    public float attackRate = 2f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Kombo Sayacý
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime) ResetCombo();
        }

        // Saldýrý Tuþu
        if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.J))
        {
            PerformComboAttack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void PerformComboAttack()
    {
        comboStep++;
        comboTimer = 0f;

        if (comboStep == 1) anim.SetTrigger("Attack1");
        else if (comboStep == 2) anim.SetTrigger("Attack2");
        else if (comboStep == 3) anim.SetTrigger("Attack3");

        DealDamage(); // Hasar Ver

        if (comboStep >= 3) comboStep = 0;
    }

    void DealDamage()
    {
        // --- MATEMATÝKSEL HESAPLAMA ---
        // 1. Karakterin yönünü bul (Scale X 1 ise sað, -1 ise sol)
        float direction = Mathf.Sign(transform.localScale.x);

        // 2. Vuruþ noktasýný hesapla (Karakterin merkezi + (Yön * Mesafe))
        Vector2 hitPos = new Vector2(
            transform.position.x + (direction * attackOffset),
            transform.position.y
        );

        // 3. O noktada düþman var mý diye bak
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitPos, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }

    void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0f;
    }

    // Editörde Kýrmýzý Daireyi Görmek Ýçin
    void OnDrawGizmosSelected()
    {
        // Oyun çalýþmýyorken bile yönü tahmin etmeye çalýþýr
        float direction = Mathf.Sign(transform.localScale.x);
        Vector2 hitPos = new Vector2(
            transform.position.x + (direction * attackOffset),
            transform.position.y
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPos, attackRange);
    }
}