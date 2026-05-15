
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Mob : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rgb2d;
    [HideInInspector] public Animator anim;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public BoxCollider2D boxCollider2D;
    [HideInInspector] public Mob_HealthBase mob_HealthBase;

    protected StateMachine stateMachine;

    [HideInInspector] public Material defaultMaterial;
    public int facingRight = 1;

    public Stats stats;

    public float viewRange = 1f;
    public float attackRange = 0.3f;
    public LayerMask attackLayer;
    public Transform attackPoint;
    [HideInInspector] public bool isAttacking = false; 
    public Transform rootPoint;
    public Transform footPoint;
    public float footRadius;

    protected virtual void Awake()
    {
        InitComponents();
    }
    protected virtual void InitComponents()
    {
        TryGetComponent(out rgb2d);
        TryGetComponent(out anim);
        TryGetComponent(out spriteRenderer);
        TryGetComponent(out boxCollider2D);
        TryGetComponent(out mob_HealthBase);
        TryGetComponent(out stateMachine);
    }
    protected virtual void Start()
    {
        defaultMaterial = spriteRenderer.material;
    }
    protected virtual void MobFlip()
    {
        facingRight *= -1;
        transform.rotation = Quaternion.Euler(transform.rotation.x, facingRight == 1 ? 0 : 180, transform.rotation.z);
    }
    public virtual void FaceToTarget(Vector2 targetPoint)
    {
        if (transform.position.x < targetPoint.x && facingRight == -1 || transform.position.x > targetPoint.x && facingRight == 1)
        {
            MobFlip();
        }
    }
    protected virtual void OnDrawGizmos()
    {
        if(attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        if(footPoint == null) return;
        Gizmos.DrawWireSphere(footPoint.position, footRadius);
    }
    protected virtual void OnCollisionEnter2D(Collision2D other) {
        stateMachine.currentState?.CollisionEnter(other);
    }
    protected virtual void OnCollisionExit2D(Collision2D other) {
        stateMachine.currentState?.CollisionExit(other);
    }
    public void ResetAnimatorTriggers()
    {
        if (anim == null) return;

        foreach (AnimatorControllerParameter parameter in anim.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
            {
                anim.ResetTrigger(parameter.nameHash);
            }
        }
    }
}
