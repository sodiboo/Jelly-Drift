using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControls : MonoBehaviour
{
    public Dictionary<InputManager.Layout, MobileLayout> layouts = new Dictionary<InputManager.Layout, MobileLayout>();
    public static MobileControls Instance;
    public GameObject pause;

    private void Awake()
    {
#if MOBILE
        Instance = this;
#else
        Destroy(gameObject);
#endif
    }

    public void ChangeLayout(InputManager.Layout disable, InputManager.Layout enable)
    {
        if (layouts.ContainsKey(disable)) layouts[disable].gameObject.SetActive(false);
        if (layouts.ContainsKey(enable)) layouts[enable].gameObject.SetActive(true);
    }
}
