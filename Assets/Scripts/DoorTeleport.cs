using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform destinationPoint; // Oyuncunun gideceði hedef nokta (Boþ Obje)
    public KeyCode interactKey = KeyCode.E; // Hangi tuþla girilecek?

    private bool isPlayerAtDoor = false; // Oyuncu kapýnýn aðzýnda mý?
    private GameObject player; // Oyuncuyu hafýzada tutmak için

    void Update()
    {
        // Eðer oyuncu kapýdaysa VE belirlediðimiz tuþa (E) bastýysa
        if (isPlayerAtDoor && Input.GetKeyDown(interactKey))
        {
            Teleport();
        }
    }

    void Teleport()
    {
        if (destinationPoint != null && player != null)
        {
            // Oyuncunun pozisyonunu hedef noktanýn pozisyonuna eþitle
            player.transform.position = destinationPoint.position;
            Debug.Log("Iþýnlanma Baþarýlý!");
        }
        else
        {
            Debug.LogWarning("Hedef nokta veya Oyuncu bulunamadý!");
        }
    }

    // Oyuncu kapýnýn alanýna girdiðinde
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Çarpan þeyin etiketi "Player" mý?
        {
            isPlayerAtDoor = true;
            player = other.gameObject;
        }
    }

    // Oyuncu kapýnýn alanýndan çýktýðýnda
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerAtDoor = false;
            player = null;
        }
    }
}