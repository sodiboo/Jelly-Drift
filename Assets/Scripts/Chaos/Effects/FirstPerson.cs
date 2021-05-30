using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.firstperson", "First Person"), ConflictsWith(typeof(IsoView), typeof(Multiplayer))]
    public class FirstPerson : ChaosEffect
    {
        public static bool value; // checked in CameraController.Update to prevent regular camera motion
        Camera cam;
        private void OnEnable()
        {
            value = true;
            CameraController.Instance.transform.parent = car.transform;
            cam = CameraController.Instance.GetComponentInChildren<Camera>();
        }

        private void OnDisable()
        {
            value = false;
            CameraController.Instance.transform.parent = null;
        }

        private void Update()
        {
            CameraController.Instance.transform.localPosition = Vector3.Lerp(CameraController.Instance.transform.localPosition, new Vector3(0f, car.firstPersonHeight, car.firstPersonDistance), Time.deltaTime * 3f);
            CameraController.Instance.transform.localRotation = Quaternion.Lerp(CameraController.Instance.transform.localRotation, Quaternion.identity, Time.deltaTime);
            cam.fieldOfView = 60f; // needed for Quake FOV compat
        }
    }
}