using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.view.iso", "POV: You're a bird watching an intense race on @map"), ConflictsWith(typeof(FirstPerson))]
    [Description("Gives you a top-down (bird's eye) view of the map in isometric mode")]
    public class IsoView : ChaosEffect
    {
        Camera cam;
        float camHeight;
        float camDist;
        float rotSpeed;
        float movSpeed;
        bool dof;
        int camMode;
        private void OnEnable()
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

        private void OnDisable()
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