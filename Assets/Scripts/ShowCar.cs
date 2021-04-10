using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class ShowCar : MonoBehaviour
{
	// Token: 0x060001A1 RID: 417 RVA: 0x00008DC9 File Offset: 0x00006FC9
	private void OnEnable()
	{
		if (!CarDisplay.Instance || !CarDisplay.Instance.currentCar)
		{
			return;
		}
		if (this.show)
		{
			CarDisplay.Instance.Show();
			return;
		}
		CarDisplay.Instance.Hide();
	}

	// Token: 0x040001B0 RID: 432
	public bool show;
}
