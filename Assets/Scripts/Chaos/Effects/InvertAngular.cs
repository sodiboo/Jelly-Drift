using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.forces.invert_angular", "Go left! No, go right, go right!"), Impulse] // Quote from Wheatley
    class InvertAngular : ChaosEffect
    {
        private void OnEnable()
        {
            car.rb.angularVelocity *= -5f;
        }
    }
}