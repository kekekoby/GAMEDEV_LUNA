using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airControlFactor = 0.4f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private float jumpCooldownTime = 0.25f;
    [SerializeField] private Key jumpKey = Key.Space;

    [Header("Ground Detection")]
    [SerializeField] private float capsuleHeight = 2f;
    [SerializeField] private LayerMask groundLayers;

    [Header("Gravity Feel")]
    [SerializeField] private float fallGravityScale = 2.5f;

    private Rigidbody body;

    private Vector2 inputAxis;
    private bool isGrounded;
    private bool canJump = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
    }

    private void Update()
    {
        CheckGrounded();
        ReadInput();
        ClampHorizontalSpeed();
        UpdateDrag();
    }

    private void FixedUpdate()
    {
        ApplyMovementForce();
        ApplyExtraFallGravity();
    }

    // ---------- Input & State ----------

    private void ReadInput()
    {
        float x = 0f, z = 0f;
        var kb = Keyboard.current;

        if (kb.dKey.isPressed) x += 1f;
        if (kb.aKey.isPressed) x -= 1f;
        if (kb.wKey.isPressed) z += 1f;
        if (kb.sKey.isPressed) z -= 1f;

        inputAxis = new Vector2(x, z);

        if (kb[jumpKey].isPressed && canJump && isGrounded)
        {
            PerformJump();
        }
    }

    private void CheckGrounded()
    {
        float rayLength = (capsuleHeight * 0.5f) + 0.2f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayLength, groundLayers);
    }

    private void UpdateDrag()
    {
        body.linearDamping = isGrounded ? groundDrag : 0f;
    }

    // ---------- Movement ----------

    private void ApplyMovementForce()
    {
        Vector3 wishDir = (orientation.forward * inputAxis.y + orientation.right * inputAxis.x).normalized;

        float forceMultiplier = isGrounded ? 1f : airControlFactor;
        body.AddForce(wishDir * walkSpeed * 10f * forceMultiplier, ForceMode.Force);
    }

    private void ClampHorizontalSpeed()
    {
        Vector3 horizontalVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);

        if (horizontalVelocity.magnitude <= walkSpeed) return;

        Vector3 clamped = horizontalVelocity.normalized * walkSpeed;
        body.linearVelocity = new Vector3(clamped.x, body.linearVelocity.y, clamped.z);
    }

    // ---------- Jumping ----------

    private void PerformJump()
    {
        canJump = false;

        // Zero out vertical velocity first so jump height is consistent
        body.linearVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);
        body.AddForce(transform.up * jumpPower, ForceMode.Impulse);

        Invoke(nameof(RearmJump), jumpCooldownTime);
    }

    private void RearmJump()
    {
        canJump = true;
    }

    // ---------- Gravity Tuning ----------

    private void ApplyExtraFallGravity()
    {
        if (body.linearVelocity.y < 0f)
        {
            body.AddForce(Physics.gravity * (fallGravityScale - 1f), ForceMode.Acceleration);
        }
    }
}