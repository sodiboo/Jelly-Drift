using UnityEngine;

// Token: 0x02000007 RID: 7
public class BallCar : MonoBehaviour
{
    // Token: 0x06000016 RID: 22 RVA: 0x000023FE File Offset: 0x000005FE
    private void Awake() => rb = base.GetComponent<Rigidbody>();

    // Token: 0x06000017 RID: 23 RVA: 0x0000240C File Offset: 0x0000060C
    private void Update() => PlayerInput();

    // Token: 0x06000018 RID: 24 RVA: 0x00002414 File Offset: 0x00000614
    private void FixedUpdate()
    {
        var vector = base.transform.InverseTransformDirection(rb.velocity);
        var vector2 = base.transform.InverseTransformDirection((rb.velocity - lastVelocity) / Time.fixedDeltaTime);
        rb.AddTorque(base.transform.up * steering * steeringPower);
        rb.AddForce(throttle * orientation.forward * speed);
        var a = Vector3.Project(rb.velocity, orientation.right);
        var d = 1.5f;
        rb.AddForce(-a * rb.mass * d);
        lastVelocity = rb.velocity;
        var num = vector2.z * 0.25f;
        var z = vector2.x * 0.5f;
        car.transform.localRotation = Quaternion.Euler(-num, 0f, z);
        var force = -C_drag * vector.z * Mathf.Abs(vector.z) * rb.velocity.normalized;
        rb.AddForce(force);
        var force2 = -C_rollFriction * vector.z * rb.velocity.normalized;
        rb.AddForce(force2);
    }

    // Token: 0x06000019 RID: 25 RVA: 0x000025C5 File Offset: 0x000007C5
    private void PlayerInput()
    {
        steering = Input.GetAxisRaw("Horizontal");
        throttle = Input.GetAxis("Vertical");
    }

    // Token: 0x0400001A RID: 26
    private Rigidbody rb;

    // Token: 0x0400001B RID: 27
    public Transform orientation;

    // Token: 0x0400001C RID: 28
    public Transform car;

    // Token: 0x0400001D RID: 29
    private float steering;

    // Token: 0x0400001E RID: 30
    private float throttle;

    // Token: 0x04000020 RID: 32
    private readonly float C_drag = 3.5f;

    // Token: 0x04000021 RID: 33
    private readonly float C_rollFriction = 91f;

    // Token: 0x04000023 RID: 35
    private readonly float speed = 18000f;

    // Token: 0x04000024 RID: 36
    private readonly float steeringPower = 6000f;

    // Token: 0x04000025 RID: 37
    private Vector3 lastVelocity;
}
