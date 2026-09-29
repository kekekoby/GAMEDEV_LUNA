using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [Header("Rotation")]
    public Vector3 rotationSpeed = Vector3.zero;

    [Header("Movement")]
    public Vector3 moveAxis = Vector3.zero;
    public float moveDistance = 3f;
    public float moveSpeed = 1f;

    Rigidbody rb;
    Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        if (rotationSpeed != Vector3.zero)
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationSpeed * Time.fixedDeltaTime));

        if (moveAxis != Vector3.zero)
        {
            float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            rb.MovePosition(startPos + moveAxis.normalized * offset);
        }
    }
}