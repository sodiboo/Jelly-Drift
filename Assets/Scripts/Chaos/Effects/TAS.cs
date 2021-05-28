using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.tas", "TAS"), ConflictsWith(typeof(Superhot))]
    class TAS : ChaosEffect
    {
        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        private void FixedUpdate()
        {
            if (car.rb.velocity.magnitude < 0.1f) Time.timeScale = 1f;
            else Time.timeScale = 14f / car.rb.velocity.magnitude;
        }
    }
}