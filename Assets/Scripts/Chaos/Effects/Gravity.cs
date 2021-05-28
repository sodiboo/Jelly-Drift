using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [ConflictsWith(typeof(Gravity), typeof(CustomGravity))]
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
        public class Moon : Gravity
        {
            protected override float multiplier => 0.166f;
        }

        [Effect("chaos.gravity.high", "Downforce")]
        public class High : Gravity
        {
            protected override float multiplier => 3f;
        }

        [Effect("chaos.gravity.negative", "Fly me to the Moon")]
        public class Negative : Gravity
        {
            protected override float multiplier => -0.5f;
        }
    }
}