using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.speed", "Speed")]
    public abstract class Speed : ChaosEffect
    {
        protected abstract float multiplier { get; }
        protected virtual void OnEnable()
        {
            car.engineForce *= multiplier;
        }

        protected virtual void OnDisable()
        {
            car.engineForce /= multiplier;
        }

        [Effect("chaos.speed.fast", "Sanik"), ConflictsWith(typeof(Grip))]
        public class Fast : Speed
        {
            float _multiplier;
            private void Awake() => _multiplier = Random.Range(1.2f, 5f);
            protected override float multiplier => _multiplier;

            protected override void OnEnable()
            {
                base.OnEnable();
                car.driftThreshold *= 10f;
            }

            protected override void OnDisable()
            {
                base.OnDisable();
                car.driftThreshold /= 10f;
            }
        }

        [Effect("chaos.speed.slow", "Slowpoke")]
        public class Slow : Speed
        {
            float _multiplier;
            private void Awake() => _multiplier = Random.Range(0.5f, 0.8f);
            protected override float multiplier => _multiplier;
        }
    }
}