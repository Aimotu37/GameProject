
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum ItemType
{
    Bed,          //床
    NoteBook,     //密码本
    Note,         //便利贴
    FishTank,     //鱼缸
    Doll,         //熊玩偶
    Award,        //奖状
    Beads         //串珠

}


public class InteractableItem : MonoBehaviour
{
    [Header("1. 基础设置")]
    public ItemType type; // 物品类型
    public GameObject questionMarkIcon; // 靠近时显示的问号图标
   
    [Header("2. 弹板配置 (可选)")]
    public bool hasPopup = false;   // 是否有线索大图
    public GameObject popupPanel;       // 拖入线索面板
    public string popupText = "默认线索描述内容"; // 可以在 Inspector 中设置或动态修改


    [Header("3. 对话配置 (可选)")]
    public bool hasDialogue = true; // 是否有对话
    public DialogueSession dialogueData; // 拖入或填写对话内容
    public Sprite popupSprite;
    private bool canInteract = false; // 是否在交互范围内

    void Start()
    {
        if (questionMarkIcon != null) questionMarkIcon.SetActive(false);
    }

    // === 鼠标点击逻辑 ===
    private void OnMouseDown()
    {
        // 1. 基础检查：是否被UI遮挡？是否在范围内？是否正在对话中？
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!canInteract) return;
        if (DialogueManager.instance.IsDialogueActive) return;

        Debug.Log($"点击了物品: {type}");

        // 2. 特殊处理：如果是笔记本，可能需要打开特殊的UI面板而不是普通弹板
        if (type == ItemType.NoteBook)
        {
            // 通知 GameManager 处理特殊逻辑（打开书本UI）
            GameManager.Instance.OnItemInteracted(type);
            return;
        }

        // 3. 通用流程：弹板 -> 对话 -> 结束通知
        if (hasPopup && popupPanel != null)
        {
            // 显示线索面板
            popupPanel.SetActive(true);


            // 动态设置文本
            TextMeshProUGUI textComponent = popupPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = popupText.Replace(@"\n", "\n"); // 动态内容
            }

            // 关闭按钮逻辑
            Button closeButton = popupPanel.GetComponentInChildren<Button>();
            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(() =>
                {
                    popupPanel.SetActive(false);
                    StartConversation();
                });
            }
            /*
            // 显示弹板，关闭后执行 StartConversation
            UIManager.Instance.ShowClue(popupSprite, () => {
                StartConversation();
            });*/
        }
        else
        {
            // 没有弹板，直接对话
            StartConversation();
        }
    }

    // === 处理对话 ===
    private void StartConversation()
    {
        if (hasDialogue)
        {
            // 开始对话，对话结束后调用 OnFinished
            DialogueManager.instance.StartDialogue(dialogueData, OnFinished);
        }
        else
        {
            OnFinished();
        }
    }

    // === 流程结束 ===
    private void OnFinished()
    {
        // 通知 GameManager：这个物品交互完了，请更新任务进度
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnItemInteracted(type);
        }
    }

    // === 范围检测 ===
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
            if (questionMarkIcon != null) questionMarkIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
            if (questionMarkIcon != null) questionMarkIcon.SetActive(false);
        }
    }
}
