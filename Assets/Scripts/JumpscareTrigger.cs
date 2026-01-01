using System.Collections;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    [Header("Jumpscare Objeleri")]
    [Tooltip("Maskeyi oluþturan tüm parçalarý buraya sürükle.")]
    public GameObject[] maskParts; // Maskenin 2 parçasýný buraya atacaðýz

    [Header("Ayarlar")]
    [Tooltip("Maske kaç saniye ekranda kalsýn? (Örn: 0.3 çok hýzlý, 1.0 yavaþ)")]
    public float duration = 0.5f; // Ekranda kalma süresi

    [Tooltip("Jumpscare anýnda çalacak ses (Opsiyonel).")]
    public AudioSource scareSound;

    private bool hasTriggered = false; // Sadece bir kere çalýþmasý için kontrol

    void Start()
    {
        // Oyun baþladýðýnda parçalarýn kapalý olduðundan emin olalým (Garanti olsun)
        SetPartsActive(false);
    }

    // Oyuncu trigger'a girdiðinde çalýþýr
    void OnTriggerEnter2D(Collider2D other)
    {
        // Çarpýþma olunca konsola ne çarptýðýný yazdýr
        Debug.Log("Bir þey çarptý: " + other.name);

        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("VE BU ÇARPAN ÞEY PLAYER!"); // Bu çýkýyorsa kod çalýþýyor demektir
            StartCoroutine(PlayJumpscare());
        }
    }

    // Zamanlamayý yöneten ana fonksiyon
    IEnumerator PlayJumpscare()
    {
        hasTriggered = true; // Bir daha çalýþmasýný engelle

        // 1. GÖSTER: Tüm parçalarý aç
        SetPartsActive(true);

        // 2. SES: Eðer ses atanmýþsa çal
        if (scareSound != null)
        {
            scareSound.Play();
        }

        // 3. BEKLE: Belirlenen süre kadar bekle
        yield return new WaitForSeconds(duration);

        // 4. GÝZLE: Tüm parçalarý kapat
        SetPartsActive(false);

        // Ýstersen bu noktada trigger objesini yok edebilirsin:
        // Destroy(gameObject); 
    }

    // Tüm parçalarý tek seferde açýp kapatmak için yardýmcý fonksiyon
    void SetPartsActive(bool isActive)
    {
        if (maskParts == null) return;

        foreach (GameObject part in maskParts)
        {
            if (part != null)
            {
                part.SetActive(isActive);
            }
        }
    }
}