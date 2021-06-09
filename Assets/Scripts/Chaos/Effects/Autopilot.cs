using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.autopilot", "Autopilot", EffectInfo.Alignment.Bad), ConflictsWith(typeof(FuckyWuckyControlsUwU), typeof(Southpaw), typeof(Multiplayer))]
    [Description("Disables your controls and adds an AI to your car")]
    public class Autopilot : ChaosEffect
    {
        protected override void Enable()
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

        protected override void Disable()
        {
            Destroy(car.GetComponent<CarAI>());
            InputManager.Instance.layout = InputManager.Layout.Car;
            var map = InputManager.Instance.actionMaps[InputManager.Layout.Car];
            InputManager.Instance.throttle?.Invoke(map.FindAction("Throttle").ReadValue<float>());
            InputManager.Instance.steering?.Invoke(map.FindAction("Steering").ReadValue<float>());
            InputManager.Instance.breaking?.Invoke(map.FindAction("Break").triggered);
        }
    }
}