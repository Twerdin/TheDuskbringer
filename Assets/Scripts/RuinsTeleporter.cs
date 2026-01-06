using UnityEngine;

public class IsinlanmaKapisi : MonoBehaviour
{
    [Header("Nereye Gidecek?")]
    public Transform hedefNokta; // Unity içinden buraya varýþ noktasýný sürükleyeceðiz.

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kapýya çarpan þeyin etiketi "Player" mý? (Düþmanlar ýþýnlanmasýn diye)
        if (other.CompareTag("Player"))
        {
            // Oyuncunun pozisyonunu hedef noktanýn pozisyonu yap
            other.transform.position = hedefNokta.position;
        }
    }
}