using TMPro;

// Token: 0x0200001D RID: 29
public class GhostCycle : ItemCycle
{
    // Token: 0x060000A8 RID: 168 RVA: 0x0000568A File Offset: 0x0000388A
    private void Awake() => base.max = 3;

    // Token: 0x060000A9 RID: 169 RVA: 0x00005693 File Offset: 0x00003893
    private void Start() => UpdateText();

    // Token: 0x060000AA RID: 170 RVA: 0x0000569B File Offset: 0x0000389B
    public override void Cycle(int n)
    {
        if (mapCycle.lockUi.activeInHierarchy)
        {
            return;
        }
        base.Cycle(n);
        UpdateText();
    }

    // Token: 0x060000AB RID: 171 RVA: 0x000056C0 File Offset: 0x000038C0
    public void UpdateText()
    {
        var selected = (GhostCycle.Ghost)base.selected;
        ghost = selected;
        ghostText.text = " (" + selected + ")";
        var str = " (" + selected + ")";
        var str2 = "| ";
        if (selected == GhostCycle.Ghost.Dani)
        {
            if (mapCycle.selected == 5) str = " (Terrain)";
            str2 += Timer.GetFormattedTime(SaveManager.Instance.state.daniTimes[mapCycle.selected]);
        }
        else if (selected == GhostCycle.Ghost.PB)
        {
            str2 += Timer.GetFormattedTime(SaveManager.Instance.state.times[mapCycle.selected]);
        }
        ghostText.text = str2 + str;
        GameState.Instance.ghost = ghost;
    }

    // Token: 0x040000D4 RID: 212
    private GhostCycle.Ghost ghost;

    // Token: 0x040000D5 RID: 213
    public TextMeshProUGUI ghostText;

    // Token: 0x040000D6 RID: 214
    public MapCycle mapCycle;

    // Token: 0x02000061 RID: 97
    public enum Ghost
    {
        // Token: 0x0400023C RID: 572
        PB,
        // Token: 0x0400023D RID: 573
        Dani,
        // Token: 0x0400023E RID: 574
        Off
    }
}
