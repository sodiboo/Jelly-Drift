using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    Dictionary<Layout, InputActionMap> actionMaps = new Dictionary<Layout, InputActionMap>();
    public static InputManager Instance;
    public InputActionAsset inputs;
    InputActionMap global;
    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        Instance = this;
        global = inputs.FindActionMap("Global");
        global.Enable();
        global.FindAction("Pause").performed += _ => pause();
        global.FindAction("Debug").performed += _ => debug();
        global.FindAction("Unlock").performed += _ =>
        {
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
        };
        foreach (Layout layout in Enum.GetValues(typeof(Layout)))
        {
            actionMaps[layout] = inputs.FindActionMap(Enum.GetName(typeof(Layout), layout));
        }
        Assign(actionMaps[Layout.Menu].FindAction("Move"), ctx => menu(ctx.ReadValue<Vector2>()));
        Assign(actionMaps[Layout.Menu].FindAction("Submit"), ctx => submit(ctx.ReadValueAsButton()));
        Assign(actionMaps[Layout.Menu].FindAction("Cancel"), ctx => cancel(ctx.ReadValueAsButton()));
        Car(Layout.Car);
        Car(Layout.Southpaw);
    }
    Layout _layout = Layout.None;
    public Layout layout { get => _layout; set => ChangeLayout(_layout, _layout = value); }

    private void Start()
    {
        layout = Layout.Menu;
    }

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

    void Car(Layout layout)
    {
        Assign(actionMaps[layout].FindAction("Throttle"), ctx => {
            var value = ctx.ReadValue<float>();
            if (ChaosController.Instance.FuckyWuckyControlsUwU && value == 0f) return;
            throttle(value);
        });
        Assign(actionMaps[layout].FindAction("Steering"), ctx => {
            var value = ctx.ReadValue<float>();
            if (ChaosController.Instance.FuckyWuckyControlsUwU && value == 0f) return;
            steering(value);
        });
        Assign(actionMaps[layout].FindAction("Break"), ctx => {
            var value = ctx.ReadValueAsButton();
            if (ChaosController.Instance.FuckyWuckyControlsUwU && !value) return;
            breaking(value);
        });
    }

    void Assign(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.performed += callback;
        action.canceled += callback;
    }

    void ChangeLayout(Layout disable, Layout enable)
    {
        actionMaps[disable]?.Disable();
        actionMaps[enable]?.Enable();
    }
}
