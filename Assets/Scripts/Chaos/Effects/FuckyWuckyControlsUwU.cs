using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.controls.cursed", "Cursed Controls", EffectInfo.Alignment.Bad)] // Thanks to MY DUMBASS for the accidental invention of this effect.
    [Description("Makes your controls never reset to zero")]
    public class FuckyWuckyControlsUwU : ChaosEffect // Thanks to the internet for the name of the class.
    {
        public static bool value; // Implementation in InputManager.Car, look for references to this property

        protected override void Enable()
        {
            value = true;
        }

        protected override void Disable()
        {
            value = false;
            // also refresh input values so it doesn't stay cursed
            var map = InputManager.Instance.actionMaps[InputManager.Layout.Car];
            InputManager.Instance.throttle?.Invoke(map.FindAction("Throttle").ReadValue<float>());
            InputManager.Instance.steering?.Invoke(map.FindAction("Steering").ReadValue<float>());
            InputManager.Instance.breaking?.Invoke(map.FindAction("Break").triggered);
        }
    }
}
