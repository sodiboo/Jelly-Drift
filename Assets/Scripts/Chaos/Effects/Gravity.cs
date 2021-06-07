using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.gravity", "Gravity"), ConflictsWith(typeof(CustomGravity))]
    public abstract class Gravity : ChaosEffect
    {
        protected abstract float multiplier { get; }
        private void OnEnable()
        {
            Physics.gravity *= multiplier;
        }

        private void OnDisable()
        {
            Physics.gravity /= multiplier;
        }

        [Effect("chaos.gravity.low", "Moon Gravity")]
        [Description("Gives you 0.166x gravity, roughly equal to the moon")]
        public class Moon : Gravity
        {
            protected override float multiplier => 0.166f;
        }

        [Effect("chaos.gravity.high", "Downforce")]
        [Description("Gives you 3x gravity, enough to prevent you from jumping")]
        public class High : Gravity
        {
            protected override float multiplier => 3f;
        }

        [Effect("chaos.gravity.negative", "Fly me to the Moon")]
        [Description("Gives you -0.5x gravity, which makes you fly up to the moon")]
        public class Negative : Gravity
        {
            protected override float multiplier => -0.5f;
        }
    }
}