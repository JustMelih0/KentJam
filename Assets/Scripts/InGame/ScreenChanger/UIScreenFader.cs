using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class UIScreenFader : MonoBehaviour
{
    public static UIScreenFader Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private DialogueSO openDialogue;

    [Header("Settings")]
    [SerializeField] private float openDelay = 0.2f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease openEase = Ease.OutCubic;
    [SerializeField] private Ease closeEase = Ease.OutCubic;

    private Tween currentTween;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Open()
    {
        Open(null);
    }

    public void Open(Action onComplete)
    {
        if (fadeImage == null)
            return;

        currentTween?.Kill();

        SetAlpha(1f);

        currentTween = DOVirtual.DelayedCall(openDelay, () =>
        {
            currentTween = fadeImage
                .DOFade(0f, duration)
                .SetEase(openEase)
                .OnComplete(() =>
                {
                    if(openDialogue) DialogueManager.Instance.StartDialogue(openDialogue);
                    onComplete?.Invoke();
                });
        });
    }

    public void Close()
    {
        FadeTo(1f, duration, closeEase, null);
    }

    public void Close(Action onComplete)
    {
        FadeTo(1f, duration, closeEase, onComplete);
    }

    public void CloseAndLoadScene(string sceneName)
    {
        FadeTo(1f, duration, closeEase, () =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    public void SetOpenInstant()
    {
        currentTween?.Kill();
        SetAlpha(0f);
    }

    public void SetClosedInstant()
    {
        currentTween?.Kill();
        SetAlpha(1f);
    }

    private void FadeTo(float targetAlpha, float time, Ease ease, Action onComplete)
    {
        if (fadeImage == null)
            return;

        currentTween?.Kill();

        currentTween = fadeImage
            .DOFade(targetAlpha, time)
            .SetEase(ease)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    private void SetAlpha(float alpha)
    {
        if (fadeImage == null)
            return;

        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}