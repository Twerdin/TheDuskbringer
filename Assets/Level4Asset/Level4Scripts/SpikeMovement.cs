using System.Collections;
using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    public float height = 2f;
    public float speed = 1f;
    public float waitTime = 1f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingUp = true;

    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(startPos.x, startPos.y + height, startPos.z);
        StartCoroutine(MoveSpike());
    }

    IEnumerator MoveSpike()
    {
        while (true)
        {
            Vector3 targetPos = movingUp ? endPos : startPos;
            float step = speed * Time.deltaTime;

            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
                yield return null;
            }

            transform.position = targetPos;
            yield return new WaitForSeconds(waitTime);
            movingUp = !movingUp;
        }
    }
}