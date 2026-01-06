using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float maxHealth = 1000f;
    public float moveSpeed = 3f;
    public float attackRange = 2.5f; // Saldırı mesafesi
    public float attackCooldown = 2.0f; // İki saldırı arası bekleme süresi

    [Header("Bağlantılar")]
    public Transform player; // Oyuncuyu buraya sürükle

    // --- Değişkenler ---
    private float currentHealth;
    private Animator anim;
    private int comboIndex = 1; // 1, 2, 3 takibi için
    
    private bool isPhase2 = false;
    private bool isDead = false;
    private bool isAttacking = false;   // Şu an saldırıyor mu?
    private bool inTransition = false;  // Faz geçişinde mi? (Bağırıyor mu?)

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Eğer öldüyse veya faz değiştiriyorsa (bağırıyorsa) hiçbir şey yapma
        if (isDead || inTransition) return;

        // Player yoksa hata vermesin diye kontrol
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // Menzildeyiz
            StopRunning();
            
            // Saldırmıyorsak saldırıyı başlat
            if (!isAttacking)
            {
                StartCoroutine(AttackRoutine());
            }
        }
        else
        {
            // Menzilde değiliz, oyuncuyu kovala
            if (!isAttacking) // Saldırı animasyonu oynarken kayarak gitmesin
            {
                MoveToPlayer();
            }
        }
    }

    void MoveToPlayer()
    {
        anim.SetBool("IsRunning", true);
        
        // Boss'un yüzünü oyuncuya döndür
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);

        // Hareket et
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void StopRunning()
    {
        anim.SetBool("IsRunning", false);
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 1. Hangi atakta olduğumuzu Animator'a bildir (1, 2 veya 3)
        anim.SetInteger("AttackIndex", comboIndex);
        
        // 2. Saldırı emrini ver (Tetiği çek)
        anim.SetTrigger("AttackTrigger");

        // 3. Sıradaki atak için index'i artır
        comboIndex++;
        if (comboIndex > 3) comboIndex = 1; // 3'ten sonra tekrar 1'e dönsün

        // 4. Animasyonun süresi + cooldown kadar bekle
        // Not: Burada sabit süre bekliyoruz, animasyon uzunluğuna göre ayarlayabilirsin.
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    // --- HASAR ALMA SİSTEMİ ---
    // Bu fonksiyonu Player'ın kılıcı Boss'a değdiğinde çağıracaksın.
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        // Debug.Log("Boss Canı: " + currentHealth);

        // Faz 2 Kontrolü (%50 can)
        if (!isPhase2 && currentHealth <= (maxHealth / 2))
        {
            StartCoroutine(StartPhase2());
        }

        // Ölüm Kontrolü
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator StartPhase2()
    {
        inTransition = true; // Boss'u dondur
        isPhase2 = true;
        isAttacking = false; // Saldırıyorsa iptal et

        // Animator'a Faz 2'ye geçtiğimizi söyle
        anim.SetBool("IsPhase2", true);
        
        // Bağırma animasyonunu tetikle
        anim.SetTrigger("Shout");

        // Bağırma animasyonu bitene kadar bekle (Örnek: 3 saniye)
        yield return new WaitForSeconds(3f);

        inTransition = false; // Boss tekrar harekete başlasın
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        
        // Boss'un collider'ını veya scriptini kapatabilirsin
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
}