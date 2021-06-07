using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.grip", "Grip")]
    public abstract class Grip : ChaosEffect
    {
        protected abstract float multiplier { get; }
        float og;
        private void OnEnable()
        {
            og = car.driftThreshold;
            car.driftThreshold *= multiplier;
        }

        private void OnDisable()
        {
            car.driftThreshold = og;
        }

        [Effect("chaos.grip.high", "No drifting")]
        [Description("Gives you 10x drift threshold, enough to prevent you from drifting on road")]
        public class High : Grip
        {
            protected override float multiplier => 10f;
        }

        [Effect("chaos.grip.low", "Smooth Wheels")]
        [Description("Gives you 0x drift threshold, which makes you always drift no matter what")]
        public class Low : Grip
        {
            protected override float multiplier => 0f;
        }
    }
}