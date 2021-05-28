using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.forces.up", "Boing!")]
    class LaunchPlayer : ChaosEffect
    {
        private void Start()
        {
            car.rb.AddForce(Vector3.up * 15, ForceMode.VelocityChange);
        }
    }
}