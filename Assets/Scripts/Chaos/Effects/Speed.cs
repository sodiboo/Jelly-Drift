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
        [Description("Makes your car 1.2x-5x faster (and prevents you from drifting or rolling as easily)")]
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

            private void FixedUpdate()
            {
                var localAngularVelocity = car.transform.InverseTransformDirection(car.rb.angularVelocity);
                if (Mathf.Abs(localAngularVelocity.z) > 0.25f)
                {
                   car.rb.angularVelocity = car.transform.TransformDirection(new Vector3(localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z * 0.75f));
                }
            }
        }

        [Effect("chaos.speed.slow", "Slowpoke")]
        [Description("Makes your car 0.5x-0.8x as fast")]
        public class Slow : Speed
        {
            float _multiplier;
            private void Awake() => _multiplier = Random.Range(0.5f, 0.8f);
            protected override float multiplier => _multiplier;
        }
    }
}