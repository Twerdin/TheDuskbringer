using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 100f; 

    void Update()
    {
        
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
