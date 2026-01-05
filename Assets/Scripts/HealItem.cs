using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 20; // Kaç can vereceði
    public float popForce = 5f; // Havaya fýrlama gücü

    void Start()
    {
        // Oyun baþlayýnca (sandýktan çýkýnca) yukarý doðru zýpla
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * popForce, ForceMode2D.Impulse);

        // Rastgele biraz saða sola da gitsin ki doðal dursun
        float randomX = Random.Range(-1f, 1f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(randomX, 0) * 1f, ForceMode2D.Impulse);
    }

    // Oyuncu çarpýnca çalýþýr
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount); // Can ver
                Destroy(gameObject); // Objeyi yok et
            }
        }
    }
}