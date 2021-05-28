using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
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

        [Effect("chaos.grip.high", "No drifting"), ConflictsWith(typeof(Grip))]
        public class High : Grip
        {
            protected override float multiplier => 10f;
        }

        [Effect("chaos.grip.low", "Smooth Wheels"), ConflictsWith(typeof(Grip))]
        public class Low : Grip
        {
            protected override float multiplier => 0f;
        }
    }
}