using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    // fuck you these don't share an implementation but they're basically the same thing so group in one file
    public sealed class Teleport
    {
        [Effect("chaos.teleportai", "(teleports behind you)"), Impulse] // Thanks to Reclaimer64 for the name
        [Description("Teleports the AI 3 units behind you")]
        public class TeleportAI : ChaosEffect
        {
            public static bool Valid() => HasEnemy;
            private void Start()
            {
                enemy.transform.SetPositionAndRotation(car.transform.position - car.transform.forward * 3, car.transform.rotation);
                enemy.currentNode = enemy.FindClosestNode(enemy.path.childCount, enemy.transform);
            }
        }

        [Effect("chaos.teleportplayer", "I wonder where the AI is"), Impulse]
        [Description("Teleports you to the AI")]
        public class TeleportToAI : ChaosEffect
        {
            public static bool Valid() => HasEnemy;

            private void Start()
            {
                car.transform.SetPositionAndRotation(enemy.car.rb.position, enemy.car.rb.rotation);
            }
        }
    }
}