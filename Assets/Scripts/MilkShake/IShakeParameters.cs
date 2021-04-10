using System;
using UnityEngine;

namespace MilkShake
{
	// Token: 0x02000056 RID: 86
	public interface IShakeParameters
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001DA RID: 474
		// (set) Token: 0x060001DB RID: 475
		ShakeType ShakeType { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001DC RID: 476
		// (set) Token: 0x060001DD RID: 477
		float Strength { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001DE RID: 478
		// (set) Token: 0x060001DF RID: 479
		float Roughness { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001E0 RID: 480
		// (set) Token: 0x060001E1 RID: 481
		float FadeIn { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001E2 RID: 482
		// (set) Token: 0x060001E3 RID: 483
		float FadeOut { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001E4 RID: 484
		// (set) Token: 0x060001E5 RID: 485
		Vector3 PositionInfluence { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001E6 RID: 486
		// (set) Token: 0x060001E7 RID: 487
		Vector3 RotationInfluence { get; set; }
	}
}
