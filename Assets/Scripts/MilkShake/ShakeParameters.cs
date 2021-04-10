using System;
using UnityEngine;

namespace MilkShake
{
	// Token: 0x02000058 RID: 88
	[Serializable]
	public class ShakeParameters : IShakeParameters
	{
		// Token: 0x060001FA RID: 506 RVA: 0x00009D8F File Offset: 0x00007F8F
		public ShakeParameters()
		{
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000A398 File Offset: 0x00008598
		public ShakeParameters(IShakeParameters original)
		{
			this.shakeType = original.ShakeType;
			this.strength = original.Strength;
			this.roughness = original.Roughness;
			this.fadeIn = original.FadeIn;
			this.fadeOut = original.FadeOut;
			this.positionInfluence = original.PositionInfluence;
			this.rotationInfluence = original.RotationInfluence;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000A3FF File Offset: 0x000085FF
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000A407 File Offset: 0x00008607
		public ShakeType ShakeType
		{
			get
			{
				return this.shakeType;
			}
			set
			{
				this.shakeType = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000A410 File Offset: 0x00008610
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000A418 File Offset: 0x00008618
		public float Strength
		{
			get
			{
				return this.strength;
			}
			set
			{
				this.strength = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000A421 File Offset: 0x00008621
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000A429 File Offset: 0x00008629
		public float Roughness
		{
			get
			{
				return this.roughness;
			}
			set
			{
				this.roughness = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000A432 File Offset: 0x00008632
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000A43A File Offset: 0x0000863A
		public float FadeIn
		{
			get
			{
				return this.fadeIn;
			}
			set
			{
				this.fadeIn = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000A443 File Offset: 0x00008643
		// (set) Token: 0x06000205 RID: 517 RVA: 0x0000A44B File Offset: 0x0000864B
		public float FadeOut
		{
			get
			{
				return this.fadeOut;
			}
			set
			{
				this.fadeOut = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000A454 File Offset: 0x00008654
		// (set) Token: 0x06000207 RID: 519 RVA: 0x0000A45C File Offset: 0x0000865C
		public Vector3 PositionInfluence
		{
			get
			{
				return this.positionInfluence;
			}
			set
			{
				this.positionInfluence = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000A465 File Offset: 0x00008665
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000A46D File Offset: 0x0000866D
		public Vector3 RotationInfluence
		{
			get
			{
				return this.rotationInfluence;
			}
			set
			{
				this.rotationInfluence = value;
			}
		}

		// Token: 0x04000217 RID: 535
		[Header("Shake Type")]
		[SerializeField]
		private ShakeType shakeType;

		// Token: 0x04000218 RID: 536
		[Header("Shake Strength")]
		[SerializeField]
		private float strength;

		// Token: 0x04000219 RID: 537
		[SerializeField]
		private float roughness;

		// Token: 0x0400021A RID: 538
		[Header("Fade")]
		[SerializeField]
		private float fadeIn;

		// Token: 0x0400021B RID: 539
		[SerializeField]
		private float fadeOut;

		// Token: 0x0400021C RID: 540
		[Header("Shake Influence")]
		[SerializeField]
		private Vector3 positionInfluence;

		// Token: 0x0400021D RID: 541
		[SerializeField]
		private Vector3 rotationInfluence;
	}
}
