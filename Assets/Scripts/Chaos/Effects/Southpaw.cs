using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.controls.southpaw", "Southpaw")] // Thanks to Dit0h for the name and idea
    [Description("Makes your controls right handed (IJKL)")]
    class Southpaw : ChaosEffect
    {
        private void OnEnable()
        {
            InputManager.Instance.layout = InputManager.Layout.Southpaw;
        }

        private void OnDisable()
        {
            InputManager.Instance.layout = InputManager.Layout.Car;
        }
    }
}
