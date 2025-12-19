using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public DialogueManager dialogueManager; // 引用 DialogueManager
    public Sprite Playerportrait1;
    public Sprite Playerportrait2;

    private void Start()
    {
        // 创建测试对话
        var testDialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {

                new DialogueLine { speakerName = "Alice", text = "你好，我是Alice！", portrait = Playerportrait1 },
                new DialogueLine { speakerName = "Bob", text = "你好，我是Bob！", portrait =Playerportrait2 },
                new DialogueLine { speakerName = "Alice", text = "很高兴认识你，Bob！", portrait = Playerportrait1 }
            }
        };

        // 启动对话
        dialogueManager.StartDialogue(testDialogue, OnDialogueFinished);
    }

    private void OnDialogueFinished()
    {
        Debug.Log("对话结束！");
    }
}
