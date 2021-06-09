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
        public bool IsFinished => State == ShakeState.Stopped && RemoveWhenStopped;

        // Token: 0x1700003A RID: 58
        // (get) Token: 0x060001ED RID: 493 RVA: 0x00009F13 File Offset: 0x00008113
        public float CurrentStrength => ShakeParameters.Strength * fadeTimer * StrengthScale;

        // Token: 0x1700003B RID: 59
        // (get) Token: 0x060001EE RID: 494 RVA: 0x00009F2E File Offset: 0x0000812E
        public float CurrentRoughness => ShakeParameters.Roughness * fadeTimer * RoughnessScale;

        // Token: 0x060001EF RID: 495 RVA: 0x00009F4C File Offset: 0x0000814C
        public ShakeInstance(int? seed = null)
        {
            if (seed == null)
            {
                seed = new int?(UnityEngine.Random.Range(-10000, 10000));
            }
            baseSeed = seed.Value;
            seed1 = baseSeed / 2f;
            seed2 = baseSeed / 3f;
            seed3 = baseSeed / 4f;
            noiseTimer = baseSeed;
            fadeTimer = 0f;
            pauseTimer = 0f;
            StrengthScale = 1f;
            RoughnessScale = 1f;
        }

        // Token: 0x060001F0 RID: 496 RVA: 0x00009FFD File Offset: 0x000081FD
        public ShakeInstance(IShakeParameters shakeData, int? seed = null) : this(seed)
        {
            ShakeParameters = new ShakeParameters(shakeData);
            fadeInTime = shakeData.FadeIn;
            fadeOutTime = shakeData.FadeOut;
            State = ShakeState.FadingIn;
        }

        // Token: 0x060001F1 RID: 497 RVA: 0x0000A034 File Offset: 0x00008234
        public ShakeResult UpdateShake(float deltaTime)
        {
            var result = default(ShakeResult);
            result.PositionShake = getPositionShake();
            result.RotationShake = getRotationShake();
            if (Time.frameCount == lastUpdatedFrame)
            {
                return result;
            }
            if (pauseFadeTime > 0f)
            {
                if (IsPaused)
                {
                    pauseTimer += deltaTime / pauseFadeTime;
                }
                else
                {
                    pauseTimer -= deltaTime / pauseFadeTime;
                }
            }
            pauseTimer = Mathf.Clamp01(pauseTimer);
            noiseTimer += (1f - pauseTimer) * deltaTime * CurrentRoughness;
            if (State == ShakeState.FadingIn)
            {
                if (fadeInTime > 0f)
                {
                    fadeTimer += deltaTime / fadeInTime;
                }
                else
                {
                    fadeTimer = 1f;
                }
            }
            else if (State == ShakeState.FadingOut)
            {
                if (fadeOutTime > 0f)
                {
                    fadeTimer -= deltaTime / fadeOutTime;
                }
                else
                {
                    fadeTimer = 0f;
                }
            }
            fadeTimer = Mathf.Clamp01(fadeTimer);
            if (fadeTimer == 1f)
            {
                if (ShakeParameters.ShakeType == ShakeType.Sustained)
                {
                    State = ShakeState.Sustained;
                }
                else if (ShakeParameters.ShakeType == ShakeType.OneShot)
                {
                    Stop(ShakeParameters.FadeOut, true);
                }
            }
            else if (fadeTimer == 0f)
            {
                State = ShakeState.Stopped;
            }
            lastUpdatedFrame = Time.frameCount;
            return result;
        }

        // Token: 0x060001F2 RID: 498 RVA: 0x0000A1CD File Offset: 0x000083CD
        public void Start(float fadeTime)
        {
            fadeInTime = fadeTime;
            State = ShakeState.FadingIn;
        }

        // Token: 0x060001F3 RID: 499 RVA: 0x0000A1DD File Offset: 0x000083DD
        public void Stop(float fadeTime, bool removeWhenStopped)
        {
            fadeOutTime = fadeTime;
            RemoveWhenStopped = removeWhenStopped;
            State = ShakeState.FadingOut;
        }

        // Token: 0x060001F4 RID: 500 RVA: 0x0000A1F4 File Offset: 0x000083F4
        public void Pause(float fadeTime)
        {
            IsPaused = true;
            pauseFadeTime = fadeTime;
            if (fadeTime <= 0f)
            {
                pauseTimer = 1f;
            }
        }

        // Token: 0x060001F5 RID: 501 RVA: 0x0000A217 File Offset: 0x00008417
        public void Resume(float fadeTime)
        {
            IsPaused = false;
            pauseFadeTime = fadeTime;
            if (fadeTime <= 0f)
            {
                pauseTimer = 0f;
            }
        }

        // Token: 0x060001F6 RID: 502 RVA: 0x0000A23A File Offset: 0x0000843A
        public void TogglePaused(float fadeTime)
        {
            if (IsPaused)
            {
                Resume(fadeTime);
                return;
            }
            Pause(fadeTime);
        }

        // Token: 0x060001F7 RID: 503 RVA: 0x0000A254 File Offset: 0x00008454
        private Vector3 getPositionShake()
        {
            var zero = Vector3.zero;
            zero.x = getNoise(noiseTimer + seed1, baseSeed);
            zero.y = getNoise(baseSeed, noiseTimer);
            zero.z = getNoise(seed3 + noiseTimer, baseSeed + noiseTimer);
            return Vector3.Scale(zero * CurrentStrength, ShakeParameters.PositionInfluence);
        }

        // Token: 0x060001F8 RID: 504 RVA: 0x0000A2E8 File Offset: 0x000084E8
        private Vector3 getRotationShake()
        {
            var zero = Vector3.zero;
            zero.x = getNoise(noiseTimer - baseSeed, seed3);
            zero.y = getNoise(baseSeed, noiseTimer + seed2);
            zero.z = getNoise(baseSeed + noiseTimer, seed1 + noiseTimer);
            return Vector3.Scale(zero * CurrentStrength, ShakeParameters.RotationInfluence);
        }

        // Token: 0x060001F9 RID: 505 RVA: 0x0000A381 File Offset: 0x00008581
        private float getNoise(float x, float y) => (Mathf.PerlinNoise(x, y) - 0.5f) * 2f;

        // Token: 0x04000206 RID: 518
        public ShakeParameters ShakeParameters;

        // Token: 0x04000207 RID: 519
        public float StrengthScale;

        // Token: 0x04000208 RID: 520
        public float RoughnessScale;

        // Token: 0x04000209 RID: 521
        public bool RemoveWhenStopped;

        // Token: 0x0400020C RID: 524
        private readonly int baseSeed;

        // Token: 0x0400020D RID: 525
        private readonly float seed1;

        // Token: 0x0400020E RID: 526
        private readonly float seed2;

        // Token: 0x0400020F RID: 527
        private readonly float seed3;

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
