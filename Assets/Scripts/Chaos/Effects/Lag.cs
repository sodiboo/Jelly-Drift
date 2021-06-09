using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.lag", "Lag", EffectInfo.Alignment.Bad)] // Thanks to ChaosModV for the name and idea
    [Description("Stores and loads your position and velocity every 0.5 seconds, effectively discarding half your progress")]
    class Lag : ChaosEffect
    {
        protected override void Enable()
        {
            StoreLag();
        }

        protected override void Disable()
        {
            CancelInvoke("StoreLag");
            CancelInvoke("LoadLag");
        }

        Vector3 position;
        Quaternion rotation;
        Vector3 velocity;
        Vector3 angularVelocity;

        void StoreLag()
        {
            position = car.rb.position;
            rotation = car.rb.rotation;
            velocity = car.rb.velocity;
            angularVelocity = car.rb.angularVelocity;
            Invoke("LoadLag", 0.5f);
        }

        void LoadLag()
        {
            car.rb.position = position;
            car.rb.rotation = rotation;
            car.rb.velocity = velocity;
            car.rb.angularVelocity = angularVelocity;
            Invoke("StoreLag", 0.5f);
        }
    }
}