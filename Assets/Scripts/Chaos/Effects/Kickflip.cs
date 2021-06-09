using UnityEngine;

namespace Chaos
{
    [Effect("chaos.forces.kickflip", "Kickflip", EffectInfo.Alignment.Bad), Impulse]
    [Description("Throws you up in the air and makes you spin a little")]
    public class Kickflip : ChaosEffect
    {
        protected override void Enable() => car.rb.AddForceAtPosition(car.transform.up * 10, car.transform.right, ForceMode.VelocityChange);
    }
}