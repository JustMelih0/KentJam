using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class UIScreenFader : MonoBehaviour
{
    public static UIScreenFader Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Image upperImage;
    [SerializeField] private Image lowerImage;
    [SerializeField] private DialogueSO openDialogue;
    [SerializeField] private DialogueSO[] openDialogues;

    [Header("Settings")]
    [SerializeField] private float openDelay = 0.2f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease openEase = Ease.OutCubic;
    [SerializeField] private Ease closeEase = Ease.OutCubic;

    private Tween currentTween;
    private RectTransform upperRect;
    private RectTransform lowerRect;
    private Vector2 upperClosedPosition;
    private Vector2 lowerClosedPosition;
    private Vector2 upperOpenPosition;
    private Vector2 lowerOpenPosition;
    private bool positionsCached;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CachePositions();
    }

    public void Open()
    {
        Open(null);
    }

    public void Open(Action onComplete)
    {
        if (CachePositions() == false)
            return;

        currentTween?.Kill();
        SetImagesActive(true);
        SetClosedInstant();

        currentTween = DOVirtual.DelayedCall(openDelay, () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Join(upperRect.DOAnchorPos(upperOpenPosition, duration).SetEase(openEase));
            sequence.Join(lowerRect.DOAnchorPos(lowerOpenPosition, duration).SetEase(openEase));
            sequence.OnComplete(() =>
            {
                SetImagesActive(false);
                onComplete?.Invoke();
            });

            currentTween = sequence.SetLink(gameObject, LinkBehaviour.KillOnDisable);
        }).SetLink(gameObject, LinkBehaviour.KillOnDisable);
    }

    public void Close()
    {
        Close(null);
    }

    public void Close(Action onComplete)
    {
        if (CachePositions() == false)
            return;

        currentTween?.Kill();
        SetImagesActive(true);
        AudioManager.Instance?.PlaySFX("cut");

        Sequence sequence = DOTween.Sequence();
        sequence.Join(upperRect.DOAnchorPos(upperClosedPosition, duration).SetEase(closeEase));
        sequence.Join(lowerRect.DOAnchorPos(lowerClosedPosition, duration).SetEase(closeEase));
        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });

        currentTween = sequence.SetLink(gameObject, LinkBehaviour.KillOnDisable);
    }

    public void CloseAndLoadScene(string sceneName)
    {
        Close(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    public void SetOpenInstant()
    {
        if (CachePositions() == false)
            return;

        currentTween?.Kill();
        SetImagesActive(false);
        upperRect.anchoredPosition = upperOpenPosition;
        lowerRect.anchoredPosition = lowerOpenPosition;
    }

    public void SetClosedInstant()
    {
        if (CachePositions() == false)
            return;

        currentTween?.Kill();
        SetImagesActive(true);
        upperRect.anchoredPosition = upperClosedPosition;
        lowerRect.anchoredPosition = lowerClosedPosition;
    }

    private bool CachePositions()
    {
        if (upperImage == null || lowerImage == null)
            return false;

        if (upperRect == null)
            upperRect = upperImage.rectTransform;

        if (lowerRect == null)
            lowerRect = lowerImage.rectTransform;

        if (positionsCached)
            return true;

        upperClosedPosition = upperRect.anchoredPosition;
        lowerClosedPosition = lowerRect.anchoredPosition;

        float upperTravelDistance = GetTravelDistance(upperRect);
        float lowerTravelDistance = GetTravelDistance(lowerRect);

        upperOpenPosition = upperClosedPosition + Vector2.up * upperTravelDistance;
        lowerOpenPosition = lowerClosedPosition + Vector2.down * lowerTravelDistance;
        positionsCached = true;
        return true;
    }

    private float GetTravelDistance(RectTransform rectTransform)
    {
        return rectTransform.rect.height + 50f;
    }

    private void SetImagesActive(bool isActive)
    {
        if (upperImage != null && upperImage.gameObject.activeSelf != isActive)
            upperImage.gameObject.SetActive(isActive);

        if (lowerImage != null && lowerImage.gameObject.activeSelf != isActive)
            lowerImage.gameObject.SetActive(isActive);
    }

    private void OnDisable()
    {
        currentTween?.Kill();
        currentTween = null;
    }
}
