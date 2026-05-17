using UnityEngine;

public class AO_Geyser : ActivatableObject
{
    public float jumpForce = 15f;
    public Transform geyserPoint;
    public float geyserRange = 0.3f;
    public LayerMask geyserLayer;
    protected Animator animator;

    void Awake()
    {
        TryGetComponent(out animator);
    }
    public override void Activate()
    {
        if(activated) return;
        base.Activate();

        if (animator)
        {
            animator.SetTrigger("active");
        }

    }
    public override void Deactivate()
    {
        base.Deactivate();
    }
    public void GayserPush()
    {
        AudioManager.Instance.PlaySFX("geyser");
        Collider2D col = Physics2D.OverlapCircle(geyserPoint.position, geyserRange, geyserLayer);
        if (col && col.TryGetComponent(out Rigidbody2D rb))
        {
            CameraController.Instance.Shake();
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
        }
        Deactivate();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(geyserPoint.position, geyserRange);
    }
}
