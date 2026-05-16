using UnityEngine;


public class Bear_Mob : DefaultMob, IActivate
{
    public Transform targetMovePoint;
    public GameObject eyeParticle;

    private Transform playerOnBear;
    private Vector2 lastPosition;

    protected override void Start()
    {
        base.Start();
        lastPosition = rgb2d.position;
    }

    private void LateUpdate()
    {
        Vector2 currentPosition = rgb2d.position;
        Vector2 delta = currentPosition - lastPosition;

        if (playerOnBear != null)
        {
            playerOnBear.position += new Vector3(delta.x, 0f, 0f);
        }

        lastPosition = currentPosition;
    }

    public void Activate()
    {
        stateMachine.Brain("Wake");
    }

    public void Deactivate()
    {
        stateMachine.Brain("Sleep");
    }
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        TrySetPlayerParent(other);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        TrySetPlayerParent(other);
    }

    protected override void OnCollisionExit2D(Collision2D other)
    {
        base.OnCollisionExit2D(other);

        if (other.gameObject.CompareTag("Player") && other.transform == playerOnBear)
        {
            playerOnBear = null;
        }
    }

    private void TrySetPlayerParent(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (other.transform.position.y > transform.position.y)
        {
            playerOnBear = other.transform;
        }
    }
}
