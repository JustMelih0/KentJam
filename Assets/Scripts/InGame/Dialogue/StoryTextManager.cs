using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StoryTextManager : MonoBehaviour
{
    public static StoryTextManager Instance { get; private set; }

    [SerializeField] private GameObject textPanel;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private float extraDelayBetweenLines = 0.35f;
    [SerializeField] private float showDuration = 0.25f;
    [SerializeField] private float hiddenScale = 0.92f;
    [SerializeField] private float waveAmount = 4f;
    [SerializeField] private float waveSpeed = 5f;
    [SerializeField] private float waveSpacing = 0.35f;

    private CanvasGroup canvasGroup;
    private RectTransform panelRectTransform;
    private Coroutine playRoutine;
    private Tween currentTween;
    private bool nextLineRequested;
    private TMP_MeshInfo[] cachedMeshInfo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (textPanel != null)
        {
            canvasGroup = textPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = textPanel.AddComponent<CanvasGroup>();
            }

            panelRectTransform = textPanel.GetComponent<RectTransform>();
            textPanel.SetActive(false);
            canvasGroup.alpha = 0f;

            if (panelRectTransform != null)
            {
                panelRectTransform.localScale = Vector3.one * hiddenScale;
            }
        }
    }

    private void OnDestroy()
    {
        currentTween?.Kill();

        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void PlayText(DialogueSO dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines == null || dialogue.dialogueLines.Length == 0 || textPanel == null || storyText == null)
        {
            return;
        }

        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
        }

        currentTween?.Kill();
        nextLineRequested = false;
        playRoutine = StartCoroutine(PlayDialogueRoutine(dialogue));
    }

    public void ShowNextLine()
    {
        nextLineRequested = true;
    }

    private IEnumerator PlayDialogueRoutine(DialogueSO dialogue)
    {
        textPanel.SetActive(true);

        for (int i = 0; i < dialogue.dialogueLines.Length; i++)
        {
            DialogueLine line = dialogue.dialogueLines[i];
            if (line == null)
            {
                continue;
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("mouse-click");
            }

            storyText.text = line.dialogueText;
            storyText.fontSize = line.fontSize;
            storyText.color = line.textColor;

            yield return PlayShowAnimation();

            yield return WaitForLine(line);

            if (i < dialogue.dialogueLines.Length - 1)
            {
                yield return new WaitForSeconds(extraDelayBetweenLines);
            }
        }

        yield return PlayHideAnimation();
        playRoutine = null;
    }

    private IEnumerator WaitForLine(DialogueLine line)
    {
        nextLineRequested = false;
        storyText.ForceMeshUpdate();
        cachedMeshInfo = storyText.textInfo.CopyMeshInfoVertexData();

        if (line.waitForTrigger)
        {
            while (nextLineRequested == false)
            {
                AnimateTextWave();
                yield return null;
            }

            ResetTextMesh();
            yield break;
        }

        float timer = 0f;
        float duration = Mathf.Max(0f, line.dialogueDuration);

        while (timer < duration && nextLineRequested == false)
        {
            AnimateTextWave();
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        ResetTextMesh();
    }

    private void AnimateTextWave()
    {
        TMP_TextInfo textInfo = storyText.textInfo;
        if (cachedMeshInfo == null || cachedMeshInfo.Length == 0)
        {
            cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
        }

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];
            if (!characterInfo.isVisible)
            {
                continue;
            }

            int materialIndex = characterInfo.materialReferenceIndex;
            int vertexIndex = characterInfo.vertexIndex;
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
            Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
            float yOffset = Mathf.Sin((Time.unscaledTime * waveSpeed) + (i * waveSpacing)) * waveAmount;
            Vector3 offset = Vector3.up * yOffset;

            vertices[vertexIndex] = sourceVertices[vertexIndex] + offset;
            vertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] + offset;
            vertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] + offset;
            vertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] + offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            storyText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }

        storyText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }

    private void ResetTextMesh()
    {
        storyText.ForceMeshUpdate();
        cachedMeshInfo = null;
    }

    private IEnumerator PlayShowAnimation()
    {
        currentTween?.Kill();

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        if (panelRectTransform != null)
        {
            panelRectTransform.localScale = Vector3.one * hiddenScale;
        }

        Sequence sequence = DOTween.Sequence();

        if (canvasGroup != null)
        {
            sequence.Join(canvasGroup.DOFade(1f, showDuration).SetEase(Ease.OutQuad));
        }

        if (panelRectTransform != null)
        {
            sequence.Join(panelRectTransform.DOScale(1f, showDuration).SetEase(Ease.OutBack));
        }

        currentTween = sequence;
        yield return sequence.WaitForCompletion();
    }

    private IEnumerator PlayHideAnimation()
    {
        currentTween?.Kill();

        Sequence sequence = DOTween.Sequence();

        if (canvasGroup != null)
        {
            sequence.Join(canvasGroup.DOFade(0f, showDuration).SetEase(Ease.InQuad));
        }

        if (panelRectTransform != null)
        {
            sequence.Join(panelRectTransform.DOScale(hiddenScale, showDuration).SetEase(Ease.InQuad));
        }

        currentTween = sequence;
        yield return sequence.WaitForCompletion();

        textPanel.SetActive(false);
    }
}
