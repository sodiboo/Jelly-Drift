using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.fake_player", "Multiplayer")]
    [Reload.OnDisable(typeof(FirstPerson), typeof(Speed), typeof(Scale), typeof(Grip),
        typeof(FuckyWuckyCollisionUwU), typeof(FuckyWuckyRenderUwU), typeof(DisableShit.Car),
        typeof(CustomGravity), typeof(Autopilot), typeof(RearSteer), typeof(Ghost))]
    public class Multiplayer : ChaosEffect
    {
        GameObject otherCar;
        private void OnEnable()
        {
            otherCar = Instantiate(PrefabManager.Instance.cars[Random.Range(0, PrefabManager.Instance.cars.Length)], car.transform.position + car.transform.forward * 5, car.transform.rotation);
            var currentUser = car.GetComponent<CheckpointUser>();
            var otherUser = otherCar.AddComponent<CheckpointUser>();
            otherUser.player = currentUser.player;
            otherUser.checkedPoints = currentUser.checkedPoints;
            var otherCarCar = otherCar.GetComponent<Car>();
            otherCar.AddComponent<InputListener>().car = otherCarCar;
            otherCarCar.throttle = car.throttle;
            otherCarCar.steering = car.steering;
            otherCarCar.breaking = car.breaking;
        }

        private void OnDisable()
        {
            if (Random.value > 0.5)
            {
                Destroy(otherCar);
                return;
            }
            Destroy(car.gameObject);
            GameController.Instance.currentCar = otherCar;
            GameController.Instance.AssignCar();
        }
    }
}