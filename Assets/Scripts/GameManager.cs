using UnityEngine;


public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    public DialogueManager dialogueManager;
    public SceneTransitionManager sceneTransitionManager;
    public TaskManager taskManager;
    public InteractableItem interactableItem;

    public Sprite Playerportrait1;//切换不同立绘表情
    private bool allInteractionsCompleted = false;

    private bool isDiaryInteracted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 播放坐起动画
      // indObjectOfType<PlayerAnimationController>().PlaySitUpAnimation();

        // 开场对白
        StartDialogueSequence();
    }

    //开场对白----------------------------------------------------------------------
    private void StartDialogueSequence()
    {
        var openingDialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{speakerName ="主控", text ="又是新的一天开始了。"},
                new DialogueLine{speakerName ="主控", text ="或许连你自己还没有意识到，这一天多么不同。"},
                new DialogueLine{speakerName ="主控", text ="我小时候的房间？...我好像还变小了。", portrait = Playerportrait1 }
            }
        };

        dialogueManager.StartDialogue(openingDialogue, OnDialogueFinished);
    }
    private void OnDialogueFinished()
    {
        Debug.Log("旁白结束");
    }


    //交互逻辑------------------------------------------------------------------------------
    public void OnItemInteracted(ItemType itemType)
    {
        // 修改：日记未收集前阻止其他交互
        if (!taskManager.IsNoteViewed() &&
        (itemType == ItemType.FishTank || itemType == ItemType.Doll || itemType == ItemType.Award))
        {
            Debug.Log("请先查看便利贴再操作其他密码物品！");
            return;
        }

        switch (itemType)
        {
            case ItemType.NoteBook:
                // 打开密码本界面，假设玩家看完便利贴后再点击物品
                Debug.Log("点击密码本！");
                break;

            case ItemType.Note:
                if (!taskManager.IsNoteViewed())
                {
                    Debug.Log("第一次查看便利贴！");
                    taskManager.ViewNote();
                    // 可在这里触发 Note 第一次查看对话
                }
                else
                {
                    Debug.Log("便利贴已查看，第二次及以后点击不再弹出对话");
                }
                break;

            case ItemType.Bed:
                Debug.Log("点击床，检查是否可推进剧情");
                if (taskManager.AreAllTasksCompleted())
                {
                    Debug.Log("所有任务完成，触发床剧情。");
                    // 播放 DH_003.MP4（可使用 VideoPlayer）
                    // 然后跳转 script5
                    /*sceneTransitionManager.SetNextScene("script5");
                    sceneTransitionManager.TransitionToNextScene();*///暂时未设置视频接口
                }
                break;

            case ItemType.FishTank:
                Debug.Log("触发了鱼缸的交互逻辑！");
                break;

            case ItemType.Doll:
                Debug.Log("触发了熊玩偶的交互逻辑！");
                break;

            case ItemType.Award:
                Debug.Log($"收集密码物品：{itemType}");
                taskManager.CollectPassword(itemType);
                break;

            case ItemType.Beads:
                Debug.Log("触发了串珠的交互逻辑！");
                break;

            default:
                Debug.LogWarning($"未处理的物品类型: {itemType}");
                break;
        }

        // 检查任务是否完成
        CheckTasks();
    }
    private void CheckTasks()
    {
        if (taskManager.AreAllTasksCompleted())
        {
            Debug.Log("所有任务完成！准备切换到下一场景。");

            // 设置下一场景并触发场景切换
            sceneTransitionManager.SetNextScene("NextSceneName"); // 替换为你的场景名称
            sceneTransitionManager.TransitionToNextScene();
        }
    }

        public void CheckInteractions()
    {
        if (AllInteractionsCompleted())
        {
            allInteractionsCompleted = true;

            
            sceneTransitionManager.TransitionToNextScene();
        }
    }

    private bool AllInteractionsCompleted()
    {
         return taskManager.AreAllTasksCompleted();
    }
}
