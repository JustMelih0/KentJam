using UnityEngine;

public class Player : Mob
{
    private const float MinVerticalVelocity = -10f;
    private const float MaxVerticalVelocity = 10f;

    public float horizontalInput;
    public LayerMask groundLayer;
    public ParticleSystem dustParticle;
    public float dustMovementThreshold = 0.05f;

    [Header("Jump Settings")]
    public float coyoteTime = 0.15f;

    private float coyoteTimeCounter;
    private bool isDustPlaying = false;

    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        ClampVerticalVelocity();
    }

    void Update()
    {
        UpdateCoyoteTime();
        Anime();
        DustControl();
    }

    private void ClampVerticalVelocity()
    {
        rgb2d.linearVelocityY = Mathf.Clamp(
            rgb2d.linearVelocityY,
            MinVerticalVelocity,
            MaxVerticalVelocity
        );
    }

    void DustControl()
    {
        bool isMoving = rgb2d.linearVelocity.sqrMagnitude > dustMovementThreshold * dustMovementThreshold;

        if (isMoving && isDustPlaying == false)
        {
            PlayDustEffect();
        }
        else if (isMoving == false && isDustPlaying == true)
        {
            StopDustEffect();
        }
    }

    void Anime()
    {
        anim.SetFloat("HorizontalInput", Mathf.Abs(horizontalInput));
        anim.SetBool("isGrounded", IsGrounded());
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

        isDustPlaying = true;
    }
    public void StopDustEffect()
    {
        if (dustParticle != null)
            dustParticle.Stop();
        
        isDustPlaying = false;
    }
}
