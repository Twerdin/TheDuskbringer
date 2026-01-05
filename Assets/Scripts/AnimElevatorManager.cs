using UnityEngine;
using System.Collections.Generic;

public class AnimElevatorManager : MonoBehaviour
{
    [Header("Ölmesi Gereken Düþmanlar")]
    public List<GameObject> enemiesToWatch; // Buraya düþmanlarý sürükle

    private Animator anim;
    private bool elevatorActive = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Eðer asansör zaten çalýþýyorsa veya düþman listesi boþsa bir þey yapma
        if (elevatorActive || enemiesToWatch.Count == 0) return;

        // Düþman kontrolü
        if (AreEnemiesDead())
        {
            ActivateElevator();
        }
    }

    void ActivateElevator()
    {
        elevatorActive = true;
        Debug.Log("Düþmanlar temizlendi! Animasyon baþlýyor.");

        // Animator'daki 'Calis' parametresini açýyoruz
        if (anim != null)
        {
            anim.SetBool("Calis", true);
        }
    }

    bool AreEnemiesDead()
    {
        foreach (GameObject enemy in enemiesToWatch)
        {
            // Eðer düþman hala sahnedeyse (ölmediyse)
            if (enemy != null && enemy.activeInHierarchy)
            {
                return false; // Biri bile yaþýyorsa bekle
            }
        }
        return true; // Hepsi yok olduysa tamamdýr
    }

    // --- OYUNCUYU ASANSÖRE YAPIÞTIRMA (ÖNEMLÝ) ---
    // Animasyonla gitse bile oyuncunun kaymamasý için bu þart
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}