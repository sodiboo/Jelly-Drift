using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class MobileControls : MonoBehaviour
{
	// Token: 0x060000E2 RID: 226 RVA: 0x000061FC File Offset: 0x000043FC
	private void Start()
	{
		if (SystemInfo.deviceType != DeviceType.Handheld) 
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00006238 File Offset: 0x00004438
	private void Update()
	{
		if (!GameController.Instance.playing)
		{
			return;
		}
		float steering = 0f;
		float num = 0f;
		if (this.left.value > 0)
		{
			steering = -1f;
		}
		if (this.right.value > 0)
		{
			steering = 1f;
		}
		if (this.throttle.value > 0)
		{
			num = 1f;
		}
		if (this.breakPedal.value > 0)
		{
			num = -1f;
		}
		this.car.steering = steering;
		this.car.throttle = num;
	}

	// Token: 0x04000106 RID: 262
	public MyButton left;

	// Token: 0x04000107 RID: 263
	public MyButton right;

	// Token: 0x04000108 RID: 264
	public MyButton throttle;

	// Token: 0x04000109 RID: 265
	public MyButton breakPedal;

	// Token: 0x0400010A RID: 266
	private Car car { get => ChaosController.Instance.car; }
}
