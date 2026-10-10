using UnityEngine;

public class SFXScript : MonoBehaviour
{
    [Header("Ayarlar")]
    public AudioSource ayakSesKaynagi;  // Alt obje (AyakSesi) buraya
    public AudioSource saldiriKaynagi;  // Player'ýn kendisi buraya
    public AudioClip slashSesi;         // Slash.mp3 buraya

    void Update()
    {
        // --- SÝGORTA KONTROLÜ ---
        // Eðer ayak sesi kaynaðýný baðlamadýysan kodun geri kalaný çalýþmaz (Hata vermez)
        if (ayakSesKaynagi == null)
        {
            return; // Buradan geri dön, aþaðý inme
        }

        // --- 1. YÜRÜME KISMI (Volume Mantýðý) ---

        // A, D veya Ok tuþlarýna basýlýyor mu?
        bool hareketVar = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
                          Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

        if (hareketVar)
        {
            // Hareket varsa sesi aç
            ayakSesKaynagi.volume = 1f;

            // Garanti olsun diye, çalmýyorsa baþlat
            if (!ayakSesKaynagi.isPlaying)
            {
                ayakSesKaynagi.Play();
            }
        }
        else
        {
            // Hareket yoksa sesi KIS
            ayakSesKaynagi.volume = 0f;
        }

        // --- 2. SALDIRI KISMI (J Tuþu) ---

        if (Input.GetKeyDown(KeyCode.J))
        {
            // Saldýrý kaynaðý ve ses dosyasý var mý kontrolü
            if (saldiriKaynagi != null && slashSesi != null)
            {
                saldiriKaynagi.PlayOneShot(slashSesi);
            }
        }
    }
}