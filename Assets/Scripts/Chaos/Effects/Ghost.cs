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
        }

        private void OnDisable()
        {
            car.collider.layer = LayerMask.NameToLayer("Car");
            Destroy(ghost);
        }
    }
}