using System;
using UnityEngine;

namespace MilkShake
{
	// Token: 0x02000057 RID: 87
	[Serializable]
	public class ShakeInstance
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00009EDE File Offset: 0x000080DE
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x00009EE6 File Offset: 0x000080E6
		public ShakeState State { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00009EEF File Offset: 0x000080EF
		// (set) Token: 0x060001EB RID: 491 RVA: 0x00009EF7 File Offset: 0x000080F7
		public bool IsPaused { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00009F00 File Offset: 0x00008100
		public bool IsFinished
		{
			get
			{
				return this.State == ShakeState.Stopped && this.RemoveWhenStopped;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00009F13 File Offset: 0x00008113
		public float CurrentStrength
		{
			get
			{
				return this.ShakeParameters.Strength * this.fadeTimer * this.StrengthScale;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00009F2E File Offset: 0x0000812E
		public float CurrentRoughness
		{
			get
			{
				return this.ShakeParameters.Roughness * this.fadeTimer * this.RoughnessScale;
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009F4C File Offset: 0x0000814C
		public ShakeInstance(int? seed = null)
		{
			if (seed == null)
			{
				seed = new int?(UnityEngine.Random.Range(-10000, 10000));
			}
			this.baseSeed = seed.Value;
			this.seed1 = (float)this.baseSeed / 2f;
			this.seed2 = (float)this.baseSeed / 3f;
			this.seed3 = (float)this.baseSeed / 4f;
			this.noiseTimer = (float)this.baseSeed;
			this.fadeTimer = 0f;
			this.pauseTimer = 0f;
			this.StrengthScale = 1f;
			this.RoughnessScale = 1f;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009FFD File Offset: 0x000081FD
		public ShakeInstance(IShakeParameters shakeData, int? seed = null) : this(seed)
		{
			this.ShakeParameters = new ShakeParameters(shakeData);
			this.fadeInTime = shakeData.FadeIn;
			this.fadeOutTime = shakeData.FadeOut;
			this.State = ShakeState.FadingIn;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000A034 File Offset: 0x00008234
		public ShakeResult UpdateShake(float deltaTime)
		{
			ShakeResult result = default(ShakeResult);
			result.PositionShake = this.getPositionShake();
			result.RotationShake = this.getRotationShake();
			if (Time.frameCount == this.lastUpdatedFrame)
			{
				return result;
			}
			if (this.pauseFadeTime > 0f)
			{
				if (this.IsPaused)
				{
					this.pauseTimer += deltaTime / this.pauseFadeTime;
				}
				else
				{
					this.pauseTimer -= deltaTime / this.pauseFadeTime;
				}
			}
			this.pauseTimer = Mathf.Clamp01(this.pauseTimer);
			this.noiseTimer += (1f - this.pauseTimer) * deltaTime * this.CurrentRoughness;
			if (this.State == ShakeState.FadingIn)
			{
				if (this.fadeInTime > 0f)
				{
					this.fadeTimer += deltaTime / this.fadeInTime;
				}
				else
				{
					this.fadeTimer = 1f;
				}
			}
			else if (this.State == ShakeState.FadingOut)
			{
				if (this.fadeOutTime > 0f)
				{
					this.fadeTimer -= deltaTime / this.fadeOutTime;
				}
				else
				{
					this.fadeTimer = 0f;
				}
			}
			this.fadeTimer = Mathf.Clamp01(this.fadeTimer);
			if (this.fadeTimer == 1f)
			{
				if (this.ShakeParameters.ShakeType == ShakeType.Sustained)
				{
					this.State = ShakeState.Sustained;
				}
				else if (this.ShakeParameters.ShakeType == ShakeType.OneShot)
				{
					this.Stop(this.ShakeParameters.FadeOut, true);
				}
			}
			else if (this.fadeTimer == 0f)
			{
				this.State = ShakeState.Stopped;
			}
			this.lastUpdatedFrame = Time.frameCount;
			return result;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000A1CD File Offset: 0x000083CD
		public void Start(float fadeTime)
		{
			this.fadeInTime = fadeTime;
			this.State = ShakeState.FadingIn;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A1DD File Offset: 0x000083DD
		public void Stop(float fadeTime, bool removeWhenStopped)
		{
			this.fadeOutTime = fadeTime;
			this.RemoveWhenStopped = removeWhenStopped;
			this.State = ShakeState.FadingOut;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000A1F4 File Offset: 0x000083F4
		public void Pause(float fadeTime)
		{
			this.IsPaused = true;
			this.pauseFadeTime = fadeTime;
			if (fadeTime <= 0f)
			{
				this.pauseTimer = 1f;
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A217 File Offset: 0x00008417
		public void Resume(float fadeTime)
		{
			this.IsPaused = false;
			this.pauseFadeTime = fadeTime;
			if (fadeTime <= 0f)
			{
				this.pauseTimer = 0f;
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000A23A File Offset: 0x0000843A
		public void TogglePaused(float fadeTime)
		{
			if (this.IsPaused)
			{
				this.Resume(fadeTime);
				return;
			}
			this.Pause(fadeTime);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A254 File Offset: 0x00008454
		private Vector3 getPositionShake()
		{
			Vector3 zero = Vector3.zero;
			zero.x = this.getNoise(this.noiseTimer + this.seed1, (float)this.baseSeed);
			zero.y = this.getNoise((float)this.baseSeed, this.noiseTimer);
			zero.z = this.getNoise(this.seed3 + this.noiseTimer, (float)this.baseSeed + this.noiseTimer);
			return Vector3.Scale(zero * this.CurrentStrength, this.ShakeParameters.PositionInfluence);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A2E8 File Offset: 0x000084E8
		private Vector3 getRotationShake()
		{
			Vector3 zero = Vector3.zero;
			zero.x = this.getNoise(this.noiseTimer - (float)this.baseSeed, this.seed3);
			zero.y = this.getNoise((float)this.baseSeed, this.noiseTimer + this.seed2);
			zero.z = this.getNoise((float)this.baseSeed + this.noiseTimer, this.seed1 + this.noiseTimer);
			return Vector3.Scale(zero * this.CurrentStrength, this.ShakeParameters.RotationInfluence);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000A381 File Offset: 0x00008581
		private float getNoise(float x, float y)
		{
			return (Mathf.PerlinNoise(x, y) - 0.5f) * 2f;
		}

		// Token: 0x04000206 RID: 518
		public ShakeParameters ShakeParameters;

		// Token: 0x04000207 RID: 519
		public float StrengthScale;

		// Token: 0x04000208 RID: 520
		public float RoughnessScale;

		// Token: 0x04000209 RID: 521
		public bool RemoveWhenStopped;

		// Token: 0x0400020C RID: 524
		private int baseSeed;

		// Token: 0x0400020D RID: 525
		private float seed1;

		// Token: 0x0400020E RID: 526
		private float seed2;

		// Token: 0x0400020F RID: 527
		private float seed3;

		// Token: 0x04000210 RID: 528
		private float noiseTimer;

		// Token: 0x04000211 RID: 529
		private float fadeTimer;

		// Token: 0x04000212 RID: 530
		private float fadeInTime;

		// Token: 0x04000213 RID: 531
		private float fadeOutTime;

		// Token: 0x04000214 RID: 532
		private float pauseTimer;

		// Token: 0x04000215 RID: 533
		private float pauseFadeTime;

		// Token: 0x04000216 RID: 534
		private int lastUpdatedFrame;
	}
}
