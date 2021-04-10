using System;
using TMPro;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class SettingCycle : ItemCycle
{
	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000187 RID: 391 RVA: 0x000087F2 File Offset: 0x000069F2
	// (set) Token: 0x06000188 RID: 392 RVA: 0x000087FA File Offset: 0x000069FA
	public TextMeshProUGUI[] options { get; private set; }

	// Token: 0x06000189 RID: 393 RVA: 0x00008803 File Offset: 0x00006A03
	private void Awake()
	{
		this.options = this.optionsParent.transform.GetChild(base.transform.GetSiblingIndex()).GetComponentsInChildren<TextMeshProUGUI>();
		base.max = this.options.Length;
		this.UpdateOptions();
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000883F File Offset: 0x00006A3F
	public override void Cycle(int n)
	{
		base.Cycle(n);
		this.UpdateOptions();
		this.settings.UpdateSettings();
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000885C File Offset: 0x00006A5C
	public void UpdateOptions()
	{
		for (int i = 0; i < base.max; i++)
		{
			this.options[i].color = this.deselectedC;
			if (i == base.selected)
			{
				this.options[i].color = this.selectedC;
			}
		}
	}

	// Token: 0x0400019B RID: 411
	private Color deselectedC = new Color(1f, 1f, 1f, 0.3f);

	// Token: 0x0400019C RID: 412
	private Color selectedC = new Color(1f, 1f, 1f);

	// Token: 0x0400019E RID: 414
	public GameObject optionsParent;

	// Token: 0x0400019F RID: 415
	public SettingsUi settings;
}
