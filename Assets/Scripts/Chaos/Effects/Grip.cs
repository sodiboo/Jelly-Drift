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
        protected override void Enable()
        {
            og = car.driftThreshold;
            car.driftThreshold *= multiplier;
        }

        protected override void Disable()
        {
            car.driftThreshold = og;
        }

        [Effect("chaos.grip.high", "No drifting", EffectInfo.Alignment.Good)]
        [Description("Gives you 10x drift threshold, enough to prevent you from drifting on road")]
        public class High : Grip
        {
            protected override float multiplier => 10f;
        }

        [Effect("chaos.grip.low", "Smooth Wheels", EffectInfo.Alignment.Neutral)] // drift multiplier makes this a speedboost, so it's not bad
        [Description("Gives you 0x drift threshold, which makes you always drift no matter what")]
        public class Low : Grip
        {
            protected override float multiplier => 0f;
        }
    }
}