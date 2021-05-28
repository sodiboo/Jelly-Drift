using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.highfov", "Quake FOV")] // Thanks to ChaosModV for the name and idea
    class QuakeFOV : ChaosEffect
    {
        Camera cam;
        private void Awake()
        {
            cam = CameraController.Instance.GetComponentInChildren<Camera>();
        }

        private void LateUpdate()
        {
            cam.fieldOfView *= 2f;
        }
    }
}