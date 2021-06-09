using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.forces.up", "Boing!", EffectInfo.Alignment.Bad), Impulse]
    [Description("Launches you up in the air")]
    class LaunchPlayer : ChaosEffect
    {
        protected override void Enable()
        {
            car.rb.AddForce(Vector3.up * 15, ForceMode.VelocityChange);
        }
    }
}