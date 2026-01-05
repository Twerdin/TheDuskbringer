using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;
    private Rigidbody2D rb;

    void Start()
    {
        spawnPoint = transform.position; // oyunun başladığı yer
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        // Ölüm anı
        rb.velocity = Vector2.zero;
        rb.simulated = false; // fizik durur

        // (istersen animasyon/ses burada)

        yield return new WaitForSeconds(0.1f);

        // Respawn
        transform.position = spawnPoint;
        rb.simulated = true;
    }
}
