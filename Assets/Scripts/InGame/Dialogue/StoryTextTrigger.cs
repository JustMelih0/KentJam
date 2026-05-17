using System.Collections.Generic;
using UnityEngine;

public class StoryTextTrigger : MonoBehaviour, I_Interactable
{
    [SerializeField] private List<DialogueSO> dialogueSo;

    public bool canDestroy = true;
    public bool isTriggerDialogue = false;

    private int index = 0;

    public void SendStoryText()
    {
        if (dialogueSo == null || dialogueSo.Count == 0 || StoryTextManager.Instance == null)
        {
            return;
        }

        int targetIndex = index;

        if (targetIndex < 0 || targetIndex >= dialogueSo.Count || dialogueSo[targetIndex] == null)
        {
            targetIndex = Mathf.Clamp(index - 1, 0, dialogueSo.Count - 1);
        }

        if (dialogueSo[targetIndex] != null)
        {
            StoryTextManager.Instance.PlayText(dialogueSo[targetIndex]);
        }

        index++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isTriggerDialogue)
        {
            SendStoryText();

            if (canDestroy)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Interact()
    {
        SendStoryText();

        if (canDestroy)
        {
            Destroy(gameObject);
        }
    }
}
