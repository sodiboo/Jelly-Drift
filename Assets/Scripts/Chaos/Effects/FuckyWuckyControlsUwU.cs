using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.controls.cursed", "Cursed Controls")] // Thanks to MY DUMBASS for the accidental invention of this effect.
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
        }
    }
}
