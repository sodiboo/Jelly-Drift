using UnityEngine;

namespace Chaos
{
    [Effect("chaos.view.iso", "POV: You're a bird watching an intense race on @map", EffectInfo.Alignment.Bad), ConflictsWith(typeof(FirstPerson))]
    [Description("Gives you a top-down (bird's eye) view of the map in isometric mode")]
    public class IsoView : ChaosEffect
    {
        private Camera cam;
        private float camHeight;
        private float camDist;
        private float rotSpeed;
        private float movSpeed;
        private bool dof;
        private int camMode;
        protected override void Enable()
        {
            cam = CameraController.Instance.GetComponentInChildren<Camera>();
            camHeight = CameraController.Instance.camHeight;
            camDist = CameraController.Instance.distFromTarget;
            rotSpeed = CameraController.Instance.rotationSpeed;
            movSpeed = CameraController.Instance.moveSpeed;
            dof = PPController.Instance.dof.enabled.value;
            CameraController.Instance.camHeight = 100f;
            CameraController.Instance.distFromTarget = 0.1f;
            CameraController.Instance.moveSpeed *= 10f;
            CameraController.Instance.rotationSpeed *= 10f;
            camMode = SaveState.Instance.cameraMode;
            SaveState.Instance.cameraMode = 1;
            cam.orthographic = true;
            cam.orthographicSize = 80f;
            PPController.Instance.dof.enabled.value = false;
        }

        protected override void Disable()
        {
            PPController.Instance.dof.enabled.value = dof;
            CameraController.Instance.camHeight = camHeight;
            CameraController.Instance.distFromTarget = camDist;
            CameraController.Instance.moveSpeed = movSpeed;
            CameraController.Instance.rotationSpeed = rotSpeed;
            SaveState.Instance.cameraMode = camMode;
            CameraController.Instance.GetComponentInChildren<Camera>().orthographic = false;
        }
    }
}