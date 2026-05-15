using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public partial class CreditsManager : MonoBehaviour
{
    [Tooltip("Saat Yönü Çarkları")]
    public List<RectTransform> clockwiseGears;
    [Tooltip("Saat Yönü Tersi Çarkları")]
    public List<RectTransform> counterclockwiseGears;

    public float baseRotationSpeed = 50f;
    public float fastSpeedMultiplier = 5f;
    public Animator creditsAnimator;

    private float currentSpeed;
    private bool changeScene = false;
    void Start()
    {
        UIScreenFader.Instance.Open();
    }

    void Update()
    {
        bool isFast = Keyboard.current.enterKey.isPressed || Keyboard.current.spaceKey.isPressed || Mouse.current.leftButton.isPressed;

        if (isFast)
        {
            currentSpeed = baseRotationSpeed * fastSpeedMultiplier;
            if (creditsAnimator != null) creditsAnimator.speed = fastSpeedMultiplier;
        }
        else
        {
            currentSpeed = baseRotationSpeed;
            if (creditsAnimator != null) creditsAnimator.speed = 1f;
        }


        RotateGears();


        if (Keyboard.current.escapeKey.wasPressedThisFrame && !changeScene)
        {
            changeScene = true;
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("mouse-click");

            Debug.Log("Menüye dönülüyor...");
            UIScreenFader.Instance.CloseAndLoadScene("Menu");
        }
    }

    private void RotateGears()
    {
        float rotationAmount = currentSpeed * Time.deltaTime;

        foreach (RectTransform gear in clockwiseGears)
        {
            if (gear != null) gear.Rotate(0, 0, -rotationAmount);
        }
        foreach (RectTransform gear in counterclockwiseGears)
        {
            if (gear != null) gear.Rotate(0, 0, rotationAmount);
        }
    }
}