using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Dictionary<Layout, InputActionMap> actionMaps = new Dictionary<Layout, InputActionMap>();
    public static InputManager Instance;
    public InputActionAsset inputs;
    private InputActionMap global;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        Instance = this;
        global = inputs.FindActionMap("Global");
        global.Enable();
        Assign(global.FindAction("Pause"), ctx => { if (ctx.ReadValueAsButton()) pause?.Invoke(); });
        Assign(global.FindAction("Debug"), ctx => { if (ctx.ReadValueAsButton()) debug?.Invoke(); });
        Assign(global.FindAction("Unlock"), ctx =>
        {
            if (!ctx.ReadValueAsButton()) return;
            for (var i = 0; i < PrefabManager.Instance.cars.Length; i++)
            {
                SaveManager.Instance.state.carsUnlocked[i] = true;
                for (var j = 0; j < SaveManager.Instance.state.skins[i].Length; j++)
                {
                    SaveManager.Instance.state.skins[i][j] = true;
                }
            }
            for (var i = 0; i < MapManager.Instance.maps.Length; i++)
            {
                SaveManager.Instance.state.mapsUnlocked[i] = true;
            }
        });
        foreach (Layout layout in Enum.GetValues(typeof(Layout)))
        {
            actionMaps[layout] = inputs.FindActionMap(Enum.GetName(typeof(Layout), layout));
        }
        Assign(actionMaps[Layout.Menu].FindAction("Move"), ctx => menu?.Invoke(ctx.ReadValue<Vector2>()));
        Assign(actionMaps[Layout.Menu].FindAction("Submit"), ctx => submit?.Invoke(ctx.ReadValueAsButton()));
        Assign(actionMaps[Layout.Menu].FindAction("Cancel"), ctx => cancel?.Invoke(ctx.ReadValueAsButton()));
        Car(Layout.Car);
        Car(Layout.Southpaw);
    }

    private Layout _layout = Layout.None;
    public Layout layout { get => _layout; set => ChangeLayout(_layout, _layout = value); }

    private void Start() => layout = Layout.Menu;

    public Action<float> steering { get; set; }
    public Action<float> throttle { get; set; }
    public Action<bool> breaking { get; set; }

    public Action<Vector2> menu { get; set; }
    public Action<bool> submit { get; set; }
    public Action<bool> cancel { get; set; }
    public Action debug { get; set; }
    public Action pause { get; set; }

    public enum Layout
    {
        None,
        Menu,
        Car,
        Southpaw,
    }

    private void Car(Layout layout)
    {
        Assign(actionMaps[layout].FindAction("Throttle"), ctx =>
        {
            var value = ctx.ReadValue<float>();
            if (Chaos.FuckyWuckyControlsUwU.value && value == 0f) return;
            throttle?.Invoke(value);
        });
        Assign(actionMaps[layout].FindAction("Steering"), ctx =>
        {
            var value = ctx.ReadValue<float>();
            if (Chaos.FuckyWuckyControlsUwU.value && value == 0f) return;
            steering?.Invoke(value);
        });
        Assign(actionMaps[layout].FindAction("Break"), ctx =>
        {
            var value = ctx.ReadValueAsButton();
            if (Chaos.FuckyWuckyControlsUwU.value && !value) return;
            breaking?.Invoke(value);
        });
    }

    private void Assign(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.performed += callback;
        action.canceled += callback;
    }

    private void ChangeLayout(Layout disable, Layout enable)
    {
        actionMaps?[disable]?.Disable();
        actionMaps?[enable]?.Enable();
#if MOBILE
        MobileControls.Instance?.ChangeLayout(disable, enable);
#endif
    }
}
