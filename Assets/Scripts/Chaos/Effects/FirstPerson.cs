using UnityEngine;

namespace Chaos
{
    [Effect("chaos.firstperson", "First Person", EffectInfo.Alignment.Neutral), ConflictsWith(typeof(IsoView))]
    [Description("Puts your camera on the hood of your car (or the stalk of the banana)")]
    public class FirstPerson : ChaosEffect
    {
        public static bool value; // checked in CameraController.Update to prevent regular camera motion
        private Camera cam;
        protected override void Enable()
        {
            value = true;
            CameraController.Instance.transform.parent = car.transform;
            cam = CameraController.Instance.GetComponentInChildren<Camera>();
        }

        protected override void Disable()
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