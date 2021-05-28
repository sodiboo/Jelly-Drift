using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.sliproad", "Offroad = Road"), ConflictsWith(typeof(Speed))] // Thanks to Akuma73 for the name
    public class InvertTerrain : ChaosEffect
    {
        public static bool value; // implementation in Suspension.NewSuspension
        private void OnEnable()
        {
            value = true;
            car.engineForce *= 2f;
        }

        private void OnDisable()
        {
            car.engineForce /= 2f;
            value = false;
        }
    }
}