using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.sliproad", "Offroad = Road"), ConflictsWith(typeof(Speed))] // Thanks to Akuma73 for the name
    [Description("Inverts the check for being on/off road and makes your car a bit stronger so it's advantageous to go offroad")]
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