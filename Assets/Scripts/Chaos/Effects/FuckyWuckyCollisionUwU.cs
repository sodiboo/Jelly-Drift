using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.collision.offset", "Bad Collision", EffectInfo.Alignment.Bad)]
    [Description("Offsets your collision mesh by 1 unit in a random direction")]
    public class FuckyWuckyCollisionUwU : ChaosEffect
    {
        Vector3 offset;
        Quaternion rot;
        protected override void Awake()
        {
            base.Awake();
            offset = Random.onUnitSphere;
            rot = Random.rotationUniform;

        }
        protected override void Enable()
        {
            car.collider.transform.localPosition += offset;
            car.collider.transform.localRotation = rot * car.collider.transform.localRotation;
        }

        protected override void Disable()
        {
            car.collider.transform.localRotation = Quaternion.Inverse(rot) * car.collider.transform.localRotation;
            car.collider.transform.localPosition -= offset;
        }
    }
}
