using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004C RID: 76
public class SliderSettingCycle : ItemCycle
{
	// Token: 0x1700002E RID: 46
	// (get) Token: 0x060001AC RID: 428 RVA: 0x00009146 File Offset: 0x00007346
	// (set) Token: 0x060001AD RID: 429 RVA: 0x0000914E File Offset: 0x0000734E
	public Image[] options { get; private set; }

	// Token: 0x060001AE RID: 430 RVA: 0x00009157 File Offset: 0x00007357
	private void Awake()
	{
		this.options = this.optionsParent.transform.GetChild(base.transform.GetSiblingIndex()).GetComponentsInChildren<Image>();
		base.max = this.options.Length;
		this.UpdateOptions();
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00009193 File Offset: 0x00007393
	public override void Cycle(int n)
	{
		base.Cycle(n);
		this.UpdateOptions();
		this.settings.UpdateSettings();
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x000091B0 File Offset: 0x000073B0
	public void UpdateOptions()
	{
		for (int i = 0; i < base.max; i++)
		{
			if (i <= base.selected)
			{
				this.options[i].color = this.selectedC;
			}
			else
			{
				this.options[i].color = this.deselectedC;
			}
		}
	}

	// Token: 0x040001B7 RID: 439
	public Color deselectedC = new Color(1f, 1f, 1f, 0.3f);

	// Token: 0x040001B8 RID: 440
	public Color selectedC = new Color(1f, 1f, 1f);

	// Token: 0x040001BA RID: 442
	public GameObject optionsParent;

	// Token: 0x040001BB RID: 443
	public SettingsUi settings;
}
