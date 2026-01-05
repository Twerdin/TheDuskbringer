using UnityEngine;
using Unity.Cinemachine;

public class CameraLook : MonoBehaviour
{
    public CinemachineCamera vcam;

    public float lookOffsetY = 3f;
    public float lookSpeed = 6f;

    private CinemachinePositionComposer composer;
    private float currentOffsetY;

    void Start()
    {
        composer = vcam.GetComponent<CinemachinePositionComposer>();
        currentOffsetY = 0f;
    }

    void Update()
    {
        float targetOffsetY = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
            targetOffsetY = lookOffsetY;
        else if (Input.GetKey(KeyCode.DownArrow))
            targetOffsetY = -lookOffsetY;

        currentOffsetY = Mathf.MoveTowards(
            currentOffsetY,
            targetOffsetY,
            lookSpeed * Time.deltaTime
        );

        composer.TargetOffset = new Vector3(
            composer.TargetOffset.x,
            currentOffsetY,
            composer.TargetOffset.z
        );
    }
}
