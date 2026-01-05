using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    public float destroyDelay = 2f;
    public float respawnDelay = 2f;

    private Vector3 startPosition;
    private Collider2D col;
    private SpriteRenderer sr;
    private bool isTriggered = false;

    void Start()
    {
        startPosition = transform.position;
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(DestroyAndRespawn());
        }
    }

    IEnumerator DestroyAndRespawn()
    {
        yield return new WaitForSeconds(destroyDelay);

        // Platformu kapat
        col.enabled = false;
        sr.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // Platformu geri getir
        transform.position = startPosition;
        col.enabled = true;
        sr.enabled = true;

        isTriggered = false;
    }
}
