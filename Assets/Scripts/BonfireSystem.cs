using UnityEngine;
using UnityEngine.Events; // Event sistemi için gerekli

public class BonfireSystem : MonoBehaviour
{
    [Header("UI Ayarlarý")]
    public GameObject tusGorseli; // "E" tuþu görseli (Canvas içindeki obje)

    [Header("Oyun Mantýðý")]
    public KeyCode etkilesimTusu = KeyCode.E;
    public bool etkilesimdeMi = false; // Oyuncu alanýn içinde mi?

    [Header("Olaylar (Events)")]
    // Buraya Inspector üzerinden baþka scriptleri baðlayabilirsin
    public UnityEvent onRest;

    void Start()
    {
        // Baþlangýçta tuþ görselini kapat
        if (tusGorseli != null)
            tusGorseli.SetActive(false);
    }

    void Update()
    {
        // Eðer oyuncu alanýn içindeyse VE "E" tuþuna bastýysa
        if (etkilesimdeMi && Input.GetKeyDown(etkilesimTusu))
        {
            BonfireYakal();
        }
    }

    void BonfireYakal()
    {
        Debug.Log("BONFIRE YAKILDI / DÝNLENÝLDÝ");

        // 1. Caný Doldurma (Örnek Kod)
        // PlayerHealth.instance.HealFull(); 

        // 2. Oyunu Kaydetme
        // SaveSystem.SaveGame();

        // 3. Düþmanlarý Yenileme (Souls mantýðý)
        // EnemyManager.RespawnAll();

        // Inspector'dan baðladýðýn eventleri çalýþtýr
        onRest.Invoke();
    }

    // --- ALAN KONTROLÜ (TRIGGER) ---

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            etkilesimdeMi = true;
            if (tusGorseli != null)
                tusGorseli.SetActive(true); // "E" tuþunu göster
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            etkilesimdeMi = false;
            if (tusGorseli != null)
                tusGorseli.SetActive(false); // "E" tuþunu gizle
        }
    }
}