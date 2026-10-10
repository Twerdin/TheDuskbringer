using UnityEngine;

public class MusicSwitcher2 : MonoBehaviour
{
    [Header("Ayarlar")]
    public AudioSource audioSource; // Müzik kutusu (Audio Source)

    [Header("Müzikler")]
    public AudioClip startMusic;   // Baþlangýçta çalacak þarký
    public AudioClip triggerMusic; // Olay olunca (Trigger) çalacak þarký

    private bool hasSwitched = false; // Müzik deðiþti mi kontrolü

    void Start()
    {
        // Eðer bir Audio Source sürüklemediysen, bu objedekini otomatik bul
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Baþlangýç müziðini çal
        if (startMusic != null)
        {
            audioSource.clip = startMusic;
            audioSource.Play();
        }
    }

    // Oyuncu Trigger alanýna girince çalýþýr
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sadece "Player" etiketli obje girerse ve müzik henüz deðiþmediyse
        if (collision.CompareTag("Player") && !hasSwitched)
        {
            SwitchMusic();
        }
    }

    void SwitchMusic()
    {
        if (triggerMusic != null)
        {
            // Yeni þarkýyý yükle ve çal
            audioSource.Stop(); // Öncekini durdur
            audioSource.clip = triggerMusic; // Yenisini tak
            audioSource.Play(); // Çalmaya baþla

            hasSwitched = true; // Tekrar tekrar deðiþmemesi için kilitle
            Debug.Log("Müzik Deðiþti: Trigger Müziði Çalýyor!");
        }
    }
}