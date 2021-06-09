using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.speed", "Speed")]
    public abstract class Speed : ChaosEffect
    {
        protected abstract float multiplier { get; }
        protected override void Enable() => car.engineForce *= multiplier;

        protected override void Disable() => car.engineForce /= multiplier;

        public override object[] CustomParameters() => new object[] { multiplier };

        [Effect("chaos.speed.fast", "Sanik", EffectInfo.Alignment.Good), ConflictsWith(typeof(Grip))]
        [Description("Makes your car 1.2x-5x faster (and prevents you from drifting or rolling as easily)")]
        public class Fast : Speed
        {
            protected override float multiplier => _multiplier;

            private float _multiplier;
            protected override void Awake()
            {
                base.Awake();
                _multiplier = Random.Range(1.2f, 5f);
            }

            protected override void Enable()
            {
                base.Enable();
                car.driftThreshold *= 10f;
            }

            protected override void Disable()
            {
                base.Disable();
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

        [Effect("chaos.speed.slow", "Slowpoke", EffectInfo.Alignment.Bad)]
        [Description("Makes your car 0.5x-0.8x as fast")]
        public class Slow : Speed
        {
            protected override float multiplier => _multiplier;

            private float _multiplier;
            protected override void Awake()
            {
                base.Awake();
                _multiplier = Random.Range(0.5f, 0.8f);
            }
        }
    }
}