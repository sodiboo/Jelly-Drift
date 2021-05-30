using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.forces.kickflip", "Kickflip"), Impulse]
    public class Kickflip : ChaosEffect
    {
        private void Start()
        {
            car.rb.AddForceAtPosition(car.transform.up * 10, car.transform.right, ForceMode.VelocityChange);
        }
    }
}