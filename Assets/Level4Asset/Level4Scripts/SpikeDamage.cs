using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public int damageAmount = 1000;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the collision is with the top of the spike
            if (collision.contacts[0].normal.y < -0.5)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
            }
        }
    }
}
