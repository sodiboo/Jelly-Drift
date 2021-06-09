using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
#if MOBILE
    [HideInCheatGUI]
#endif
    [Effect("chaos.view.mobile", "Mobile Experience", EffectInfo.Alignment.Neutral)]
    [Description("Attempts (but fails) to make your view 9:16")]
    class VerticalDesktop : ChaosEffect
    {
        Camera cam;
        Camera clearCam;
        protected override void Awake()
        {
            base.Awake();
            cam = CameraController.Instance.GetComponentInChildren<Camera>();
            clearCam = gameObject.AddComponent<Camera>();
            clearCam.clearFlags = CameraClearFlags.SolidColor;
            clearCam.backgroundColor = Color.black;
            clearCam.cullingMask = 0;
            clearCam.enabled = false;
            clearCam.depth = -2;
        }

        protected override void Enable()
        {
            cam.projectionMatrix = Matrix4x4.Perspective(cam.fieldOfView, 9f / 16f, cam.nearClipPlane, cam.farClipPlane);
            var wRatio = 9f / 16f / (Screen.width / Screen.height);
            cam.rect = new Rect((1 - wRatio) / 2, 0, wRatio, 1);
            clearCam.enabled = true;
        }

        protected override void Disable()
        {
            cam.ResetProjectionMatrix();
            cam.rect = new Rect(0, 0, 1, 1);
            clearCam.enabled = false;
        }

        private void OnDestroy()
        {
            Destroy(clearCam);
        }

#if MOBILE
        public static bool Valid() => false;
#else
        public static bool Valid() => true;
#endif
    }
}
