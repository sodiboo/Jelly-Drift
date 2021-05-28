using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chaos
{
    // fuck you these effects don't share an implementation but they're similar so i want them in the same file
    [ConflictsWith(typeof(Gravity), typeof(CustomGravity))] // but they do share conflicts!
    public abstract class CustomGravity : ChaosEffect
    {
        [Effect("chaos.gravity.blackhole", "Black Hole")] // Thanks to Akuma73 for the idea
        public class Blackhole : CustomGravity
        {
            public static bool Valid() => HasEnemy;

            List<Rigidbody> rbs;

            private void Awake()
            {
                rbs = FindObjectsOfType<Rigidbody>().ToList();
                rbs.Remove(car.rb);
            }
            private void OnEnable()
            {
                foreach (var rb in rbs)
                {
                    rb.useGravity = false;
                }
            }

            private void OnDisable()
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

        [Effect("chaos.gravity.checkpoint", "Checkpoint Magnet")] // Thanks to Akuma73 for the idea
        public class CheckpointMagnet : CustomGravity
        {
            CheckpointUser user;
            private void Awake()
            {
                user = car.GetComponent<CheckpointUser>();
            }

            private void OnEnable()
            {
                car.rb.useGravity = false;
            }

            private void OnDisable()
            {
                car.rb.useGravity = true;
            }

            private void FixedUpdate()
            {
                var dir = GameController.Instance.checkPoints.GetChild((user.GetCurrentCheckpoint(GameController.Instance.finalCheckpoint == 0) + 1) % GameController.Instance.checkPoints.childCount).transform.position - car.transform.position;
                car.rb.AddForce(dir.normalized * Physics.gravity.magnitude * car.rb.mass * 5f);
            }
        }
    }
}