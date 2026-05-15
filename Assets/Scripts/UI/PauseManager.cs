using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private CanvasGroup canvasGroup;
    private bool isPaused = false;

    void Awake()
    {
        canvasGroup = pausePanel.GetComponent<CanvasGroup>();
        ResetPauseSystem();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    private void ResetPauseSystem()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            if (canvasGroup != null) canvasGroup.alpha = 0f;
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.Instance.PlaySFX("mouse-click");

        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(1, 0.5f).SetUpdate(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFX("mouse-click");

        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0, 0.3f).SetUpdate(true).OnComplete(() =>
            {
                pausePanel.SetActive(false);
            });
        }
        else
        {
            pausePanel.SetActive(false);
        }
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        AudioManager.Instance.PlaySFX("mouse-click");
        DOTween.KillAll();
        if (pausePanel != null) pausePanel.SetActive(false);

        UIScreenFader.Instance.CloseAndLoadScene("Menu");
    }
}