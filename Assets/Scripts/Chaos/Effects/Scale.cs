using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.scale", "Scale")]
    public abstract class Scale : ChaosEffect
    {
        public static float value = 1f; // Car.Steering also scales in method body according to this value
        protected abstract float multiplier { get; }
        protected override void Enable()
        {
            car.transform.localScale *= multiplier;
            car.suspensionLength *= multiplier;
            car.rb.mass *= multiplier;
            car.engineForce *= multiplier;
            car.restHeight *= multiplier;
            foreach (var suspension in car.wheelPositions)
            {
                suspension.restLength *= multiplier;
                suspension.springTravel *= multiplier;
            }
            value *= multiplier;
        }

        protected override void Disable()
        {
            car.transform.localScale /= multiplier;
            car.suspensionLength /= multiplier;
            car.rb.mass /= multiplier;
            car.engineForce /= multiplier;
            car.restHeight /= multiplier;
            foreach (var suspension in car.wheelPositions)
            {
                suspension.restLength /= multiplier;
                suspension.springTravel /= multiplier;
            }
            value /= multiplier;
        }

        public override object[] CustomParameters() => new object[] { multiplier };

        [Effect("chaos.scale.big", "Big", EffectInfo.Alignment.Good)]
        [Description("Makes your car 1.2x-3x bigger (and more powerful)")]
        public class Big : Scale
        {
            protected override float multiplier => _multiplier;

            private float _multiplier;
            protected override void Awake()
            {
                base.Awake();
                _multiplier = Random.Range(1.2f, 3f);
            }
        }

        [Effect("chaos.scale.small", "Tiny", EffectInfo.Alignment.Bad)]
        [Description("Makes your car 0.4x-0.8x as big (and as powerful)")]
        public class Tiny : Scale
        {
            protected override float multiplier => _multiplier;

            private float _multiplier;
            protected override void Awake()
            {
                base.Awake();
                _multiplier = Random.Range(0.4f, 0.8f);
            }
        }
    }
}