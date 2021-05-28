using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.view.iso", "POV: You're a bird watching an intense race on [MAP]"), ConflictsWith(typeof(FirstPerson))]
    public class IsoView : ChaosEffect
    {
        float camHeight;
        float camDist;
        bool dof;
        private void OnEnable()
        {
            ChaosController.Instance.text.text = $"POV: You're a bird watching an intense race on {MapManager.Instance.maps[GameState.Instance.map].name}";
            camHeight = CameraController.Instance.camHeight;
            camDist = CameraController.Instance.distFromTarget;
            dof = PPController.Instance.dof.enabled.value;
            CameraController.Instance.camHeight = 20f;
            CameraController.Instance.distFromTarget = 0.1f;
            CameraController.Instance.GetComponentInChildren<Camera>().orthographic = true;
            PPController.Instance.dof.enabled.value = false;
        }

        private void OnDisable()
        {
            PPController.Instance.dof.enabled.value = dof;
            CameraController.Instance.camHeight = camHeight;
            CameraController.Instance.distFromTarget = camDist;
            CameraController.Instance.GetComponentInChildren<Camera>().orthographic = false;
        }
    }
}