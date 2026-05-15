using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Ayarlar")]
    public bool isShake = true;
    public bool isScalable = true;

    [Header("Büyüme Ayarları")]
    public float hoverScale = 1.1f;
    public float scaleDuration = 0.2f;

    [Header("Sallanma Ayarları")]
    public float shakeStrength = 2f;
    public float shakeDuration = 0.5f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (isShake)
        {
            StartRandomIdleShake();
        }
    }

    private void StartRandomIdleShake()
    {

        float randomDelay = Random.Range(0f, 1f);

        float startAngle = Random.Range(-shakeStrength, shakeStrength);
        transform.localRotation = Quaternion.Euler(0, 0, startAngle);


        transform.DOLocalRotate(new Vector3(0, 0, -shakeStrength), shakeDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(randomDelay);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isScalable)
        {
            transform.DOScale(originalScale * hoverScale, scaleDuration).SetEase(Ease.OutBack);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isScalable)
        {
            transform.DOScale(originalScale, scaleDuration).SetEase(Ease.InOutSine);
        }
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}