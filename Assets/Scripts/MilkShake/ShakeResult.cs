using UnityEngine;

namespace MilkShake
{
    // Token: 0x0200005A RID: 90
    public struct ShakeResult
    {
        // Token: 0x06000219 RID: 537 RVA: 0x0000A4F8 File Offset: 0x000086F8
        public static ShakeResult operator +(ShakeResult a, ShakeResult b)
        {
            return new ShakeResult
            {
                PositionShake = a.PositionShake + b.PositionShake,
                RotationShake = a.RotationShake + b.RotationShake
            };
        }

        // Token: 0x04000225 RID: 549
        public Vector3 PositionShake;

        // Token: 0x04000226 RID: 550
        public Vector3 RotationShake;
    }
}
