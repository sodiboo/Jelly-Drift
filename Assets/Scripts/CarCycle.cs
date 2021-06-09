using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000E RID: 14
public class CarCycle : ItemCycle
{
    // Token: 0x06000050 RID: 80 RVA: 0x00003DF9 File Offset: 0x00001FF9
    private void Start() => base.max = CarDisplay.Instance.nCars;

    // Token: 0x06000051 RID: 81 RVA: 0x00003E0C File Offset: 0x0000200C
    private void OnEnable()
    {
        if (CarDisplay.Instance)
        {
            var lastCar = SaveManager.Instance.state.lastCar;
            base.selected = lastCar;
            CarDisplay.Instance.SpawnCar(lastCar);
            name.text = "| " + CarDisplay.Instance.currentCar.name;
            CarDisplay.Instance.SetSkin(SaveManager.Instance.state.lastSkin[lastCar]);
            carStats.SetStats(base.selected);
            skinCycle.selected = SaveManager.Instance.state.lastSkin[lastCar];
        }
    }

    // Token: 0x06000052 RID: 82 RVA: 0x00003EB8 File Offset: 0x000020B8
    public override void Cycle(int n)
    {
        base.Cycle(n);
        skinCycle.SetCarToCycle(base.selected);
        CarDisplay.Instance.SpawnCar(base.selected);
        if (SaveManager.Instance.state.carsUnlocked[base.selected])
        {
            name.text = "| " + CarDisplay.Instance.currentCar.name;
            SaveManager.Instance.state.lastCar = base.selected;
            SaveManager.Instance.state.lastSkin[base.selected] = skinCycle.selected;
            SaveManager.Instance.Save();
            GameState.Instance.car = base.selected;
            nextBtn.enabled = true;
            skinCycle.UpdateColor();
        }
        else
        {
            MonoBehaviour.print("not unlcoked");
            var str = "???";
            if (base.selected <= 6)
            {
                str = "<size=60%>Complete " + MapManager.Instance.maps[base.selected - 1].name + " on normal difficulty";
            }
            else if (base.selected == 7)
            {
                str = "<size=60%>Complete all races on hard difficulty";
            }
            else if (base.selected == 8)
            {
                str = "<size=60%>Complete 3-star time on all maps";
            }
            name.text = "| " + str;
            nextBtn.enabled = false;
            skinCycle.text.text = "| ???";
        }
        carStats.SetStats(base.selected);
    }

    // Token: 0x06000053 RID: 83 RVA: 0x000020AB File Offset: 0x000002AB
    public void BuyCar()
    {
    }

    // Token: 0x06000054 RID: 84 RVA: 0x00004043 File Offset: 0x00002243
    public void SaveCar()
    {
        SaveManager.Instance.state.lastCar = base.selected;
        SaveManager.Instance.Save();
        GameState.Instance.car = base.selected;
        GameState.Instance.LoadMap();
    }

    // Token: 0x04000086 RID: 134
    public SkinCycle skinCycle;

    // Token: 0x04000087 RID: 135
    public new TextMeshProUGUI name;

    // Token: 0x04000088 RID: 136
    public Button nextBtn;

    // Token: 0x04000089 RID: 137
    public CarStats carStats;
}
