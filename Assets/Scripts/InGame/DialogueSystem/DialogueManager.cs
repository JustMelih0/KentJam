using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 5)] public string dialogueText;
    public float typeSpeed = 0.05f; 
    public bool isShaking;
    public float fontSize = 70;
}


public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    private Coroutine typingCoroutine;
    
    [SerializeField]private DialogueSO currentDialogue;
    private int currentIndex = 0;
    
    private bool isTyping = false;

    public static DialogueManager Instance;

    public static event Action<DialogueSO> DialogueEnterEvent;
    public static event Action<DialogueSO> DialogueExitEvent;
    
    void Start()
    {
        dialoguePanel.SetActive(false);
    }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && dialoguePanel.activeInHierarchy)
        {
            ShowNextLine();
        }
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        //AudioManager.Instance.PlaySFX("talk");
        currentDialogue = dialogue;
        DialogueEnterEvent?.Invoke(currentDialogue);
        currentIndex = 0;
        dialoguePanel.SetActive(true);

        ShowNextLine();
    }
    
    public void ShowNextLine()
    {
        AudioManager.Instance.PlaySFX("mouse-click");
        if (currentIndex >= currentDialogue.dialogueLines.Length)
        {
            EndDialogue();
            return;
        }


        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine); 
            dialogueText.transform.localPosition = Vector3.zero; 
        }
        

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue.dialogueLines[currentIndex].dialogueText;
            isTyping = false;
            currentIndex++;
            return;
        }

        
        if (currentIndex < currentDialogue.dialogueLines.Length)
        {

            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine); 
                dialogueText.transform.localPosition = Vector3.zero; 
            }

            typingCoroutine = StartCoroutine(TypeText(currentDialogue.dialogueLines[currentIndex]));
        }
        else
        {
            EndDialogue();
        }
    }
    
    private IEnumerator TypeText(DialogueLine line)
    {
        isTyping = true;
        dialogueText.text = "";
        dialogueText.fontSize = line.fontSize;

        if (line.isShaking)
        {
            if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(ShakeText()); 
        }

        foreach (char letter in line.dialogueText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(line.typeSpeed);
        }
        isTyping = false;
        currentIndex++;
    }

    private Coroutine shakeCoroutine; 

    private IEnumerator ShakeText()
    {
        Vector3 originalPosition = dialogueText.transform.localPosition;
        int defaultIndex = currentIndex;
        while (defaultIndex == currentIndex) 
        {
            float x = UnityEngine.Random.Range(-2f, 2f);
            float y = UnityEngine.Random.Range(-2f, 2f);
            dialogueText.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            yield return new WaitForSeconds(0.05f);
        }

        dialogueText.transform.localPosition = originalPosition; 
    }

    
    public void EndDialogue()
    {
        DialogueExitEvent?.Invoke(currentDialogue);
        dialoguePanel.SetActive(false);
    }
}
