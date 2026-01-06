using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [Header("Müzik Ayarları")]
    public AudioSource mainAudioSource; // Çalan ana müzik kaynağı
    public AudioClip bossMusic;         // Çalınacak Boss müziği
    
    private AudioClip originalMusic;    // Eski müziği hafızada tutmak için

    private void Start()
    {
        // Oyun başladığında çalan müziği hafızaya alalım
        if (mainAudioSource != null)
        {
            originalMusic = mainAudioSource.clip;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Alana giren objenin etiketi "Player" ise
        if (other.CompareTag("Player"))
        {
            // Eğer şu an zaten boss müziği çalmıyorsa değiştir
            if (mainAudioSource.clip != bossMusic)
            {
                mainAudioSource.Stop(); // Mevcut müziği durdur
                mainAudioSource.clip = bossMusic; // Klibi Boss müziği yap
                mainAudioSource.Play(); // Oynat
            }
        }
    }

    // İSTEĞE BAĞLI: Alandan çıkınca eski müzik geri gelsin istersen bu kısmı açabilirsin
    /*
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mainAudioSource.Stop();
            mainAudioSource.clip = originalMusic;
            mainAudioSource.Play();
        }
    }
    */
}
