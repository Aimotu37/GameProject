using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [Header("鼠标图标设置")]
    public Texture2D searchCursor; // 搜寻图标（放大镜）
    public Vector2 hotspot = Vector2.zero; // 图标的点击中心点（比如放大镜的中心）

    void Awake()
    {
        Instance = this;
    }

    // 变更为搜寻图标
    public void SetSearchCursor()
    {
        // CursorMode.Auto 让系统自动处理硬件加速
        Cursor.SetCursor(searchCursor, hotspot, CursorMode.Auto);
    }

    // 恢复默认箭头
    public void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
