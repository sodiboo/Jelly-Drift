using UnityEngine;
// using Steamworks;

// i couldn't fucking figure out how to make this work, but i'm not gonna delete it because i'm pretty sure i got most of the way there, so if anyone wants to continue on this please do, just add the Facepunch Steamworks library
// originally the unity input system code was in UnityInputManager.cs, but after giving up on Steam Input i merged it all into InputManager. Please split that off again if you wanna work on Steam Input
// the main reason i wanted Steam Input is for Steam Controller, but Unity is working on that anyways so hopefully in the future i can enjoy native support at the engine level for my wonderful controller
public class SteamInputManager : MonoBehaviour
{
    /*
    void Start()
    {
        SteamClient.Init(480);
        if (SteamClient.RestartAppIfNecessary(480)) Application.Quit();
    }

    private void Update()
    {
        SteamInput.RunFrame();
        switch (InputManager.Instance.layout)
        {
            case InputManager.Layout.Car:
                Car("Car");
                break;
            case InputManager.Layout.Southpaw:
                Car("Southpaw");
                break;
            case InputManager.Layout.Menu:
                var controllers = SteamInput.Controllers.ToList();
                var x = 0;
                var y = 0;
                var submit = false;
                var cancel = false;
                for (var i = 0; i < controllers.Count; i++)
                {
                    var controller = controllers[i];
                    controller.ActionSet = "Menu";
                    if (controller.GetDigitalState("menu_up").Pressed) y--;
                    if (controller.GetDigitalState("menu_down").Pressed) y++;
                    if (controller.GetDigitalState("menu_left").Pressed) x--;
                    if (controller.GetDigitalState("menu_right").Pressed) x++;
                    submit |= controller.GetDigitalState("menu_select").Pressed;
                    cancel |= controller.GetDigitalState("menu_cancel").Pressed;
                }
                var menu = new Vector2(x, y);
                InputManager.Instance.menu(menu);
                InputManager.Instance.lastMenu = menu;
                InputManager.Instance.submit(submit);
                InputManager.Instance.lastSubmit = submit;
                InputManager.Instance.cancel(cancel);
                InputManager.Instance.lastCancel = cancel;
                break;
        }
    }

    void Car(string actionSet)
    {
        var controllers = SteamInput.Controllers.ToList();
        var throttle = 0f;
        var steering = 0f;
        var breaking = false;
        for (var i = 0; i < controllers.Count; i++)
        {
            var controller = controllers[i];
            controller.ActionSet = actionSet;
            var drive = controller.GetAnalogState("Drive");
            throttle += drive.Y;
            steering += drive.X;
            throttle += (controller.GetAnalogState("ThrottleUp").X - controller.GetAnalogState("ThrottleDown").X) / 2;
            steering += (controller.GetAnalogState("SteeringRight").X - controller.GetAnalogState("SteeringLeft").X) / 2;
            breaking |= controller.GetDigitalState("Break").Pressed;
        }
        InputManager.Instance.throttle(Mathf.Clamp(throttle, -1, 1));
        InputManager.Instance.steering(Mathf.Clamp(steering, -1, 1));
        InputManager.Instance.breaking(breaking);
    }
    */
}
