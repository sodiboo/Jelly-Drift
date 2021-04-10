using System;
using TMPro;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class CarButton : MonoBehaviour
{
	// Token: 0x0600004C RID: 76 RVA: 0x00003D84 File Offset: 0x00001F84
	private void Awake()
	{
		this.SetState(CarButton.ButtonState.Next);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00003D8D File Offset: 0x00001F8D
	public void SetState(CarButton.ButtonState state)
	{
		this.state = state;
		if (state == CarButton.ButtonState.Next)
		{
			this.text.text = "Next";
			return;
		}
		this.text.text = "Buy";
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003DBA File Offset: 0x00001FBA
	public void Use()
	{
		if (this.state == CarButton.ButtonState.Next)
		{
			this.carCycle.SaveCar();
			return;
		}
		if (this.state == CarButton.ButtonState.BuySkin)
		{
			this.skinCycle.BuySkin();
			return;
		}
		if (this.state == CarButton.ButtonState.BuyCar)
		{
			this.carCycle.BuyCar();
		}
	}

	// Token: 0x04000082 RID: 130
	private CarButton.ButtonState state;

	// Token: 0x04000083 RID: 131
	public TextMeshProUGUI text;

	// Token: 0x04000084 RID: 132
	public CarCycle carCycle;

	// Token: 0x04000085 RID: 133
	public SkinCycle skinCycle;

	// Token: 0x0200005E RID: 94
	public enum ButtonState
	{
		// Token: 0x04000233 RID: 563
		Next,
		// Token: 0x04000234 RID: 564
		BuySkin,
		// Token: 0x04000235 RID: 565
		BuyCar
	}
}
