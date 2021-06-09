using System;

namespace Assets.Scripts
{
    // Token: 0x02000055 RID: 85
    public class Gear
    {
        // Token: 0x060001D7 RID: 471 RVA: 0x00009CC4 File Offset: 0x00007EC4
        public static int LookupTorqueCurve(int rpm)
        {
            if (rpm > 6000)
            {
                return 280;
            }
            for (var i = 0; i < Gear.rpmTorque.Length; i++)
            {
                if (i >= Gear.rpmTorque.Length)
                {
                    return Gear.rpmTorque[Gear.rpmTorque.Length].Item2;
                }
                var num = (float)Gear.rpmTorque[i].Item1;
                var num2 = (float)Gear.rpmTorque[i + 1].Item1;
                if (rpm >= num && rpm < num2)
                {
                    var num3 = (float)Gear.rpmTorque[i].Item2;
                    var num4 = rpmTorque[i + 1].Item2 - num3;
                    var num5 = 1f - (num2 - rpm) / (num2 - num);
                    return (int)(num3 + num4 * num5);
                }
            }
            return 290;
        }

        // Token: 0x040001FD RID: 509
        public static float g1 = 2.66f;

        // Token: 0x040001FE RID: 510
        public static float g2 = 1.78f;

        // Token: 0x040001FF RID: 511
        public static float g3 = 1.3f;

        // Token: 0x04000200 RID: 512
        public static float g4 = 1f;

        // Token: 0x04000201 RID: 513
        public static float g5 = 0.74f;

        // Token: 0x04000202 RID: 514
        public static float g6 = 0.5f;

        // Token: 0x04000203 RID: 515
        public static float gR = 2.9f;

        // Token: 0x04000204 RID: 516
        public static float x_d = 3.42f;

        // Token: 0x04000205 RID: 517
        public static ValueTuple<int, int>[] rpmTorque = new ValueTuple<int, int>[]
        {
            new ValueTuple<int, int>(1000, 290),
            new ValueTuple<int, int>(2000, 325),
            new ValueTuple<int, int>(3000, 335),
            new ValueTuple<int, int>(3500, 345),
            new ValueTuple<int, int>(4000, 350),
            new ValueTuple<int, int>(4500, 355),
            new ValueTuple<int, int>(5000, 347),
            new ValueTuple<int, int>(5400, 330),
            new ValueTuple<int, int>(5650, 300),
            new ValueTuple<int, int>(6000, 280)
        };
    }
}
