using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] dialogueLines;
}