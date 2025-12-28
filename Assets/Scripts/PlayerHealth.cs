using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering; // DÝKKAT: Kütüphane deðiþti! (Eskisi: .PostProcessing idi)

public class PlayerHealth : MonoBehaviour
{
    [Header("Can Deðerleri")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Baðlantýlarý")]
    public Image healthBarImage;
    public GameObject gameOverPanel;

    [Header("Efekt Ayarlarý")]
    public Volume deathVolume; // DÝKKAT: Türü 'PostProcessVolume' deðil artýk 'Volume' oldu
    public float effectSpeed = 0.5f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        isDead = false;

        UpdateHealthBar();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Volume bileþeni weight mantýðýný ayný þekilde kullanýr
        if (deathVolume != null)
            deathVolume.weight = 0;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Karakter Öldü");

        // Hareket scriptlerini durdurma (Senin kodundaki gibi)
        if (GetComponent<PlayerMovement>() != null) GetComponent<PlayerMovement>().enabled = false;
        if (GetComponent<Rigidbody2D>() != null) GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        StartCoroutine(ShowGameOverEffects());
    }

    System.Collections.IEnumerator ShowGameOverEffects()
    {
        float counter = 0f;

        while (counter < 1f)
        {
            counter += Time.deltaTime * effectSpeed;

            if (deathVolume != null)
            {
                deathVolume.weight = counter; // Kodun mantýðý ayný kalýr
            }

            yield return null;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }
}