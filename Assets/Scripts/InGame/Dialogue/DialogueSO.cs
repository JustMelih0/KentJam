using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] dialogueLines;
}
[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 5)] public string dialogueText;
    public float typeSpeed = 0.05f; 
    public float dialogueDuration = 1f;
    public bool waitForTrigger;
    public bool isShaking;
    public Color textColor = Color.white;
    public float fontSize = 70;
}
