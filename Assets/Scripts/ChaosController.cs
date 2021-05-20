using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;
using Valid = System.Func<bool>;
using TMPro;
using UnityEngine.InputSystem;

public class ChaosController : MonoBehaviour
{
    public static ChaosController Instance;
    public GameObject road;
    public Car car { get => ShakeController.Instance.car; }
    InputActionAsset inputs;
    TextMeshProUGUI text;

    void Throttle(InputAction.CallbackContext context)
    {
        car.throttle = context.ReadValue<float>();
    }

    void Steering(InputAction.CallbackContext context)
    {
        car.steering = context.ReadValue<float>();
    }

    void Break(InputAction.CallbackContext context)
    {
        car.breaking = context.ReadValueAsButton();
    }

    void AssignActionMap(InputActionMap action)
    {
        action.FindAction("Throttle").performed += Throttle;
        action.FindAction("Steering").performed += Steering;
        action.FindAction("Break").performed += Break;
    }

    public InputActionMap southpaw { get; set; }
    public InputActionMap normal { get; set; }

    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(this);
        Instance = this;
        text = GetComponent<TextMeshProUGUI>();
        inputs = PrefabManager.Instance.inputs;
        normal = inputs.FindActionMap("Car");
        southpaw = inputs.FindActionMap("Southpaw");
        AssignActionMap(normal);
        AssignActionMap(southpaw);
    }

    private void Start()
    {
        Effect("Random Car", RandomCar);
        Effect("Random Skin", RandomSkin, valid: HasSkins);
        Effect("Wait, where did it go?", DisableRenderers, EnableRenderers);
        Effect("Big", BigSize, ResetSize);
        Effect("Tiny", SmallSize, ResetSize);
        Effect("Sanik", FastSpeed, ResetSpeed);
        Effect("Slowpoke", SlowSpeed, ResetSpeed);
        Effect("No Drifting", HighGrip, ResetGrip);
        Effect("Smooth Wheels", LowGrip, ResetGrip);
        Effect("No Jumping", StrongGravity, ResetGravity);
        Effect("You'll overshoot the jump", WeakGravity, ResetGravity);
        Effect("Where are you going?", RandomRotation);
        Effect("Wrong way lol", Flip);
        Effect("Are you sure you got that checkpoint?", Pause.Instance.Recover); // Thanks to Dit0h for the name and idea
        Effect("Wow, he sure hates you", TeleportAI, valid: Race);
        Effect("I wonder where the AI is", TeleportToAI, valid: Either(Race, TimeTrial));
        Effect("Slippery Road", DriftOnRoad, DriftOffRoad);
        Effect("Oh, you don't know what Karlson is?", URL("steam://advertise/1228610"), valid: Weighted(0.03f)); // shh don't tell anyone this exists if you're actually reading the source code
        Effect("Multiplayer", AddCar, DeleteCar);
        Effect("Nonexistent Road", DisableRoad, EnableRoad); // Thanks to ProfessorEmu for the idea, i think
        Effect("Go left! No, go right, go right!", InvertAngular); // Quote from Wheatley
        Effect("Southpaw", (Action)southpaw.Enable + normal.Disable, (Action)southpaw.Disable + normal.Enable); // Thanks to Dit0h for the name and idea
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private static void Nothing() { }
    private static bool Always() => true;
    bool Race() => GameState.Instance.gamemode == Gamemode.Race;
    bool TimeTrial() => GameState.Instance.gamemode == Gamemode.TimeTrial;
    Valid Weighted(float threshold) => () => Random.value < threshold;
    Valid Either(params Valid[] predicates) => () =>
    {
        foreach (var valid in predicates)
        {
            if (valid()) return true;
        }
        return false;
    };

    void Effect(string name, Action effect, Action cleanup = null, Valid valid = null)
    {
        effects.Add((name, effect, cleanup ?? Nothing, valid ?? Always));
    }

    public int currentEffect { get; set; } = -1;

    private void Chaos()
    {
        var valid = new List<int>();
        if (currentEffect != -1)
        {
            effects[currentEffect].cleanup();
        }
        for (var i = 0; i < effects.Count; i++)
        {
            if (currentEffect == i) continue;
            if (effects[i].valid()) valid.Add(i);
        }
        currentEffect = valid[Random.Range(0, valid.Count)];
        effects[currentEffect].run();
        text.text = effects[currentEffect].name;
    }

    public void StartChaos()
    {
        InvokeRepeating("Chaos", 0, 5);
    }

    public void StopChaos()
    {
        CancelInvoke("Chaos");
        effects[currentEffect].cleanup();
        currentEffect = -1;
        text.text = "";
    }

    public List<(string name, Action run, Action cleanup, Valid valid)> effects = new List<(string, Action, Action, Valid)>() { };

    void Rotation(Quaternion newRotation)
    {
        var rotation = car.transform.rotation;
        car.transform.rotation = newRotation;
        car.rb.velocity = newRotation * (Quaternion.Inverse(rotation) * car.rb.velocity);
    }

    void Size(float size)
    {
        car.transform.localScale *= size;
        car.suspensionLength *= size;
        car.rb.mass *= size;
        car.engineForce *= size;
        car.restHeight *= size;
        foreach (var suspension in car.wheelPositions)
        {
            suspension.restLength *= size;
            suspension.springTravel *= size;
        }
    }

    void Speed(float speed)
    {
        car.engineForce *= speed;
    }

    void Grip(float grip)
    {
        car.driftThreshold *= grip;
    }

    void Gravity(float gravity)
    {
        Physics.gravity *= gravity;
    }

    public float speed { get; set; } = 1;

    public float size { get; set; } = 1;

    public float grip { get; set; } = 1;

    public float gravity { get; set; } = 1;

    void BigSize() => Size(size = Random.Range(1.2f, 3f));
    void SmallSize() => Size(size = Random.Range(0.4f, 0.8f));
    void FastSpeed() => Speed(speed = Random.Range(1.2f, 5f));
    void SlowSpeed() => Speed(speed = Random.Range(0.5f, 0.8f));
    void HighGrip() => Grip(grip = Random.Range(1.2f, 10f));
    void LowGrip() => Grip(grip = Random.Range(0.2f, 0.8f));
    void StrongGravity() => Gravity(gravity = Random.Range(1.5f, 3f));
    void WeakGravity() => Gravity(gravity = Random.Range(0.1f, 0.8f));

    void ResetSize()
    {
        Size(1 / size);
        size = 1;
    }
    void ResetSpeed()
    {
        Speed(1 / speed);
        speed = 1;
    }

    void ResetGrip()
    {
        Grip(1 / grip);
        grip = 1;
    }

    void ResetGravity()
    {
        Gravity(1 / gravity);
        gravity = 1;
    }
    

    void SetCar(GameObject obj)
    {
        GameController.Instance.currentCar = obj;
        GameController.Instance.AssignCar();
    }

    void RandomSkin() => RandomSkin(true);
    void RandomSkin(bool excludeCurrent)
    {
        var skin = car.GetComponent<CarSkin>();
        int newSkin;
        if (excludeCurrent)
        {
            if (skin.skinsToChange.Length == 1) return;
            newSkin = Random.Range(0, skin.skinsToChange.Length - 1);
            if (newSkin >= GameState.Instance.skin) newSkin++;
        }
        else
        {
            newSkin = Random.Range(0, skin.skinsToChange.Length);
        }
        GameState.Instance.skin = newSkin;
        skin.SetSkin(newSkin);
    }

    bool HasSkins() => car.GetComponent<CarSkin>().skinsToChange.Length > 1;

    void RandomCar()
    {
        var data = car.GetComponent<CheckpointUser>().checkedPoints;
        var pos = car.transform.position;
        var rot = car.transform.rotation;
        var vel = car.rb.velocity;
        var ang = car.rb.angularVelocity;
        Destroy(car.gameObject);
        var newCar = Random.Range(0, PrefabManager.Instance.cars.Length - 1);
        if (GameState.Instance.car >= newCar) newCar++;
        GameState.Instance.car = newCar;
        SetCar(Instantiate(PrefabManager.Instance.cars[newCar], pos, rot));
        car.GetComponent<CheckpointUser>().checkedPoints = data;
        car.rb.velocity = vel;
        car.rb.angularVelocity = ang;
        RandomSkin(false);
    }

    void TeleportAI()
    {
        GameController.Instance.GetComponent<Race>().enemyCar.transform.SetPositionAndRotation(car.transform.position, car.transform.rotation);
    }

    void TeleportToAI()
    {
        Transform target;
        switch (GameState.Instance.gamemode)
        {
            case Gamemode.Race:
                target = GameController.Instance.GetComponent<Race>().enemyCar.transform;
                break;
            case Gamemode.TimeTrial:
                target = ReplayController.Instance.GetComponent<Replay>().rb.transform;
                break;
            default:
                return;
        }
        car.transform.SetPositionAndRotation(target.position, target.rotation);
    }

    public bool invertTerrain { get; set; }

    void DriftOnRoad()
    {
        invertTerrain = true;
    }
    void DriftOffRoad()
    {
        invertTerrain = false;
    }
    Action URL(string url) => () =>
    {
        Application.OpenURL(url);
    };

    void RandomRotation()
    {
        Rotation(Random.rotationUniform);
    }

    void Flip()
    {
        Rotation(Quaternion.LookRotation(-car.transform.forward, car.transform.up));
    }

    void EnableRenderers()
    {
        var renderers = car.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
        }
    }

    void DisableRenderers()
    {
        var renderers = car.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }
    }

    GameObject otherCar;

    void AddCar()
    {
        otherCar = Instantiate(PrefabManager.Instance.cars[Random.Range(0, PrefabManager.Instance.cars.Length)], car.transform.position + car.transform.forward * 5, car.transform.rotation);
        otherCar.AddComponent<FakeCheckpointUser>();
    }

    void DeleteCar()
    {
        Destroy(otherCar);
    }

    void EnableRoad()
    {
        road.SetActive(true);
    }

    void DisableRoad()
    {
        road.SetActive(false);
    }

    void InvertAngular()
    {
        car.rb.angularVelocity *= -5;
    }
}
