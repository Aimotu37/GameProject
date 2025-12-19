using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public string interactionType; // 例如 "PasswordPiece" 或 "Diary"
    public string interactionDialogue; // 对应的对白内容

    private bool isInteracted = false;

    public void Interact()
    {
        if (isInteracted) return;

        isInteracted = true;

        // 触发对白
        var interactionDialogueSession = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{speakerName ="主控", text =interactionDialogue }
            }
        };
        FindObjectOfType<DialogueManager>().StartDialogue(interactionDialogueSession);

        // 更新任务状态
        if (interactionType == "PasswordPiece")
        {
            FindObjectOfType<TaskManager>().FindPasswordPiece();
        }
        else if (interactionType == "Diary")
        {
            FindObjectOfType<TaskManager>().FindDiary();
        }
    }
}
