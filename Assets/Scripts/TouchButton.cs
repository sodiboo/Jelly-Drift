using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(RectTransform))]
public class TouchButton : MonoBehaviour
{
    private RectTransform rectTransform;
    public InputType type;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        EnhancedTouchSupport.Enable();
    }

    //private void OnEnable()
    //{
    //    usedLastTime = new Dictionary<Finger, bool>();
    //    Touch.onFingerDown += HandleFinger;
    //    Touch.onFingerMove += HandleFinger;
    //    Touch.onFingerUp += HandleFinger;
    //}

    //private void OnDisable()
    //{
    //    Touch.onFingerDown -= HandleFinger;
    //    Touch.onFingerMove -= HandleFinger;
    //    Touch.onFingerUp -= HandleFinger;
    //    usedLastTime = null;
    //}

    //Dictionary<Finger, bool> usedLastTime;

    //private void HandleFinger(Finger finger)
    //{
    //    var size = Vector2.Scale(rectTransform.rect.size, transform.lossyScale);
    //    var value = new Rect((Vector2)rectTransform.position - (size * rectTransform.pivot), size).Contains(finger.screenPosition);
    //    if (!value)
    //    {
    //        if (ChaosController.Instance?.FuckyWuckyControlsUwU == true) return;
    //        if (usedLastTime.Any(x => x.Value && x.Key != finger)) return;
    //        if (usedLastTime.TryGetValue(finger, out var lastTime)) {
    //            if (!lastTime) return;
    //        }
    //        else
    //        {
    //            usedLastTime[finger] = false;
    //            return;
    //        }
    //    }
    //    usedLastTime[finger] = true;
    //    switch (type)
    //    {
    //        case InputType.Pause: Pause.Instance.PauseGame(); break;
    //        case InputType.Throttle: InputManager.Instance.throttle?.Invoke(value ? 1f : 0f); break;
    //        case InputType.Break: InputManager.Instance.throttle?.Invoke(value ? -1f : 0f); break;
    //        case InputType.Right: InputManager.Instance.steering?.Invoke(value ? 1f : 0f); break;
    //        case InputType.Left: InputManager.Instance.steering?.Invoke(value ? -1f : 0f); break;
    //    }
    //}

    private bool lastValue;

    private void Update()
    {
        var size = Vector2.Scale(rectTransform.rect.size, transform.lossyScale);
        var rect = new Rect((Vector2)transform.position - (size * rectTransform.pivot), size);
        var value = Touch.activeTouches.Any(touch => rect.Contains(touch.screenPosition));
        if (lastValue == value) return;
        lastValue = value;
        switch (type)
        {
            case InputType.Pause: if (value) Pause.Instance.PauseGame(); break;
            case InputType.Throttle: InputManager.Instance.throttle?.Invoke(value ? 1f : 0f); break;
            case InputType.Break: InputManager.Instance.throttle?.Invoke(value ? -1f : 0f); break;
            case InputType.Right: InputManager.Instance.steering?.Invoke(value ? 1f : 0f); break;
            case InputType.Left: InputManager.Instance.steering?.Invoke(value ? -1f : 0f); break;
        }
    }

    public enum InputType
    {
        Pause,
        Throttle,
        Break,
        Right,
        Left,
    }
}
