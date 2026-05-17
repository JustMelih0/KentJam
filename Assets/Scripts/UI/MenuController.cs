using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    private bool isMuted = false;

    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        UIScreenFader.Instance.Open();
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
        OnMusicSliderChanged();
        OnSfxSliderChanged();
    }

    public void OnMusicButtonClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("mouse-click");
        }
        isMuted = !isMuted;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleSound();
        }
        UpdateButtonVisual();
    }

    public void OnMusicSliderChanged()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
    }
    public void OnSfxSliderChanged()
    {
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
    }

    public void ChangeScene(string sceneName)
    {
        if(sceneName == "Level1") AudioManager.Instance.PlaySFX("play");
        else AudioManager.Instance.PlaySFX("mouse-click");
        UIScreenFader.Instance.CloseAndLoadScene(sceneName);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    private void UpdateButtonVisual()
    {
    }
}