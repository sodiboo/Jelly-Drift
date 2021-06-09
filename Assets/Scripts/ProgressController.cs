using TMPro;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class ProgressController : MonoBehaviour
{
    // Token: 0x06000107 RID: 263 RVA: 0x0000666B File Offset: 0x0000486B
    private void Awake() => defaultLevelScale = level.transform.localScale;

    // Token: 0x06000108 RID: 264 RVA: 0x00006684 File Offset: 0x00004884
    public void SetProgress(int oldXp, int newXp, int oldLevel, int oldMoney, int newMoney)
    {
        xp = oldXp;
        desiredXp = newXp;
        currentLevel = oldLevel;
        currentMoney = oldMoney;
        desiredMoney = newMoney;
        level.text = "Lvl " + oldLevel;
        money.text = "$" + oldMoney;
        xpGained.text = "+ " + (newXp - oldXp) + "xp";
        moneyGained.text = "+ " + (newMoney - oldMoney) + "$";
        var num = (float)((int)xp - SaveManager.Instance.state.XpForLevel(currentLevel));
        var num2 = SaveManager.Instance.state.XpForLevel(currentLevel + 1) - SaveManager.Instance.state.XpForLevel(currentLevel);
        var x = num / num2;
        progress.localScale = new Vector3(x, 1f, 1f);
        base.Invoke(nameof(GetReady), 0.5f);
    }

    // Token: 0x06000109 RID: 265 RVA: 0x000067B1 File Offset: 0x000049B1
    private void GetReady() => ready = true;

    // Token: 0x0600010A RID: 266 RVA: 0x000067BC File Offset: 0x000049BC
    private void Update()
    {
        if (!ready || (UnlockManager.Instance && UnlockManager.Instance.gameObject.activeInHierarchy))
        {
            return;
        }
        xp = Mathf.Lerp(xp, desiredXp, Time.deltaTime * 0.5f);
        currentMoney = Mathf.Lerp(currentMoney, desiredMoney, Time.deltaTime * 0.5f);
        var num = (float)((int)xp - SaveManager.Instance.state.XpForLevel(currentLevel));
        var num2 = SaveManager.Instance.state.XpForLevel(currentLevel + 1) - SaveManager.Instance.state.XpForLevel(currentLevel);
        var x = num / num2;
        progress.localScale = new Vector3(x, 1f, 1f);
        money.text = "$" + Mathf.CeilToInt(currentMoney);
        if (SaveManager.Instance.state.GetLevel((int)xp) > currentLevel)
        {
            MonoBehaviour.print("levelled up!");
            level.transform.localScale = defaultLevelScale * 1.5f;
            currentLevel++;
            level.text = "Lvl " + currentLevel;
        }
        ScaleLevel();
    }

    // Token: 0x0600010B RID: 267 RVA: 0x00006940 File Offset: 0x00004B40
    private void ScaleLevel() => level.transform.localScale = Vector3.Lerp(level.transform.localScale, defaultLevelScale, Time.deltaTime * 1f);

    // Token: 0x0400011E RID: 286
    public Transform progress;

    // Token: 0x0400011F RID: 287
    public TextMeshProUGUI level;

    // Token: 0x04000120 RID: 288
    public TextMeshProUGUI money;

    // Token: 0x04000121 RID: 289
    public TextMeshProUGUI xpGained;

    // Token: 0x04000122 RID: 290
    public TextMeshProUGUI moneyGained;

    // Token: 0x04000123 RID: 291
    private float currentMoney;

    // Token: 0x04000124 RID: 292
    private float desiredMoney;

    // Token: 0x04000125 RID: 293
    private float xp;

    // Token: 0x04000126 RID: 294
    private int desiredXp;

    // Token: 0x04000127 RID: 295
    private int currentLevel;

    // Token: 0x04000128 RID: 296
    private bool ready;

    // Token: 0x04000129 RID: 297
    private Vector3 defaultLevelScale;
}
