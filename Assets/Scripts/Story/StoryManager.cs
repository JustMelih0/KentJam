using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public Image storyImage;
    public List<Sprite> sprites = new();
    public float imageDuration = 2f;
    public float transitionDuration = 0.5f;
    public float startScale = 1.08f;
    public float endScale = 0.95f;

    private Sequence storySequence;

    private void Start()
    {
        PlayStory();
    }

    private void OnDisable()
    {
        storySequence?.Kill();
    }

    public void PlayStory()
    {
        if (storyImage == null || sprites == null || sprites.Count == 0)
        {
            StartGame();
            return;
        }

        storySequence?.Kill();
        storySequence = DOTween.Sequence();

        for (int i = 0; i < sprites.Count; i++)
        {
            Sprite sprite = sprites[i];
            storySequence.AppendCallback(() => SetStoryImage(sprite));
            storySequence.Append(storyImage.rectTransform.DOScale(endScale, imageDuration).SetEase(Ease.InOutSine));
            storySequence.Join(storyImage.DOFade(1f, imageDuration * 0.75f).SetEase(Ease.OutSine));
            storySequence.Append(storyImage.DOFade(0f, transitionDuration).SetEase(Ease.InOutSine));
        }

        storySequence.OnComplete(StartGame);
    }

    private void SetStoryImage(Sprite sprite)
    {
        storyImage.sprite = sprite;
        storyImage.color = new Color(storyImage.color.r, storyImage.color.g, storyImage.color.b, 0f);
        storyImage.rectTransform.localScale = Vector3.one * startScale;
    }

    public void StartGame()
    {
        AudioManager.Instance.PlaySFX("mouse-click");
        UIScreenFader.Instance.CloseAndLoadScene("Level1");

    }
}
