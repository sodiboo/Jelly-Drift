using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.collision.offset", "Bad Collision")]
    public class FuckyWuckyCollisionUwU : ChaosEffect
    {
        Vector3 offset;
        private void Awake()
        {
            offset = Random.onUnitSphere;
        }
        private void OnEnable()
        {
            car.collider.transform.localPosition += offset;
        }

        private void OnDisable()
        {
            car.collider.transform.localPosition -= offset;
        }
    }
}
