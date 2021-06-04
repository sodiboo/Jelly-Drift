using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.collision.none", "Ghost")]
    public class Ghost : ChaosEffect
    {
        global::Ghost ghost;
        private void OnEnable()
        {
            ghost = car.gameObject.AddComponent<global::Ghost>();
            car.collider.layer = LayerMask.NameToLayer("Ghost");
            car.suspensionLayers = 1 << LayerMask.NameToLayer("Ground");
        }

        private void OnDisable()
        {
            car.collider.layer = LayerMask.NameToLayer("Car");
            car.suspensionLayers = Physics.AllLayers ^ ((1 << LayerMask.NameToLayer("Trigger")) | (1 << LayerMask.NameToLayer("Ghost")));
            Destroy(ghost);
        }
    }
}