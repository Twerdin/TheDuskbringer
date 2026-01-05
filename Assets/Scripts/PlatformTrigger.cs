using UnityEngine;
using System.Collections.Generic;

public class PlatformTrigger : MonoBehaviour
{
    [Header("BU Tetikleyici Hangilerini Düþürecek?")]
    // Buraya sürüklediðin platformlar, SADECE bu kutuya deðince düþer.
    public List<TimedPlatform> myPlatforms;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Oyuncu çarparsa ve daha önce çalýþmadýysa
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            Debug.Log(gameObject.name + " Tetiklendi!");

            // Sadece bu listedekilere "Düþ" emri ver
            foreach (TimedPlatform p in myPlatforms)
            {
                if (p != null) p.StartCountdown();
            }

            // Ýstersen tetikleyiciyi yok et
            Destroy(gameObject);
        }
    }
}