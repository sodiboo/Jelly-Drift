using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    public abstract class Rotate : ChaosEffect
    {
        protected abstract Quaternion rotation { get; }
        private void Start()
        {
            var current = car.transform.rotation;
            var newer = rotation;
            car.transform.rotation = newer;
            car.rb.velocity = newer * (Quaternion.Inverse(current) * car.rb.velocity);
        }

        [Effect("chaos.rotate.random", "Where are you going?"), Impulse]
        [Description("Randomizes your car rotation")]
        public class Random : Rotate
        {
            protected override Quaternion rotation => UnityEngine.Random.rotationUniform;
        }

        [Effect("chaos.rotate.flip", "Wrong way lol"), Impulse]
        [Description("Turns your car the other way")]
        public class Flip : Rotate
        {
            protected override Quaternion rotation => Quaternion.LookRotation(-car.transform.forward, car.transform.up);
        }
    }
}