using UnityEngine;
using UnityEngine.Events;

public class BonfireSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    public KeyCode interactKey = KeyCode.E;
    private bool isInRange = false; // Oyuncu yakýn mý?

    [Header("Görsel Efektler")]
    public GameObject uiText; // "E'ye Bas" yazýsý
    // public GameObject fireParticle; // Ýstersen ateþ efekti ekle

    [Header("Olaylar")]
    public UnityEvent OnRest; // Inspector'daki event kutusu

    void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            RestAtBonfire();
        }
    }

    void RestAtBonfire()
    {
        Debug.Log("Bonfire Yakýldý! Oyun Kaydedildi.");

        // --- KAYIT SÝSTEMÝ (EN ÖNEMLÝ KISIM) ---
        // Bonfire'ýn konumunu kaydet
        PlayerPrefs.SetFloat("RespawnX", transform.position.x);
        PlayerPrefs.SetFloat("RespawnY", transform.position.y);

        // Hangi sahnede olduðumuzu kaydet (Geri dönmek için)
        PlayerPrefs.SetString("SavedLevel", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Kayýt olduðunu bildiren bayraðý kaldýr (Artýk kayýtlý bir yerimiz var)
        PlayerPrefs.SetInt("HasSave", 1);

        PlayerPrefs.Save(); // Veriyi diske yaz
        // ----------------------------------------

        // Inspector'da baðladýðýn diðer olaylarý tetikle (Can doldurma vs.)
        OnRest.Invoke();
    }

    // Oyuncu alana girdi
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            if (uiText != null) uiText.SetActive(true);
        }
    }

    // Oyuncu alandan çýktý
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            if (uiText != null) uiText.SetActive(false);
        }
    }
}