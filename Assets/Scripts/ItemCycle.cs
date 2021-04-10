using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class ItemCycle : MonoBehaviour
{
	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000590D File Offset: 0x00003B0D
	// (set) Token: 0x060000B7 RID: 183 RVA: 0x00005915 File Offset: 0x00003B15
	public int selected { get; set; }

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000591E File Offset: 0x00003B1E
	// (set) Token: 0x060000B9 RID: 185 RVA: 0x00005926 File Offset: 0x00003B26
	public int max { get; set; }

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060000BA RID: 186 RVA: 0x0000592F File Offset: 0x00003B2F
	// (set) Token: 0x060000BB RID: 187 RVA: 0x00005937 File Offset: 0x00003B37
	public bool activeCycle { get; set; } = true;

	// Token: 0x060000BC RID: 188 RVA: 0x00005940 File Offset: 0x00003B40
	public virtual void Cycle(int n)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.selected += n;
		if (this.selected >= this.max)
		{
			this.selected = 0;
		}
		if (this.selected < 0)
		{
			this.selected = this.max - 1;
		}
	}
}
