using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.controls.cursed", "Cursed Controls")] // Thanks to MY DUMBASS for the accidental invention of this effect.
    [Description("Makes your controls never reset to zero")]
    public class FuckyWuckyControlsUwU : ChaosEffect // Thanks to the internet for the name of the class.
    {
        public static FuckyWuckyControlsUwU Instance; // Implementation in InputManager.Car, look for references to this property
        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
            // also refresh input values so it doesn't stay cursed
            var map = InputManager.Instance.actionMaps[InputManager.Layout.Car];
            InputManager.Instance.throttle?.Invoke(map.FindAction("Throttle").ReadValue<float>());
            InputManager.Instance.steering?.Invoke(map.FindAction("Steering").ReadValue<float>());
            InputManager.Instance.breaking?.Invoke(map.FindAction("Break").triggered);
        }
    }
}
