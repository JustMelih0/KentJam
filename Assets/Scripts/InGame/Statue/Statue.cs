using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Statue : MonoBehaviour, I_Interactable
{
    public GameObject lowerStatue;
    public GameObject upperStatue;
    public Animator animator;
    public Transform statueStonePoint;
    public Transform statueWalkPoint;
    public Transform projectilePoint;
    public Light2D upperLight;
    protected Collider2D upperCollider;

    [SerializeField] private float moveAmount = 0.5f;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float upperLightTargetIntensity = 1.2f;

    private bool isOpen;

    void Awake()
    {
        upperStatue.TryGetComponent(out upperCollider);       
    }
    public void Interact()
    {
        if (isOpen) return;

        isOpen = true;
        GameManager.Instance.playerStateMachine.InputRequest("StoneState");

    }

    private System.Collections.IEnumerator OpenStatue()
    {
        if (lowerStatue == null || upperStatue == null) yield break;

        Vector3 lowerStartPosition = lowerStatue.transform.position;
        Vector3 upperStartPosition = upperStatue.transform.position;
        Vector3 lowerTargetPosition = lowerStartPosition + Vector3.down * moveAmount;
        Vector3 upperTargetPosition = upperStartPosition + Vector3.up * moveAmount;
        upperCollider.isTrigger = true;

        if (moveDuration <= 0f)
        {
            lowerStatue.transform.position = lowerTargetPosition;
            upperStatue.transform.position = upperTargetPosition;
            yield break;
        }

        float timer = 0f;
        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / moveDuration);
            t = t * t * (3f - 2f * t);

            lowerStatue.transform.position = Vector3.Lerp(lowerStartPosition, lowerTargetPosition, t);
            upperStatue.transform.position = Vector3.Lerp(upperStartPosition, upperTargetPosition, t);
            yield return null;
        }

        lowerStatue.transform.position = lowerTargetPosition;
        upperStatue.transform.position = upperTargetPosition;
    }
    public void ActivateStatue()
    {
        if (animator != null)
        {
            animator.SetTrigger("open");
        }

        StartCoroutine(OpenStatue());
        StartCoroutine(OpenUpperLight());
    }

    private System.Collections.IEnumerator OpenUpperLight()
    {
        if (upperLight == null) yield break;

        upperLight.intensity = 0f;

        if (moveDuration <= 0f)
        {
            upperLight.intensity = upperLightTargetIntensity;
            yield break;
        }

        float timer = 0f;
        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / moveDuration);
            t = t * t * (3f - 2f * t);

            upperLight.intensity = Mathf.Lerp(0f, upperLightTargetIntensity, t);
            yield return null;
        }

        upperLight.intensity = upperLightTargetIntensity;
    }
}
