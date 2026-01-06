using UnityEngine;

public class DemonHealth : MonoBehaviour
{
    [Header("Düþman Özellikleri")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Faz 2 Ayarlarý (YENÝ)")]
    public bool isPhase2 = false; // Faz 2'ye geçti mi?

    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // --- FAZ 2 KONTROLÜ ---
        // Eðer can yarýdan azsa VE henüz Faz 2'ye geçmediysek
        if (currentHealth <= (maxHealth / 2) && !isPhase2)
        {
            EnterPhase2();
        }
        else if (!isPhase2) // Sadece Faz 1'deysek normal hasar animasyonu oynat
        {
            // Faz 2'ye geçerken Hurt animasyonu Shout'u bölmesin diye else içine aldýk
            if (anim != null) anim.SetTrigger("Hurt");
        }

        // --- ÖLÜM KONTROLÜ ---
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void EnterPhase2()
    {
        isPhase2 = true; // Bir daha bu fona girmesin diye iþaretledik
        Debug.Log("BOSS ÇILDIRDI! (Phase 2)");

        if (anim != null)
        {
            // 1. Shout animasyonunu tetikle (Her þeyi böler)
            anim.SetTrigger("ShoutTrig");

            // 2. Animator'a artýk öfkeli modda olduðumuzu bildir
            // Böylece kodun geri kalaný Flame animasyonlarýný kullanacak
            anim.SetBool("isEnraged", true);
        }
    }

    void Die()
    {
        if (anim != null) anim.SetBool("IsDead", true);

        // Diðer ölüm kodlarýn (Efekt, Destroy vs.)
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}