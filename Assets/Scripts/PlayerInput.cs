using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class PlayerInput : MonoBehaviour
{
	// Token: 0x06000102 RID: 258 RVA: 0x000065D0 File Offset: 0x000047D0
	private void GetPlayerInput()
	{
		this.car.steering = Input.GetAxisRaw("Horizontal");
		this.car.throttle = Input.GetAxis("Vertical");
		this.car.breaking = Input.GetButton("Breaking");
	}

	// Token: 0x06000103 RID: 259 RVA: 0x0000661C File Offset: 0x0000481C
	private void Update()
	{
		if (GameController.Instance && !GameController.Instance.playing)
		{
			return;
		}
		this.GetPlayerInput();
	}

	// Token: 0x04000118 RID: 280
	public Car car;
}
