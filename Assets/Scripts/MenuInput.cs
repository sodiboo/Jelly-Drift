using UnityEngine;

// Token: 0x02000026 RID: 38
public class MenuInput : MonoBehaviour
{
    // Token: 0x060000D4 RID: 212 RVA: 0x0000606F File Offset: 0x0000426F
    private void Awake()
    {
        if (MenuInput.Instance)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            return;
        }
        MenuInput.Instance = this;
    }

    // Token: 0x060000D5 RID: 213 RVA: 0x0000608F File Offset: 0x0000428F
    private void Update() => PlayerInput();

    // Token: 0x060000D6 RID: 214 RVA: 0x000020AB File Offset: 0x000002AB
    private void PlayerInput()
    {
    }

    // Token: 0x040000F9 RID: 249
    public bool horizontalDone;

    // Token: 0x040000FA RID: 250
    public bool verticalDone;

    // Token: 0x040000FB RID: 251
    public int horizontal;

    // Token: 0x040000FC RID: 252
    public int vertical;

    // Token: 0x040000FD RID: 253
    public bool select;

    // Token: 0x040000FE RID: 254
    public static MenuInput Instance;

    // Token: 0x040000FF RID: 255
    public int wat;
}
