using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    public Transform spawnPoint;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fallzone")) return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = spawnPoint.position;
    }
}
