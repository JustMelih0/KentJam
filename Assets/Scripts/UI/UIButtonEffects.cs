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
            StartEqualIdleShake();
        }
    }

    private void StartEqualIdleShake()
    {
        transform.localRotation = Quaternion.identity;


        transform.DOLocalRotate(new Vector3(0, 0, -shakeStrength), shakeDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
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