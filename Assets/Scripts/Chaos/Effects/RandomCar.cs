using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.random.car", "Random Car"), Impulse]
    [Reload.OnEnable(typeof(FirstPerson), typeof(Speed), typeof(Scale), typeof(Grip),
        typeof(FuckyWuckyCollisionUwU), typeof(FuckyWuckyRenderUwU), typeof(DisableShit.Car),
        typeof(CustomGravity), typeof(Autopilot), typeof(RearSteer), typeof(Rainbow.Car), typeof(Ghost), typeof(BrightAsFuck))]
    public class RandomCar : ChaosEffect
    {
        private void Awake()
        {
            var currentCar = GameState.Instance.car;
            var rand = Random.Range(0, PrefabManager.Instance.cars.Length - 1);
            if (currentCar <= rand) rand++;
            GameState.Instance.car = rand;
            var newCar = Instantiate(PrefabManager.Instance.cars[rand]).GetComponent<Car>();
            newCar.gameObject.AddComponent<InputListener>().car = newCar;
            var user = newCar.gameObject.AddComponent<CheckpointUser>();
            user.checkedPoints = car.GetComponent<CheckpointUser>().checkedPoints;
            user.player = true;
            newCar.transform.SetPositionAndRotation(car.transform.position, car.transform.rotation);
            newCar.rb.velocity = car.rb.velocity;
            newCar.rb.angularVelocity = car.rb.angularVelocity;
            newCar.throttle = car.throttle;
            newCar.steering = car.steering;
            newCar.breaking = car.breaking;
            Destroy(car.gameObject);

            GameController.Instance.currentCar = newCar.gameObject;
            GameController.Instance.AssignCar();

            var skin = newCar.GetComponent<CarSkin>();
            if (skin.skinsToChange.Any())
            {
                GameState.Instance.skin = Random.Range(0, skin.skinsToChange.Length);
                skin.SetSkin(GameState.Instance.skin);
            }
        }
    }
}