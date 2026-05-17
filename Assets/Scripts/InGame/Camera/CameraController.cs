using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    [Range(0.01f, 1f)]
    public float smoothTime = 0.2f;

    [Header("Axis Control")]
    public bool followX = true;
    public bool followY = true;

    [Header("Bounds")]
    public bool useBounds = true;
    public Transform minXPoint;
    public Transform maxXPoint;
    public Transform minYPoint;
    public Transform maxYPoint;

    private Vector3 velocity = Vector3.zero;
    private Vector3 shakeOffset;
    private float shakeTimer;
    private float shakeDuration;
    private float shakeStrength;
    public static CameraController Instance;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;
        Vector3 currentPosition = transform.position - shakeOffset;

        float targetX = followX ? targetPosition.x : currentPosition.x;
        float targetY = followY ? targetPosition.y : currentPosition.y;

        Vector3 desiredPosition = new Vector3(targetX, targetY, offset.z);

        Vector3 smoothPosition = Vector3.SmoothDamp(
            currentPosition,
            desiredPosition,
            ref velocity,
            smoothTime
        );

        if (useBounds)
        {
            if (minXPoint != null && maxXPoint != null)
            {
                smoothPosition.x = Mathf.Clamp(
                    smoothPosition.x,
                    minXPoint.position.x,
                    maxXPoint.position.x
                );
            }

            if (minYPoint != null && maxYPoint != null)
            {
                smoothPosition.y = Mathf.Clamp(
                    smoothPosition.y,
                    minYPoint.position.y,
                    maxYPoint.position.y
                );
            }
        }

        UpdateShake();

        transform.position = new Vector3(
            smoothPosition.x,
            smoothPosition.y,
            offset.z
        ) + shakeOffset;
    }

    public void Shake(float duration = 0.1f, float strength = 0.1f)
    {
        shakeDuration = duration;
        shakeStrength = strength;
        shakeTimer = duration;
    }

    private void UpdateShake()
    {
        if (shakeTimer > 0f)
        {
            float power = shakeTimer / shakeDuration;
            shakeOffset = Random.insideUnitCircle * shakeStrength * power;
            shakeTimer -= Time.deltaTime;
            return;
        }

        shakeOffset = Vector3.zero;
    }
}
