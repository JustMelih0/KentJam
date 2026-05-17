
using UnityEngine;

public class AO_Ivy : ActivatableObject
{
    [SerializeField] private float deActivateTime = 5f;
    [SerializeField] private float activeWidth = 5f;
    [SerializeField] private float growSpeed = 6f;
    [SerializeField]protected bool showStoryNext = false;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private float startWidth;
    private Vector2 startColliderSize;
    private Vector2 startColliderOffset;
    private float targetWidth;
    private Animator animator;

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);
        TryGetComponent(out boxCollider);
        TryGetComponent(out animator);

        if (spriteRenderer != null)
        {
            startWidth = spriteRenderer.size.x;
            targetWidth = startWidth;
        }

        if (boxCollider != null)
        {
            startColliderSize = boxCollider.size;
            startColliderOffset = boxCollider.offset;
        }
    }

    private void Update()
    {
        if (spriteRenderer == null) return;

        float currentWidth = spriteRenderer.size.x;
        if (Mathf.Approximately(currentWidth, targetWidth)) return;

        float newWidth = Mathf.MoveTowards(
            currentWidth,
            targetWidth,
            growSpeed * Time.deltaTime
        );

        SetWidth(newWidth);
    }

    public override void Activate()
    {
        if (activated) return;

        base.Activate();

        if(showStoryNext)
        {
            showStoryNext = false;
            StoryTextManager.Instance.ShowNextLine();
        }
        AudioManager.Instance.PlaySFX("mouse-click");
        if(animator) animator.SetTrigger("open");
        targetWidth = activeWidth;
        Invoke(nameof(Deactivate), deActivateTime);
    }

    public override void Deactivate()
    {
        if (!activated) return;

        base.Deactivate();
        if(animator) animator.SetTrigger("close");
        targetWidth = startWidth;
    }

    private void SetWidth(float width)
    {
        spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);

        if (boxCollider == null || startWidth <= 0f) return;

        float ratio = width / startWidth;
        boxCollider.size = new Vector2(startColliderSize.x * ratio, startColliderSize.y);
        boxCollider.offset = new Vector2(startColliderOffset.x * ratio, startColliderOffset.y);
    }
}
