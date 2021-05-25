﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Action = System.Action;
using Valid = System.Func<bool>;
using TMPro;

public class ChaosController : MonoBehaviour
{
    public static ChaosController Instance;
    public GameObject blackholeObjects;
    public GameObject road;
    public Renderer terrain;
    public GameObject sun;
    public Car car { get => ShakeController.Instance.car; }
    public bool FuckyWuckyControlsUwU = false;
    TextMeshProUGUI text;
    Camera clearCam;

    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(this);
        Instance = this;
        text = GetComponent<TextMeshProUGUI>();
#if !MOBILE
        clearCam = gameObject.AddComponent<Camera>();
        clearCam.clearFlags = CameraClearFlags.SolidColor;
        clearCam.backgroundColor = Color.black;
        clearCam.cullingMask = 0;
        clearCam.depth = -2;
#endif
    }

    public void RegisterChaos()
    {
        Effect("Random Car", RandomCar);
        Effect("Random Skin", RandomSkin, valid: HasSkins);
        Effect("Wait, where did it go?", DisableRenderers, EnableRenderers);
        Effect("Big", BigSize, ResetSize);
        Effect("Tiny", SmallSize, ResetSize);
        Effect("Sanik", (Action)FastSpeed + HighGrip, (Action)ResetSpeed + ResetGrip);
        Effect("Slowpoke", SlowSpeed, ResetSpeed);
        Effect("No Drifting", HighGrip, ResetGrip);
        Effect("Smooth Wheels", LowGrip, ResetGrip);
        Effect("Downforce", StrongGravity, ResetGravity);
        Effect("Moon Gravity", WeakGravity, ResetGravity);
        Effect("Where are you going?", RandomRotation);
        Effect("Wrong way lol", Flip);
        Effect("Are you sure you got that checkpoint?", Pause.Instance.Recover); // Thanks to Dit0h for the name and idea
        Effect("(teleports behind you)", TeleportAI, valid: Race); // Thanks to Reclaimer64 for the name
        Effect("I wonder where the AI is", TeleportToAI, valid: Race);
        Effect("Cheater", TeleportToAI, valid: TimeTrial);
        Effect("Slippery Road", DriftOnRoad, DriftOffRoad);
        Effect("Oh, you don't know what Karlson is?", URL("steam://advertise/1228610"), valid: Weighted(0.03f)); // shh don't tell anyone this exists if you're actually reading the source code
        Effect("Multiplayer", AddCar, DeleteCar);
        Effect("Nonexistent Road", Disable(road), Enable(road)); // Thanks to ProfessorEmu for the idea, i think
        Effect("Go left! No, go right, go right!", InvertAngular); // Quote from Wheatley
        Effect("Cursed Controls", () => FuckyWuckyControlsUwU = true, () => FuckyWuckyControlsUwU = false);
        Effect("Southpaw", ControlScheme(InputManager.Layout.Southpaw), ControlScheme(InputManager.Layout.Car)); // Thanks to Dit0h for the name and idea
        Effect("Kickflip", Kickflip);
        Effect("Potato", () => Application.targetFrameRate = 5, () => Application.targetFrameRate = -1); // Thanks to WoodComet for the idea
        Effect("Bigger", BiggerSize, ResetSize); // Thanks to WoodComet for the name and idea
        Effect("Fly me to the Moon", Up); // Thanks to WoodComet for the idea
        Effect("Where did everything go?", Disable(road) + Disable(terrain), Enable(road) + Enable(terrain)); // Thanks to WoodComet for the idea, thanks to ChaosModV for the name
        Effect("Dark Mode", Disable(sun), Enable(sun)); // Thanks to WoodComet for the idea
        Effect("Bad Collision", OffsetCollider, ResetCollider);
        Effect("Ghost", GhostMode, UnGhost);
        Effect("Lag", SetLag, StopLag); // Thanks to ChaosModV for the name and idea
        Effect("Checkpoint Magnet", EnableMagnet, DisableMagnet); // Thanks to Akuma73 for the idea
        Effect("Black Hole", EnableBlackhole, DisableBlackhole, valid: Race); // Thanks to Akuma73 for the idea
        Effect("Autopilot", AddAI, DeleteAI, valid: Race);
        Effect("Rainbow", EnableRainbow, DisableRainbow, valid: HasManySkins); // Thanks to Akuma73 for the idea
        Effect("Superhot", Superhot, Supercold);
        Effect($"POV: You're a bird watching an intense race on {MapManager.Instance.maps[GameState.Instance.map].name}", IsoOn, IsoOff);
        Effect("Dani Says", Simon, UnSimon);
        simonPunishment = Effect("Punishment", Nothing, UnSimon, valid: Never);
#if MOBILE
        Effect("Portrait Mode", Vertical, ResetScreen, valid: IsHorizontal);
#else
        Effect("Mobile", Vertical, ResetScreen, valid: IsHorizontal);
#endif
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        StopChaos();
    }

    private static void Nothing() { }
    private static bool Always() => true;
    private static bool Never() => false;
    bool Race() => GameState.Instance.gamemode == Gamemode.Race;
    bool TimeTrial() => GameState.Instance.gamemode == Gamemode.TimeTrial;
    Valid Weighted(float threshold) => () => Random.value < threshold;
    Action Rig(int effect) => () => riggedEffect = effect;

    int Effect(string name, Action effect, Action cleanup = null, Valid valid = null)
    {
        var count = effects.Count;
        effects.Add((name, effect, cleanup ?? Nothing, valid ?? Always));
        return count;
    }

    public int currentEffect { get; set; } = -1;
    private int riggedEffect { get; set; } = -1;

    private void Chaos()
    {
        if (currentEffect != -1)
        {
            effects[currentEffect].cleanup();
        }
        if (riggedEffect == -1)
        {
            var valid = new List<int>();
            for (var i = 0; i < effects.Count; i++)
            {
                if (currentEffect == i) continue;
                if (effects[i].valid()) valid.Add(i);
            }
            currentEffect = valid[Random.Range(0, valid.Count)];
        }
        else
        {
            currentEffect = riggedEffect;
            riggedEffect = -1;
        }
        effects[currentEffect].run();
        text.text = effects[currentEffect].name;
    }

    public void StartChaos()
    {
        if (!IsInvoking("Chaos")) InvokeRepeating("Chaos", 0, 5);
    }

    public void StopChaos()
    {
        if (!IsInvoking("Chaos")) return;
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

    void Collider(Vector3 offset)
    {
        car.collider.transform.localPosition += offset;
    }

    public float speed { get; set; } = 1;

    public float size { get; set; } = 1;

    public float grip { get; set; } = 1;

    public float gravity { get; set; } = 1;
    public new Vector3 collider { get; set; } = Vector3.zero;

    void BiggerSize() => Size(size = 10f);
    void BigSize() => Size(size = Random.Range(1.2f, 3f));
    void SmallSize() => Size(size = Random.Range(0.4f, 0.8f));
    void FastSpeed() => Speed(speed = Random.Range(1.2f, 5f));
    void SlowSpeed() => Speed(speed = Random.Range(0.5f, 0.8f));
    void HighGrip() => Grip(grip = 10f);
    void LowGrip() => Grip(grip = 0.1f);
    void StrongGravity() => Gravity(gravity = 3f);
    void WeakGravity() => Gravity(gravity = 0.166f);
    void OffsetCollider() => Collider(collider = Random.insideUnitSphere);

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
    
    void ResetCollider()
    {
        Collider(-collider);
        collider = Vector3.zero;
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
        var throttle = car.throttle;
        var steering = car.steering;
        var breaking = car.breaking;
        Destroy(car.gameObject);
        var newCar = Random.Range(0, PrefabManager.Instance.cars.Length - 1);
        if (GameState.Instance.car >= newCar) newCar++;
        GameState.Instance.car = newCar;
        SetCar(Instantiate(PrefabManager.Instance.cars[newCar], pos, rot));
        car.GetComponent<CheckpointUser>().checkedPoints = data;
        car.gameObject.AddComponent<InputListener>().car = car;
        car.rb.velocity = vel;
        car.rb.angularVelocity = ang;
        car.throttle = throttle;
        car.steering = steering;
        car.breaking = breaking;
        RandomSkin(false);
    }

    void TeleportAI()
    {
        var enemy = GameController.Instance.GetComponent<Race>().enemyCar.GetComponent<CarAI>();
        enemy.transform.SetPositionAndRotation(car.transform.position - car.transform.forward * 3, car.transform.rotation);
        enemy.currentNode = enemy.FindClosestNode(enemy.path.childCount, enemy.transform);
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
        var otherCarCar = otherCar.GetComponent<Car>();
        otherCar.AddComponent<InputListener>().car = otherCarCar;
        otherCarCar.throttle = car.throttle;
        otherCarCar.steering = car.steering;
        otherCarCar.breaking = car.breaking;
    }

    void DeleteCar()
    {
        if (Random.value > 0.5)
        {
            Destroy(otherCar);
            return;
        }
        var chekedPoints = car.GetComponent<CheckpointUser>().checkedPoints;
        Destroy(otherCar.GetComponent<FakeCheckpointUser>());
        var user = otherCar.AddComponent<CheckpointUser>();
        user.checkedPoints = chekedPoints;
        Destroy(car.gameObject);
        SetCar(otherCar);
    }

    Action Enable(GameObject obj) => () => obj.SetActive(true);

    Action Disable(GameObject obj) => () => obj.SetActive(false);

    Action Enable(Behaviour behaviour) => () => behaviour.enabled = true;

    Action Disable(Behaviour behaviour) => () => behaviour.enabled = false;

    Action Enable(Renderer renderer) => () => renderer.enabled = true;

    Action Disable(Renderer renderer) => () => renderer.enabled = false;

    void InvertAngular()
    {
        car.rb.angularVelocity *= -5;
    }

    Action ControlScheme(InputManager.Layout layout) => () =>
    {
        InputManager.Instance.layout = layout;
    };

    void Kickflip()
    {
        car.rb.AddForceAtPosition(car.transform.up * 10, car.transform.right, ForceMode.VelocityChange);
    }

    private void Up()
    {
        car.rb.AddForce(Vector3.up * 30, ForceMode.VelocityChange);
    }

    private void GhostMode()
    {
        car.collider.SetActive(false);
        if (car.TryGetComponent<Ghost>(out var ghost))
        {
            ghost.enabled = true;
        }
        else
        {
            car.gameObject.AddComponent<Ghost>();
        }
    }

    private void UnGhost()
    {
        car.collider.SetActive(true);
        car.GetComponent<Ghost>().enabled = false;
    }

    Vector3 lagPos;
    Quaternion lagRot;
    Vector3 lagVel;
    Vector3 lagAng;

    void SetLag()
    {
        lagPos = car.transform.position;
        lagRot = car.transform.rotation;
        lagVel = car.rb.velocity;
        lagAng = car.rb.angularVelocity;
        Invoke("LoadLag", 0.5f);
    }

    void LoadLag()
    {
        car.transform.position = lagPos;
        car.transform.rotation = lagRot;
        car.rb.velocity = lagVel;
        car.rb.angularVelocity = lagAng;
        Invoke("SetLag", 0.5f);
    }

    void StopLag()
    {
        CancelInvoke("SetLag");
        CancelInvoke("LoadLag");
    }

    bool magnet;
    CheckpointUser magnetUser;

    void EnableMagnet()
    {
        magnet = true;
        car.rb.useGravity = false;
        magnetUser = car.GetComponent<CheckpointUser>();
    }

    void DisableMagnet()
    {
        magnet = false;
        car.rb.useGravity = true;
    }

    bool blackhole;
    Rigidbody[] blackholeRBs;

    void EnableBlackhole()
    {
        blackhole = true;
        if (blackholeObjects != null)
        {
            blackholeRBs = new Rigidbody[blackholeObjects.transform.childCount + 1];
            for (var i = 0; i < blackholeObjects.transform.childCount; i++)
            {
                blackholeRBs[i] = blackholeObjects.transform.GetChild(i).GetComponent<Rigidbody>();
            }
            blackholeRBs[blackholeObjects.transform.childCount] = GameController.Instance.GetComponent<Race>().enemyCar.GetComponent<Rigidbody>();
        }
        else
        {
            blackholeRBs = new Rigidbody[] { GameController.Instance.GetComponent<Race>().enemyCar.GetComponent<Rigidbody>() };
        }
        for (var i = 0; i < blackholeRBs.Length; i++)
        {
            blackholeRBs[i].useGravity = false;
        }
    }

    void DisableBlackhole()
    {
        blackhole = false;
        for (var i = 0; i < blackholeRBs.Length; i++)
        {
            blackholeRBs[i].useGravity = true;
        }
    }

    bool rainbow;
    Material rainbowMat;
    
    bool HasManySkins() => car.GetComponent<CarSkin>().skinsToChange.Length > 2;

    void EnableRainbow()
    {
        var skin = car.GetComponent<CarSkin>();
        rainbow = true;
        rainbowMat = new Material(skin.materials[GameState.Instance.skin]);
        for (var i = 0; i < skin.skinsToChange[0].myArray.Length; i++)
        {
            var renderer = skin.renderers[skin.skinsToChange[0].myArray[i++]];
            var newMats = new Material[renderer.materials.Length];
            renderer.materials.CopyTo(newMats, 0);
            newMats[skin.skinsToChange[0].myArray[i++]] = rainbowMat;
            renderer.materials = newMats;
        }
            
    }

    void DisableRainbow()
    {
        rainbow = false;
    }



    private void Update()
    {
        if (rainbow)
        {
            Color.RGBToHSV(rainbowMat.color, out var H, out var S, out var V);
            if (S < 0.5f) S = 0.5f;
            if (V < 0.5f) V = 0.5f;
            H += Time.deltaTime;
            H %= 1;
            rainbowMat.color = Color.HSVToRGB(H, S, V);
        }
        if (hot && GameController.Instance.playing && !Pause.Instance.paused)
        {
            Time.timeScale = Mathf.Clamp(Mathf.Abs(car.speed)  / 100f, 0.1f, 1f);
        }
        if (enforceSimon) TickSimon();
    }

    private void FixedUpdate()
    {
        if (magnet)
        {
            var dir = GameController.Instance.checkPoints.GetChild((magnetUser.GetCurrentCheckpoint(GameController.Instance.finalCheckpoint == 0) + 1) % GameController.Instance.checkPoints.childCount).transform.position - car.transform.position;
            car.rb.AddForce(-Physics.gravity * car.rb.mass);
            car.rb.AddForce(dir.normalized * Physics.gravity.magnitude * car.rb.mass * 5f);
        }
        if (blackhole)
        {
            for (var i = 0; i < blackholeRBs.Length; i++)
            {
                var dir = car.transform.position - blackholeRBs[i].transform.position;
                blackholeRBs[i].AddForce(dir * blackholeRBs[i].mass * 10f);
            }
        }
    }

    void AddAI()
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

    void DeleteAI()
    {
        Destroy(car.GetComponent<CarAI>());
        InputManager.Instance.layout = InputManager.Layout.Car;
    }

    bool hot;

    void Superhot()
    {
        hot = true;
    }

    void Supercold()
    {
        hot = false;
        Time.timeScale = 1;
    }

    float camHeight;
    float camDist;

    void IsoOn()
    {
        camHeight = CameraController.Instance.camHeight;
        camDist = CameraController.Instance.distFromTarget;
        CameraController.Instance.camHeight = 20f;
        CameraController.Instance.distFromTarget = 0.1f;
        CameraController.Instance.GetComponentInChildren<Camera>().orthographic = true;
    }

    void IsoOff()
    {
        CameraController.Instance.camHeight = camHeight;
        CameraController.Instance.distFromTarget = camDist;
        CameraController.Instance.GetComponentInChildren<Camera>().orthographic = false;
    }

    bool invertSimon;
    SimonAction simon;
    bool enforceSimon;

    enum SimonAction
    {
        Accelerate,
        Decelerate,
#if !MOBILE
        Break,
#endif
        Left,
        Right,
    }

    Vector3? simonVel;
    Vector3? simonAng;

    void Simon() => StartCoroutine(RunSimon());
    

    IEnumerator RunSimon()
    {
        simon = (SimonAction)Random.Range(0, System.Enum.GetValues(typeof(SimonAction)).Length);
        invertSimon = Random.value > 0.5f;
        Time.timeScale = 0.2f;
        var split = Instantiate(PrefabManager.Instance.splitUi).GetComponent<SplitUi>();
        split.transform.SetParent(UIManager.Instance.splitPos);
        split.transform.localPosition = Vector3.zero;
        string name = "";
        switch (simon)
        {
            case SimonAction.Accelerate: name = "forwards"; break;
            case SimonAction.Decelerate: name = "backwards"; break;
#if !MOBILE
            case SimonAction.Break: name = "break"; break;
#endif
            case SimonAction.Right: name = "right"; break;
            case SimonAction.Left: name = "left"; break;
        }
        split.SetSplit(invertSimon ? $"Dani says don't press {name}" : $"Dani says press {name}");
        yield return new WaitForSeconds(1f);

        enforceSimon = true;
        Time.timeScale = 1f;
    }

    void TickSimon()
    {
        if (invertSimon == FollowsSimon())
        {
            simonVel = car.rb.velocity;
            simonAng = car.rb.angularVelocity;
            car.rb.constraints = RigidbodyConstraints.FreezeAll;
            enforceSimon = false;
            riggedEffect = simonPunishment;
        }
    }

    bool FollowsSimon()
    {
        switch (simon)
        {
            case SimonAction.Accelerate: return car.throttle > 0f;
            case SimonAction.Decelerate: return car.throttle < 0f;
#if !MOBILE
            case SimonAction.Break: return car.breaking;
#endif
            case SimonAction.Right: return car.steering > 0f;
            case SimonAction.Left: return car.steering < 0f;
            default: return false;
        }
    }

    private int simonPunishment;

    void UnSimon()
    {
        if (riggedEffect != -1) return;
        CancelInvoke("EnforceSimon");
        enforceSimon = false;
        car.rb.constraints = RigidbodyConstraints.None;
        Time.timeScale = 1f;
        if (simonVel.HasValue) car.rb.velocity = simonVel.Value;
        if (simonAng.HasValue) car.rb.angularVelocity = simonAng.Value;
        simonVel = simonAng = null;
    }

    bool IsHorizontal() => Screen.height < Screen.width;

    void ResetScreen()
    {
#if MOBILE
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        StartCoroutine(Wait3Seconds());
#else
        var cam = CameraController.Instance.GetComponentInChildren<Camera>();
        cam.ResetProjectionMatrix();
        cam.rect = new Rect(0, 0, 1, 1);
#endif
    }

    void Vertical()
    {
#if MOBILE
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        StartCoroutine(Wait3Seconds());
#else
        var cam = CameraController.Instance.GetComponentInChildren<Camera>();
        cam.projectionMatrix = Matrix4x4.Perspective(cam.fieldOfView, 9f/16f, cam.nearClipPlane, cam.farClipPlane);
        var wRatio = 9f / 16f / (Screen.width / Screen.height);
        cam.rect = new Rect((1 - wRatio) / 2, 0, wRatio, 1);
#endif
    }

    IEnumerator Wait3Seconds()
    {
        var pause = InputManager.Instance.inputs.FindActionMap("Global").FindAction("Pause");
        pause.Disable();
#if MOBILE
        MobileControls.Instance.pause.SetActive(false);
#endif
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        pause.Enable();
#if MOBILE
        MobileControls.Instance.pause.SetActive(true);
#endif
    }
}
