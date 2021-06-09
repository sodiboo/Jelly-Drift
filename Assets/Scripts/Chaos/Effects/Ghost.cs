using UnityEngine;

namespace Chaos
{
    [Effect("chaos.collision.none", "Ghost", EffectInfo.Alignment.Good)]
    [Description("Prevents you from colliding with most objects in a level")]
    public class Ghost : ChaosEffect
    {
        private global::Ghost ghost;
        protected override void Enable()
        {
            ghost = car.gameObject.AddComponent<global::Ghost>();
            car.collider.layer = LayerMask.NameToLayer("Ghost");
            car.suspensionLayers = 1 << LayerMask.NameToLayer("Ground");
        }

        protected override void Disable()
        {
            car.collider.layer = LayerMask.NameToLayer("Car");
            car.suspensionLayers = Physics.AllLayers ^ ((1 << LayerMask.NameToLayer("Trigger")) | (1 << LayerMask.NameToLayer("Ghost")));
            Destroy(ghost);
        }
    }
}