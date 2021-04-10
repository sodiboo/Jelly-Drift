using System;
using UnityEngine.UI;

// Token: 0x02000008 RID: 8
public class ButtonCycle : ItemCycle
{
	// Token: 0x0600001B RID: 27 RVA: 0x00002636 File Offset: 0x00000836
	private void Awake()
	{
		this.btn = base.GetComponent<Button>();
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002644 File Offset: 0x00000844
	public override void Cycle(int n)
	{
		if (!this.btn.enabled)
		{
			return;
		}
		this.btn.onClick.Invoke();
	}

	// Token: 0x04000026 RID: 38
	private Button btn;
}
