using TMPro;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class DifficultyCycle : ItemCycle
{
    // Token: 0x0600007B RID: 123 RVA: 0x000049D1 File Offset: 0x00002BD1
    private void Awake()
    {
        base.max = 3;
        base.selected = SaveManager.Instance.state.lastDifficulty;
        MonoBehaviour.print("loaded selected: " + base.selected);
    }

    // Token: 0x0600007C RID: 124 RVA: 0x00004A09 File Offset: 0x00002C09
    private void Start()
    {
        UpdateText();
        MonoBehaviour.print("in start method: " + base.selected);
    }

    // Token: 0x0600007D RID: 125 RVA: 0x00004A2B File Offset: 0x00002C2B
    public override void Cycle(int n)
    {
        if (mapCycle.lockUi.activeInHierarchy)
        {
            return;
        }
        base.Cycle(n);
        UpdateText();
    }

    // Token: 0x0600007E RID: 126 RVA: 0x00004A50 File Offset: 0x00002C50
    public void UpdateText()
    {
        var selected = (DifficultyCycle.Difficulty)base.selected;
        ghostText.text = "| " + selected;
        GameState.Instance.difficulty = selected;
        SaveManager.Instance.state.lastDifficulty = base.selected;
        SaveManager.Instance.Save();
        MonoBehaviour.print("saved last difficulty as:  " + base.selected);
    }

    // Token: 0x0600007F RID: 127 RVA: 0x00004AC4 File Offset: 0x00002CC4
    public void UpdateTextOnly()
    {
        var selected = (DifficultyCycle.Difficulty)base.selected;
        ghostText.text = "| " + selected;
    }

    // Token: 0x040000B1 RID: 177
    public TextMeshProUGUI ghostText;

    // Token: 0x040000B2 RID: 178
    public MapCycle mapCycle;

    // Token: 0x02000060 RID: 96
    public enum Difficulty
    {
        // Token: 0x04000238 RID: 568
        Easy,
        // Token: 0x04000239 RID: 569
        Normal,
        // Token: 0x0400023A RID: 570
        Hard
    }
}
