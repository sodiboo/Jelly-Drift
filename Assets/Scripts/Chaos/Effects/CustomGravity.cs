using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.gravity.custom", "Custom Gravity", SeparateCheats = true)]
    public abstract class CustomGravity : ChaosEffect
    {
        [Effect("chaos.gravity.blackhole", "Black Hole", EffectInfo.Alignment.Bad)] // Thanks to Akuma73 for the idea
        [Description("Makes you dense enough to attract all cones and AI")]
        public class Blackhole : CustomGravity
        {
            public static bool Valid() => HasEnemy;

            private List<Rigidbody> rbs;

            protected override void Awake()
            {
                base.Awake();
                rbs = FindObjectsOfType<Rigidbody>().ToList();
                rbs.Remove(car.rb);
            }
            protected override void Enable()
            {
                foreach (var rb in rbs)
                {
                    rb.useGravity = false;
                }
            }

            protected override void Disable()
            {
                foreach (var rb in rbs)
                {
                    rb.useGravity = true;
                }
            }

            private void FixedUpdate()
            {
                foreach (var rb in rbs)
                {
                    var dir = car.rb.worldCenterOfMass - rb.worldCenterOfMass;
                    rb.velocity = dir.normalized * rb.velocity.magnitude;
                    rb.AddForce(dir * 10f, ForceMode.Acceleration);
                }
            }
        }

        [Effect("chaos.gravity.checkpoint", "Checkpoint Magnet", EffectInfo.Alignment.Good)] // Thanks to Akuma73 for the idea
        [Description("Turns your gravitational pull towards the next checkpoint")]
        public class CheckpointMagnet : CustomGravity
        {
            private CheckpointUser user;

            protected override void Enable()
            {
                user = car.GetComponent<CheckpointUser>();
                car.rb.useGravity = false;
            }

            protected override void Disable() => car.rb.useGravity = true;

            private void FixedUpdate()
            {
                var dir = GameController.Instance.checkPoints.GetChild((user.GetCurrentCheckpoint(GameController.Instance.finalCheckpoint == 0) + 1) % GameController.Instance.checkPoints.childCount).transform.position - car.transform.position;
                car.rb.AddForce(dir.normalized * Physics.gravity.magnitude * car.rb.mass * 5f);
            }
        }
    }
}