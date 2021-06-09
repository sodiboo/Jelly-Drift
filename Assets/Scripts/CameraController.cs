using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class CameraController : MonoBehaviour
{
    // Token: 0x0600001E RID: 30 RVA: 0x0000266C File Offset: 0x0000086C
    private void Awake()
    {
        CameraController.Instance = this;
        if (target != null)
        {
            AssignTarget(target);
        }
        mainCam = base.GetComponentInChildren<Camera>();
        speedometer = Instantiate(PrefabManager.Instance.speedometer).GetComponent<TextMesh>();
    }

    // Token: 0x0600001F RID: 31 RVA: 0x0000269A File Offset: 0x0000089A
    public void AssignTarget(Transform target)
    {
        MonoBehaviour.print("assinging target");
        this.target = target;
        targetRb = target.GetComponent<Rigidbody>();
        targetCar = target.GetComponent<Car>();
        targetSkin = targetCar.GetComponent<CarSkin>();
    }

    // Token: 0x06000020 RID: 32 RVA: 0x000026C8 File Offset: 0x000008C8
    private void Update()
    {
        if (!target)
        {
            return;
        }
        if (Chaos.FirstPerson.value) return;

        var normalized = new Vector3(target.forward.x, 0f, target.forward.z).normalized;
        var a = new Vector3(targetRb.velocity.x, 0f, targetRb.velocity.z).normalized;
        if ((targetCar.speed < 5f && targetCar.speed > -15f) || SaveState.Instance.cameraMode == 1)
        {
            a = Vector3.zero;
        }
        var a2 = normalized * 0.2f + a * 0.8f;
        a2.Normalize();
        desiredPosition = target.position + -a2 * distFromTarget + Vector3.up * camHeight + offset;
        base.transform.position = Vector3.Lerp(base.transform.position, desiredPosition, Time.deltaTime * moveSpeed);
        var d = targetRb.velocity.magnitude * 0.25f;
        var forward = target.position - desiredPosition + d * a2 + d * Vector3.down * 0.3f;
        desiredLook = Quaternion.LookRotation(forward);
        base.transform.rotation = Quaternion.Lerp(base.transform.rotation, desiredLook, Time.deltaTime * rotationSpeed);
        var b = (float)Mathf.Clamp(70 + (int)(targetRb.velocity.magnitude * 0.35f), 70, 85);
        fov = Mathf.Lerp(fov, b, Time.deltaTime * 5f);
        mainCam.fieldOfView = fov;
        offset = Vector3.Lerp(offset, Vector3.zero, Time.deltaTime * offsetSpeed);
        if (targetCar.acceleration.y > shakeThreshold)
        {
            var d2 = (Mathf.Clamp(targetCar.acceleration.y, shakeThreshold, 50f) - shakeThreshold / 2f) * 0.14f;
            OffsetCamera(Vector3.down * d2);
        }
    }

    private void LateUpdate()
    {
        if (speedometer != null)
        {
            if (targetSkin.skinsToChange.Length > 2)
            {
                var data = targetSkin.skinsToChange[GameState.Instance.skin].myArray;
                var materials = targetSkin.renderers[data[0]].materials;
                speedometer.color = materials[Math.Min(data[1], materials.Length - 1)].color;
            }
            switch (SaveState.Instance.speedometer)
            {
                case 0:
                    speedometer.text = "";
                    return;
                case 1:
                    speedometer.text = $"{targetRb.velocity.magnitude:0.0} u/s";
                    goto relocate;
                case 2:
                    speedometer.text = $"{targetRb.velocity.magnitude * 3.6:0.0} ku/h";
                    goto relocate;

                relocate:
                    speedometer.transform.rotation = mainCam.transform.rotation;
                    speedometer.transform.position = targetCar.transform.position + mainCam.transform.right * 10;
                    break;
            }
        }
    }

    private void OnDisable()
    {
        if (speedometer != null) speedometer.text = "";
    }

    // Token: 0x06000021 RID: 33 RVA: 0x00002982 File Offset: 0x00000B82
    public void OffsetCamera(Vector3 offset)
    {
        if (!readyToOffset)
        {
            return;
        }
        this.offset += offset;
        readyToOffset = false;
        base.Invoke("GetReady", 0.5f);
        ShakeController.Instance.Shake();
    }

    // Token: 0x06000022 RID: 34 RVA: 0x000029C0 File Offset: 0x00000BC0
    private void GetReady() => readyToOffset = true;

    // Token: 0x04000027 RID: 39
    public Transform target;

    // Token: 0x04000028 RID: 40
    private Rigidbody targetRb;

    // Token: 0x04000029 RID: 41
    private Car targetCar;
    private CarSkin targetSkin;

    // Token: 0x0400002A RID: 42
    private Vector3 desiredPosition;

    // Token: 0x0400002B RID: 43
    private Vector3 offset;

    // Token: 0x0400002C RID: 44
    private Quaternion desiredLook;

    // Token: 0x0400002D RID: 45
    public float moveSpeed;

    // Token: 0x0400002E RID: 46
    public float rotationSpeed;

    // Token: 0x0400002F RID: 47
    public float distFromTarget;

    // Token: 0x04000030 RID: 48
    public float camHeight;

    // Token: 0x04000031 RID: 49
    public float offsetSpeed = 1.5f;

    // Token: 0x04000032 RID: 50
    private Camera mainCam;

    // Token: 0x04000033 RID: 51
    public static CameraController Instance;

    // Token: 0x04000034 RID: 52
    private float fov;

    // Token: 0x04000035 RID: 53
    private readonly float shakeThreshold = 16f;

    // Token: 0x04000036 RID: 54
    private bool readyToOffset = true;

    private TextMesh speedometer;
}
