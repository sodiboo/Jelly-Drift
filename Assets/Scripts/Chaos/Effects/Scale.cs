using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.scale", "Scale")]
    public abstract class Scale : ChaosEffect
    {
        public static float value = 1f; // Car.Steering also scales in method body according to this value
        protected abstract float multiplier { get; }
        private void OnEnable()
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
            value = multiplier;
        }

        private void OnDisable()
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
            value = 1f;
        }

        [Effect("chaos.scale.big", "Big")]
        [Description("Makes your car 1.2x-3x bigger (and more powerful)")]
        public class Big : Scale
        {
            float _multiplier;
            private void Awake() => _multiplier = Random.Range(1.2f, 3f);
            protected override float multiplier => _multiplier;
        }

        [Effect("chaos.scale.small", "Tiny")]
        [Description("Makes your car 0.4x-0.8x as big (and as powerful)")]
        public class Tiny : Scale
        {
            float _multiplier;
            private void Awake() => _multiplier = Random.Range(0.4f, 0.8f);
            protected override float multiplier => _multiplier;
        }
    }
}