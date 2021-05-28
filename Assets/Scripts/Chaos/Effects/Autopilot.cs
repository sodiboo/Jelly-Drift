using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.autopilot", "Autopilot")]
    public class Autopilot : ChaosEffect
    {
        private void OnEnable()
        {
            var engine = (int)car.engineForce;
            var ai = car.gameObject.AddComponent<CarAI>();
            ai.difficultyConfig = new int[] { engine, engine, engine };
            var enemy = GameController.Instance.GetComponent<Race>().enemyCar.GetComponent<CarAI>();
            car.engineForce = engine;
            ai.respawnHeight = enemy.respawnHeight;
            ai.SetPath(enemy.path);
            ai.currentNode = ai.FindClosestNode(ai.path.childCount, ai.transform);
            InputManager.Instance.layout = InputManager.Layout.None;
        }

        private void OnDisable()
        {
            Destroy(car.GetComponent<CarAI>());
            InputManager.Instance.layout = InputManager.Layout.Car;
        }
    }
}