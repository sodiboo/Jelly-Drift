using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.rearsteer", "Rear Wheel Steering")]
    class RearSteer : ChaosEffect
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

        private void OnEnable() => Toggle();
        private void OnDisable() => Toggle();
    }
}