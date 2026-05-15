using UnityEngine;

public class Player : Mob
{
    public float horizontalInput;
    public LayerMask groundLayer;
    public ParticleSystem dustParticle;

    [Header("Jump Settings")]
    public float coyoteTime = 0.15f;

    private float coyoteTimeCounter;

    void Update()
    {
        UpdateCoyoteTime();
        Anime();
    }

    void Anime()
    {
        //anim.SetFloat("HorizontalInput", Mathf.Abs(horizontalInput));
    }

    void UpdateCoyoteTime()
    {
        if (IsGrounded())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    public void FaceToInput()
    {
        if ((horizontalInput > 0 && facingRight == -1) || (horizontalInput < 0 && facingRight == 1))
        {
            PlayDustEffect();
            MobFlip();
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(footPoint.position, footRadius, groundLayer);
    }

    public bool CanJumpWithCoyoteTime()
    {
        return coyoteTimeCounter > 0f;
    }

    public void ConsumeCoyoteTime()
    {
        coyoteTimeCounter = 0f;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (footPoint)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(footPoint.position, footRadius);
        }
    }

    public void PlayDustEffect()
    {
        if (dustParticle != null)
            dustParticle.Play();
    }
}