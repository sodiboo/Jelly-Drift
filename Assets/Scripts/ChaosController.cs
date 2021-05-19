using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;

public class ChaosController : MonoBehaviour
{
    public static ChaosController Instance;
    Car car { get => ShakeController.Instance.car; }
    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        Effect("Random Car", RandomCar);
        Effect("Random Skin", RandomSkin);
        Effect("Small Car", Size(0.5f), Size(2f));
        Effect("Big Car", Size(2f), Size(0.5f));
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private static void Nothing() { }

    void Effect(string name, Action effect, Action cleanup = null)
    {
        effects.Add((name, effect, cleanup ?? Nothing));
    }

    public int currentEffect = 0;

    private void Chaos()
    {
        effects[currentEffect].cleanup();
        currentEffect = Random.Range(0, effects.Count);
        effects[currentEffect].run();
    }

    public void StartChaos()
    {
        InvokeRepeating("Chaos", 0, 5);
    }

    public void StopChaos()
    {
        CancelInvoke("Chaos");
        effects[currentEffect].cleanup();
        currentEffect = 0;
    }

    // ew wtf is this long and verbose line
    public List<(string name, Action run, Action cleanup)> effects = new List<(string, Action, Action)>() { ("Nothing", Nothing, Nothing) };

    Action Size(float multiplier) => () =>
    {
        car.transform.localScale *= multiplier;
    };

    void SetCar(GameObject obj)
    {
        GameController.Instance.currentCar = obj;
        GameController.Instance.AssignCar();
    }

    void RandomSkin()
    {
        var skin = car.GetComponent<CarSkin>();
        skin.SetSkin(Random.Range(0, skin.skinsToChange.Length));
    }

    void RandomCar()
    {
        var data = car.GetComponent<CheckpointUser>().checkedPoints;
        var rb = car.GetComponent<Rigidbody>();
        var pos = car.transform.position;
        var rot = car.transform.rotation;
        var vel = rb.velocity;
        var ang = rb.angularVelocity;
        Destroy(car.gameObject);
        var newCar = PrefabManager.Instance.cars[Random.Range(0, PrefabManager.Instance.cars.Length)];
        SetCar(Instantiate(newCar, pos, rot));
        car.GetComponent<CheckpointUser>().checkedPoints = data;
        rb = car.GetComponent<Rigidbody>();
        rb.velocity = vel;
        rb.angularVelocity = ang;
        RandomSkin();
    }


}
