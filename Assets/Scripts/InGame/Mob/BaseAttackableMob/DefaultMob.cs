using System.Collections.Generic;
using UnityEngine;

public class DefaultMob : Mob
{
    public bool isGrounded = false;
    private readonly List<Collider2D> _hits = new List<Collider2D>(16);

    public Collider2D GetNearestTargetInRange(Vector2 center, float range)
    {
        _hits.Clear();

        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = attackLayer;
        filter.useTriggers = true; 

        int count = Physics2D.OverlapCircle(center, range, filter, _hits);
        if (count <= 0) return null;

        Collider2D best = null;
        float bestSqr = float.PositiveInfinity;

        for (int i = 0; i < _hits.Count; i++)
        {
            var c = _hits[i];
            if (c == null) continue;
            if (c.transform == transform) continue; 

            float sqr = ((Vector2)c.transform.position - center).sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                best = c;
            }
        }

        return best;
    }
    void FixedUpdate()
    {


    }

    public Collider2D IsTargetInViewRange()
        => GetNearestTargetInRange(rootPoint.position, viewRange);

    public Collider2D IsTargetInViewRange(float range)
        => GetNearestTargetInRange(rootPoint.position, range);

    public Collider2D IsTargetInAttackRange()
        => GetNearestTargetInRange(attackPoint.position, attackRange);

    public Collider2D IsTargetInAttackRange(float range)
        => GetNearestTargetInRange(attackPoint.position, range);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (rootPoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rootPoint.position, viewRange);
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        base.OnCollisionEnter2D(other);
    }
    protected override void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        base.OnCollisionExit2D(other);
    }


}
