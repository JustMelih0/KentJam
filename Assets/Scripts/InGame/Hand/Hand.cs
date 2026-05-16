using UnityEngine;

public class Hand : MonoBehaviour
{
    public GameObject player;
    public Transform landingPoint;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float waitDuration = 0.5f;

    private void Start()
    {
        FreezePlayerPosition();
        StartCoroutine(MoveHandToLandingPoint());
    }

    public void FreezePlayerPosition()
    {
        if (player == null) return;
        if (!player.TryGetComponent(out Rigidbody2D playerRigidbody)) return;

        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    private System.Collections.IEnumerator MoveHandToLandingPoint()
    {
        if (player == null || landingPoint == null) yield break;
        Vector3 handStartPosition = transform.position;

        if (moveDuration <= 0f)
        {
            transform.position = landingPoint.position;
            player.transform.SetParent(null, true);
            UnfreezePlayerPosition();
            yield return new WaitForSeconds(waitDuration);
            transform.position = handStartPosition;
            yield break;
        }

        Vector3 targetPosition = landingPoint.position;
        yield return MoveHand(handStartPosition, targetPosition);

        transform.position = targetPosition;
        player.transform.SetParent(null, true);
        UnfreezePlayerPosition();

        yield return new WaitForSeconds(waitDuration);
        yield return MoveHand(targetPosition, handStartPosition);
        transform.position = handStartPosition;
    }

    private System.Collections.IEnumerator MoveHand(Vector3 startPosition, Vector3 targetPosition)
    {
        float timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / moveDuration);
            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }

    private void UnfreezePlayerPosition()
    {
        if (player == null) return;
        if (!player.TryGetComponent(out Rigidbody2D playerRigidbody)) return;

        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
