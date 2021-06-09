using UnityEngine;

public class InputListener : MonoBehaviour
{
    public Car car;

    private void OnEnable()
    {
        InputManager.Instance.throttle += Throttle;
        InputManager.Instance.steering += Steering;
        InputManager.Instance.breaking += Breaking;
    }

    private void OnDisable()
    {
        InputManager.Instance.throttle -= Throttle;
        InputManager.Instance.steering -= Steering;
        InputManager.Instance.breaking -= Breaking;
    }

    private void Throttle(float value) => car.throttle = value;

    private void Steering(float value) => car.steering = value;

    private void Breaking(bool value) => car.breaking = value;
}
