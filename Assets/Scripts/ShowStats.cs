using UnityEngine;

// Token: 0x0200004A RID: 74
public class ShowStats : MonoBehaviour
{
    // Token: 0x060001A3 RID: 419 RVA: 0x00008E06 File Offset: 0x00007006
    private void OnEnable()
    {
        if (MenuStats.Instance)
        {
            MenuStats.Instance.gameObject.SetActive(show);
        }
    }

    // Token: 0x040001B1 RID: 433
    public bool show;
}
