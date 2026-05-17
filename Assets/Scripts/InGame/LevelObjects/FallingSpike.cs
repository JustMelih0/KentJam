using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingSpike : Spark
{
    [Header("Detect")]
    [SerializeField] private Vector2 triggerBoxOffset = Vector2.down;
    [SerializeField] private Vector2 triggerBoxSize = new Vector2(2f, 4f);
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;

    [Header("Shake")]
    [SerializeField] private float idleShakeAmount = 3f;
    [SerializeField] private float idleShakeSpeed = 3f;

    [Header("Fall")]
    [SerializeField] private float fallGravity = 3f;

    private Rigidbody2D rb;
    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;
    private bool isFalling;
    private bool isStopped;
    private int groundContactCount;

    private void Awake()
    {
        TryGetComponent(out rb);
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;

        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.constraints |= RigidbodyConstraints2D.FreezePosition;
    }

    private void Update()
    {
        if (isStopped || isFalling)
            return;

        IdleShake();

        if (Physics2D.OverlapBox(GetTriggerBoxCenter(), triggerBoxSize, 0f, playerLayer))
        {
            Fall();
        }
    }

    private void IdleShake()
    {
        float angle = Mathf.Sin(Time.time * idleShakeSpeed) * idleShakeAmount;
        transform.localRotation = startLocalRotation * Quaternion.Euler(0f, 0f, angle);
    }

    private void Fall(bool resetToStart = true)
    {
        isFalling = true;
        isStopped = false;
        groundContactCount = 0;

        if (resetToStart)
        {
            transform.localPosition = startLocalPosition;
            transform.localRotation = startLocalRotation;
        }

        rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        rb.gravityScale = fallGravity;
    }

    private void StopFalling()
    {
        isFalling = false;
        isStopped = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        rb.constraints |= RigidbodyConstraints2D.FreezePosition;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        bool hitGround = IsInLayerMask(collision.gameObject.layer, groundLayer);
        if (hitGround)
        {
            groundContactCount++;

            if (isFalling)
            {
                StopFalling();
            }

            return;
        }

        if (!isStopped)
        {
            base.OnCollisionEnter2D(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsInLayerMask(collision.gameObject.layer, groundLayer))
            return;

        groundContactCount = Mathf.Max(0, groundContactCount - 1);

        if (isStopped && groundContactCount == 0)
        {
            Fall(false);
        }
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

    private Vector2 GetTriggerBoxCenter()
    {
        return (Vector2)transform.position + triggerBoxOffset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetTriggerBoxCenter(), triggerBoxSize);
    }
}
