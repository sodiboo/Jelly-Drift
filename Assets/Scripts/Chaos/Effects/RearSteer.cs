using UnityEngine;

namespace Chaos
{
    [Effect("chaos.rearsteer", "Rear Wheel Steering", EffectInfo.Alignment.Bad)]
    [Description("Makes your rear wheels front wheels, and your front wheels rear wheels")]
    internal class RearSteer : ChaosEffect
    {
        private void Toggle()
        {
            foreach (var sus in car.wheelPositions)
            {
                sus.rearWheel = !sus.rearWheel;
                if (sus.rearWheel)
                {
                    sus.steeringAngle = 0f;
                    sus.wheelAngleVelocity = 0f;
                    sus.transform.localRotation = Quaternion.identity;
                }
            }
        }

        protected override void Enable()
        {
            Scale.value *= -1f;
            Toggle();
        }
        protected override void Disable()
        {
            Scale.value *= -1f;
            Toggle();
        }
    }
}