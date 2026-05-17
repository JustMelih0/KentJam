using UnityEngine;

public class Spark : MonoBehaviour
{

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.playerHealth.TakeDamage(1000, transform.position);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.playerHealth.TakeDamage(1000, transform.position);
        }
    }
}
