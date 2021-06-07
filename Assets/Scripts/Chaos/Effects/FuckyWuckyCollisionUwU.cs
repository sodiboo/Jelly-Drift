using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.collision.offset", "Bad Collision")]
    [Description("Offsets your collision mesh by 1 unit in a random direction")]
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
