using UnityEngine;

namespace MilkShake
{
    // Token: 0x02000059 RID: 89
    [CreateAssetMenu(fileName = "New Shake Preset", menuName = "MilkShake/Shake Preset")]
    public class ShakePreset : ScriptableObject, IShakeParameters
    {
        // Token: 0x17000043 RID: 67
        // (get) Token: 0x0600020A RID: 522 RVA: 0x0000A476 File Offset: 0x00008676
        // (set) Token: 0x0600020B RID: 523 RVA: 0x0000A47E File Offset: 0x0000867E
        public ShakeType ShakeType
        {
            get => shakeType;
            set => shakeType = value;
        }

        // Token: 0x17000044 RID: 68
        // (get) Token: 0x0600020C RID: 524 RVA: 0x0000A487 File Offset: 0x00008687
        // (set) Token: 0x0600020D RID: 525 RVA: 0x0000A48F File Offset: 0x0000868F
        public float Strength
        {
            get => strength;
            set => strength = value;
        }

        // Token: 0x17000045 RID: 69
        // (get) Token: 0x0600020E RID: 526 RVA: 0x0000A498 File Offset: 0x00008698
        // (set) Token: 0x0600020F RID: 527 RVA: 0x0000A4A0 File Offset: 0x000086A0
        public float Roughness
        {
            get => roughness;
            set => roughness = value;
        }

        // Token: 0x17000046 RID: 70
        // (get) Token: 0x06000210 RID: 528 RVA: 0x0000A4A9 File Offset: 0x000086A9
        // (set) Token: 0x06000211 RID: 529 RVA: 0x0000A4B1 File Offset: 0x000086B1
        public float FadeIn
        {
            get => fadeIn;
            set => fadeIn = value;
        }

        // Token: 0x17000047 RID: 71
        // (get) Token: 0x06000212 RID: 530 RVA: 0x0000A4BA File Offset: 0x000086BA
        // (set) Token: 0x06000213 RID: 531 RVA: 0x0000A4C2 File Offset: 0x000086C2
        public float FadeOut
        {
            get => fadeOut;
            set => fadeOut = value;
        }

        // Token: 0x17000048 RID: 72
        // (get) Token: 0x06000214 RID: 532 RVA: 0x0000A4CB File Offset: 0x000086CB
        // (set) Token: 0x06000215 RID: 533 RVA: 0x0000A4D3 File Offset: 0x000086D3
        public Vector3 PositionInfluence
        {
            get => positionInfluence;
            set => positionInfluence = value;
        }

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x06000216 RID: 534 RVA: 0x0000A4DC File Offset: 0x000086DC
        // (set) Token: 0x06000217 RID: 535 RVA: 0x0000A4E4 File Offset: 0x000086E4
        public Vector3 RotationInfluence
        {
            get => rotationInfluence;
            set => rotationInfluence = value;
        }

        // Token: 0x0400021E RID: 542
        [Header("Shake Type")]
        [SerializeField]
        private ShakeType shakeType;

        // Token: 0x0400021F RID: 543
        [Header("Shake Strength")]
        [SerializeField]
        private float strength;

        // Token: 0x04000220 RID: 544
        [SerializeField]
        private float roughness;

        // Token: 0x04000221 RID: 545
        [Header("Fade")]
        [SerializeField]
        private float fadeIn;

        // Token: 0x04000222 RID: 546
        [SerializeField]
        private float fadeOut;

        // Token: 0x04000223 RID: 547
        [Header("Shake Influence")]
        [SerializeField]
        private Vector3 positionInfluence;

        // Token: 0x04000224 RID: 548
        [SerializeField]
        private Vector3 rotationInfluence;
    }
}
