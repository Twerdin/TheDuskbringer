using UnityEngine;

public class ZeminKontrol : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform ayakNoktasi; // Ayaklarýn tam altýna koyduðun boþ obje
    public Vector2 kutuBoyutu = new Vector2(0.8f, 0.2f); // Kutunun geniþliði ve yüksekliði
    public LayerMask zeminKatmani; // Hangi objeleri "Zemin" sayacaðýz?

    bool YerdeMiyim()
    {
        // Physics2D.OverlapBox bize çarptýðý objeyi verir (Collider2D).
        // Eðer hiçbir þeye çarpmazsa "null" (boþ) döner.

        Collider2D carpanSey = Physics2D.OverlapBox(ayakNoktasi.position, kutuBoyutu, 0, zeminKatmani);

        // Eðer 'carpanSey' null deðilse, bir þeye çarptýk demektir.
        if (carpanSey != null)
        {
            return true; // Evet, yerdeyiz
        }
        else
        {
            return false; // Hayýr, havadayýz
        }
    }

    // TEST ÝÇÝN: Görünmez kutuyu sahnede çizelim ki ne yaptýðýmýzý görelim
    void OnDrawGizmos()
    {
        if (ayakNoktasi != null)
        {
            Gizmos.color = Color.red; // Kutunun rengi kýrmýzý olsun
            // DrawWireCube = Ýçi boþ tel kafes þeklinde kutu çiz
            Gizmos.DrawWireCube(ayakNoktasi.position, kutuBoyutu);
        }
    }

    void Update()
    {
        if (YerdeMiyim())
        {
            Debug.Log("ZEMÝNE BASIYORUM!");
        }
        else
        {
            Debug.Log("HAVADAYIM...");
        }
    }
}