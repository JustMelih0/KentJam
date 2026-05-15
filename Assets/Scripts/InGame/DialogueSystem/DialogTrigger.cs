using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField]private DialogueSO dialogueSO;
    public bool canDestroy = true;


    public void SendDialogMessage()
    {
        DialogueManager.Instance.StartDialogue(dialogueSO);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DialogueManager.Instance.StartDialogue(dialogueSO);
            if(canDestroy)
            Destroy(gameObject);
        }
    }

}
