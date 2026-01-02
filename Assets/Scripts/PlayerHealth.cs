using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering; // URP Volume sistemi için gerekli kütüphane

public class PlayerHealth : MonoBehaviour
{
    [Header("Can Deðerleri")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Baðlantýlarý")]
    public Image healthBarImage;
    public GameObject gameOverPanel;

    [Header("Efekt Ayarlarý")]
    public Volume deathVolume; // URP'de PostProcessVolume yerine Volume kullanýlýr
    public float effectSpeed = 0.5f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        isDead = false;

        UpdateHealthBar();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Oyun baþladýðýnda efektin görünmemesi için aðýrlýðý sýfýrlýyoruz
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
        // Debug.Log("Karakter Öldü");

        // Karakterin hareketini durdurma
        if (GetComponent<PlayerMovement>() != null) GetComponent<PlayerMovement>().enabled = false;

        // Fiziksel etkileþimleri kapatmak için (isteðe baðlý)
        if (GetComponent<Rigidbody2D>() != null) GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        // Efekti ve Game Over ekranýný baþlat
        StartCoroutine(ShowGameOverEffects());
    }

    System.Collections.IEnumerator ShowGameOverEffects()
    {
        float counter = 0f;

        // Efektin yavaþça gelmesini saðlar (Volume Weight 0'dan 1'e çýkar)
        while (counter < 1f)
        {
            counter += Time.deltaTime * effectSpeed;

            if (deathVolume != null)
            {
                deathVolume.weight = counter;
            }

            yield return null;
        }

        // Efekt tamamlanýnca Game Over panelini aç
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