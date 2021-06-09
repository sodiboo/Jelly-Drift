using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.highfov", "Quake FOV", EffectInfo.Alignment.Neutral)] // Thanks to ChaosModV for the name and idea
    [Description("Doubles your FOV")]
    class QuakeFOV : ChaosEffect
    {
        Camera cam;
        protected override void Awake()
        {
            base.Awake();
            cam = CameraController.Instance.GetComponentInChildren<Camera>();
        }

        private void LateUpdate()
        {
            cam.fieldOfView *= 2f;
        }
    }
}