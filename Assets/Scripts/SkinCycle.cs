using System;
using TMPro;

// Token: 0x0200004B RID: 75
public class SkinCycle : ItemCycle
{
	// Token: 0x060001A5 RID: 421 RVA: 0x00008E29 File Offset: 0x00007029
	private void Start()
	{
		base.max = SaveManager.Instance.state.skins[this.carCycle.selected].Length;
		this.UpdateColor();
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00008E54 File Offset: 0x00007054
	public void SetCarToCycle(int n)
	{
		base.selected = SaveManager.Instance.state.lastSkin[n];
		base.max = SaveManager.Instance.state.skins[n].Length;
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00008E86 File Offset: 0x00007086
	public override void Cycle(int n)
	{
		base.Cycle(n);
		this.UpdateColor();
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00008E98 File Offset: 0x00007098
	public void UpdateColor()
	{
		if (!SaveManager.Instance.state.carsUnlocked[this.carCycle.selected])
		{
			return;
		}
		CarDisplay.Instance.SetSkin(base.selected);
		int num = 0;
		if (SaveManager.Instance.state.skins[this.carCycle.selected][base.selected])
		{
			num = base.selected;
			SaveManager.Instance.state.lastSkin[this.carCycle.selected] = num;
			SaveManager.Instance.Save();
		}
		GameState.Instance.skin = num;
		this.UpdateText(num == base.selected);
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00008F40 File Offset: 0x00007140
	public void UpdateText(bool unlocked)
	{
		int selected = this.carCycle.selected;
		int selected2 = base.selected;
		this.carBtn.SetState(CarButton.ButtonState.Next);
		string text = "<size=60%>";
		if (!SaveManager.Instance.state.skins[selected][selected2])
		{
			if (selected < 5)
			{
				if (selected2 == 1)
				{
					text = text + "Complete " + MapManager.Instance.maps[selected].name + " on hard difficulty";
				}
				else if (selected2 == 2)
				{
					text = text + "Complete " + MapManager.Instance.maps[selected].name + " 3-star time";
				}
				else
				{
					this.carBtn.SetState(CarButton.ButtonState.BuySkin);
					int skinPrice = PlayerSave.GetSkinPrice(selected, selected2);
					text = string.Concat(new object[]
					{
						text,
						"<size=80%><font=\"Ubuntu-Bold SDF\">Buy (",
						skinPrice,
						"$)"
					});
				}
			}
			if (selected == 5)
			{
				text += "Beat the ghost of Dani on all maps..";
			}
		}
		else
		{
			text = CarDisplay.Instance.currentCar.GetComponent<CarSkin>().GetSkinName(base.selected);
		}
		this.text.text = "| " + text;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x00009064 File Offset: 0x00007264
	public void BuySkin()
	{
		int skinPrice = PlayerSave.GetSkinPrice(this.carCycle.selected, base.selected);
		if (SaveManager.Instance.state.money >= skinPrice)
		{
			SaveManager.Instance.state.money -= skinPrice;
			SaveManager.Instance.state.skins[this.carCycle.selected][base.selected] = true;
			SaveManager.Instance.Save();
			this.UpdateText(true);
			this.unlockManager.unlocks.Add(new UnlockManager.Unlock(UnlockManager.UnlockType.Skin, this.carCycle.selected, base.selected));
			this.unlockManager.gameObject.SetActive(true);
			this.menuStats.UpdateStats();
			this.UpdateColor();
			SoundManager.Instance.PlayMoney();
			return;
		}
		SoundManager.Instance.PlayError();
	}

	// Token: 0x040001B2 RID: 434
	public TextMeshProUGUI text;

	// Token: 0x040001B3 RID: 435
	public CarCycle carCycle;

	// Token: 0x040001B4 RID: 436
	public CarButton carBtn;

	// Token: 0x040001B5 RID: 437
	public MenuStats menuStats;

	// Token: 0x040001B6 RID: 438
	public UnlockManager unlockManager;
}
