using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float moveSpeed;
    private Vector2 direction;
    private bool setup = false;

    public void SetupProjectile(Vector2 direction, float speed)
    {
        this.direction = direction.sqrMagnitude > Mathf.Epsilon ? direction.normalized : Vector2.right;
        moveSpeed = speed;
        setup = true;

        transform.right = this.direction;
        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        if(setup == false) return;

        Vector2 nextPosition = (Vector2)transform.position + moveSpeed * Time.fixedDeltaTime * direction;
        transform.position = nextPosition;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (setup == false) return;

        if (collision.TryGetComponent(out IActivate activate))
        {
            activate.Activate();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
