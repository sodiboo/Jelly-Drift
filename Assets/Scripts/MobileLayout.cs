using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileLayout : MonoBehaviour
{
    public InputManager.Layout layout;
    private void Start()
    {
        MobileControls.Instance.layouts[layout] = this;
        gameObject.SetActive(false);
    }
}
