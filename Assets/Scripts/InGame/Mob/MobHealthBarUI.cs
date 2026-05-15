using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MobHealthBarUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Mob mob;
    [SerializeField] private Mob_HealthBase healthBase;

    [SerializeField] private GameObject barRoot;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Image frontFill;
    [SerializeField] private Image backFill;

    [Header("Anim")]
    [SerializeField] private float frontDuration = 0.15f;
    [SerializeField] private float backDelay = 0.08f;
    [SerializeField] private float backDuration = 0.35f;
    [SerializeField] private float showHideDuration = 0.15f;

    [Header("Hide")]
    [SerializeField] private float hideDelay = 2f;

    private Coroutine hideCoroutine;
    private bool isDead;

    private void Awake()
    {
        if (mob == null)
            mob = GetComponentInParent<Mob>();

        if (healthBase == null)
            healthBase = GetComponentInParent<Mob_HealthBase>();
    }

    private void OnEnable()
    {
        if (healthBase != null)
        {
            healthBase.damagedEvent += OnDamaged;
            healthBase.deadEvent += OnDead;
        }
    }

    private void OnDisable()
    {
        if (healthBase != null)
        {
            healthBase.damagedEvent -= OnDamaged;
            healthBase.deadEvent -= OnDead;
        }

        KillTweens();

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }
    }

    private void OnDestroy()
    {
        KillTweens();
    }

    private void Start()
    {
        if (barRoot != null)
            barRoot.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        RefreshBarInstant();
    }

    private void LateUpdate()
    {
        if (isDead) return;
        if (mob == null || barRoot == null) return;

        Vector3 rot = barRoot.transform.localEulerAngles;
        rot.y = mob.facingRight == 1 ? 0f : 180f;
        barRoot.transform.localEulerAngles = rot;
    }

    private void OnDamaged(float currentHealth, float maxHealth, Vector2 hitPosition)
    {
        if (isDead) return;

        ShowBar();
        AnimateHealth(currentHealth, maxHealth);
        RestartHideTimer();
    }

    private void OnDead()
    {
        isDead = true;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        KillTweens();

        if (barRoot != null)
            barRoot.SetActive(false);
    }

    private void ShowBar()
    {
        if (isDead) return;

        if (barRoot != null && !barRoot.activeSelf)
            barRoot.SetActive(true);

        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, showHideDuration).SetUpdate(true);
        }
    }

    private void HideBar()
    {
        if (isDead) return;

        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, showHideDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    if (barRoot != null)
                        barRoot.SetActive(false);
                });
        }
        else
        {
            if (barRoot != null)
                barRoot.SetActive(false);
        }
    }

    private void RestartHideTimer()
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideTimer());
    }

    private IEnumerator HideTimer()
    {
        yield return new WaitForSeconds(hideDelay);

        if (!isDead)
            HideBar();

        hideCoroutine = null;
    }

    private void AnimateHealth(float currentHealth, float maxHealth)
    {
        if (isDead) return;
        if (frontFill == null || backFill == null) return;

        float targetFill = maxHealth <= 0 ? 0f : currentHealth / maxHealth;

        frontFill.DOKill();
        backFill.DOKill();

        frontFill.DOFillAmount(targetFill, frontDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);

        backFill.DOFillAmount(targetFill, backDuration)
            .SetDelay(backDelay)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
    }

    private void RefreshBarInstant()
    {
        if (healthBase == null || mob == null) return;
        if (frontFill == null || backFill == null) return;

        float percent = mob.stats.maxHP <= 0 ? 0f : healthBase.currentHealth / mob.stats.maxHP;
        frontFill.fillAmount = percent;
        backFill.fillAmount = percent;
    }

    private void KillTweens()
    {
        if (canvasGroup != null)
            canvasGroup.DOKill();

        if (frontFill != null)
            frontFill.DOKill();

        if (backFill != null)
            backFill.DOKill();
    }
}