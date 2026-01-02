using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 20f; // Ne kadar hasar vereceði

    // Eðer düþman/tuzak 'Is Trigger' deðilse (Fiziksel çarpýþma)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarptýðýmýz objenin tag'i Player mý?
        if (collision.gameObject.CompareTag("Player"))
        {
            // Oyuncunun üzerindeki PlayerHealth scriptini bul
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // Eðer script varsa hasar ver
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    // Eðer düþman/tuzak 'Is Trigger' ise (Ýçinden geçilen hayalet obje)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}