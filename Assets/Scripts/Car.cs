using TMPro;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class Car : MonoBehaviour
{
    // Token: 0x17000001 RID: 1
    // (get) Token: 0x06000027 RID: 39 RVA: 0x00002A66 File Offset: 0x00000C66
    // (set) Token: 0x06000028 RID: 40 RVA: 0x00002A6E File Offset: 0x00000C6E
    public Rigidbody rb { get; set; }

    // Token: 0x17000002 RID: 2
    // (get) Token: 0x06000029 RID: 41 RVA: 0x00002A77 File Offset: 0x00000C77
    // (set) Token: 0x0600002A RID: 42 RVA: 0x00002A7F File Offset: 0x00000C7F
    public float steering { get; set; }

    // Token: 0x17000003 RID: 3
    // (get) Token: 0x0600002B RID: 43 RVA: 0x00002A88 File Offset: 0x00000C88
    // (set) Token: 0x0600002C RID: 44 RVA: 0x00002A90 File Offset: 0x00000C90
    public float throttle { get; set; }

    // Token: 0x17000004 RID: 4
    // (get) Token: 0x0600002D RID: 45 RVA: 0x00002A99 File Offset: 0x00000C99
    // (set) Token: 0x0600002E RID: 46 RVA: 0x00002AA1 File Offset: 0x00000CA1
    public bool breaking { get; set; }

    // Token: 0x17000005 RID: 5
    // (get) Token: 0x0600002F RID: 47 RVA: 0x00002AAA File Offset: 0x00000CAA
    // (set) Token: 0x06000030 RID: 48 RVA: 0x00002AB2 File Offset: 0x00000CB2
    public float speed { get; private set; }

    // Token: 0x17000006 RID: 6
    // (get) Token: 0x06000031 RID: 49 RVA: 0x00002ABB File Offset: 0x00000CBB
    // (set) Token: 0x06000032 RID: 50 RVA: 0x00002AC3 File Offset: 0x00000CC3
    public float steerAngle { get; set; }

    // Token: 0x06000033 RID: 51 RVA: 0x00002ACC File Offset: 0x00000CCC
    private void Awake()
    {
        suspensionLayers ^= (1 << LayerMask.NameToLayer("Trigger")) | (1 << LayerMask.NameToLayer("Ghost"));
        rb = base.GetComponent<Rigidbody>();
        if (autoValues)
        {
            suspensionLength = 0.3f;
            suspensionForce = 10f * rb.mass;
            suspensionDamping = 4f * rb.mass;
        }
        var componentsInChildren = base.gameObject.GetComponentsInChildren<AntiRoll>();
        for (var i = 0; i < componentsInChildren.Length; i++)
        {
            componentsInChildren[i].antiRoll = antiRoll;
        }
        if (centerOfMass)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }
        InitWheels();
    }

    // Token: 0x06000034 RID: 52 RVA: 0x00002CB8 File Offset: 0x00000EB8
    private void Update()
    {
        MoveWheels();
        Audio();
        CheckGrounded();
        Steering();
    }

    // Token: 0x06000035 RID: 53 RVA: 0x00002CD2 File Offset: 0x00000ED2
    private void FixedUpdate()
    {
        Movement();
        WrapAround();
    }

    // Token: 0x06000036 RID: 54 RVA: 0x00002CDC File Offset: 0x00000EDC
    private void Audio()
    {
        accelerate.volume = Mathf.Lerp(accelerate.volume, Mathf.Abs(throttle) + Mathf.Abs(speed / 80f), Time.deltaTime * 6f);
        deaccelerate.volume = Mathf.Lerp(deaccelerate.volume, speed / 40f - throttle * 0.5f, Time.deltaTime * 4f);
        accelerate.pitch = Mathf.Lerp(accelerate.pitch, 0.65f + Mathf.Clamp(Mathf.Abs(speed / 160f), 0f, 1f), Time.deltaTime * 5f);
        if (!grounded)
        {
            accelerate.pitch = Mathf.Lerp(accelerate.pitch, 1.5f, Time.deltaTime * 8f);
        }
        deaccelerate.pitch = Mathf.Lerp(deaccelerate.pitch, 0.5f + speed / 40f, Time.deltaTime * 2f);
    }

    // Token: 0x17000007 RID: 7
    // (get) Token: 0x06000037 RID: 55 RVA: 0x00002E22 File Offset: 0x00001022
    // (set) Token: 0x06000038 RID: 56 RVA: 0x00002E2A File Offset: 0x0000102A
    public Vector3 acceleration { get; private set; }

    // Token: 0x06000039 RID: 57 RVA: 0x00002E34 File Offset: 0x00001034
    private void Movement()
    {
        drifting = false;
        var vector = XZVector(rb.velocity);
        var vector2 = base.transform.InverseTransformDirection(XZVector(rb.velocity));
        acceleration = (lastVelocity - vector2) / Time.fixedDeltaTime;
        dir = Mathf.Sign(base.transform.InverseTransformDirection(vector).z);
        speed = vector.magnitude * 3.6f * dir;
        var num = Mathf.Abs(rb.angularVelocity.y);
        foreach (var suspension in wheelPositions)
        {
            if (suspension.grounded)
            {
                var vector3 = XZVector(rb.GetPointVelocity(suspension.hitPos));
                base.transform.InverseTransformDirection(vector3);
                var a = Vector3.Project(vector3, suspension.transform.right);
                var d = 1f;
                var num2 = 1f;
                if (suspension.terrain)
                {
                    num2 = 0.6f;
                    d = 0.75f;
                }
                var f = Mathf.Atan2(vector2.x, vector2.z);
                if (breaking)
                {
                    num2 -= 0.6f;
                }
                var num3 = driftThreshold;
                if (num > 1f)
                {
                    num3 -= 0.2f;
                }
                if (Mathf.Abs(f) > num3)
                {
                    var num4 = Mathf.Clamp(Mathf.Abs(f) * 2.4f - num3, 0f, 1f);
                    num2 = Mathf.Clamp(1f - num4, 0.05f, 1f);
                    var magnitude = rb.velocity.magnitude;
                    drifting = true;
                    if (magnitude < 8f)
                    {
                        num2 += (8f - magnitude) / 8f;
                    }
                    if (num < yawGripThreshold)
                    {
                        var num5 = (yawGripThreshold - num) / yawGripThreshold;
                        num2 += num5 * yawGripMultiplier;
                    }
                    if (Mathf.Abs(throttle) < 0.3f)
                    {
                        num2 += 0.1f;
                    }
                    num2 = Mathf.Clamp(num2, 0f, 1f);
                }
                var d2 = 1f;
                if (drifting)
                {
                    d2 = driftMultiplier;
                }
                if (breaking)
                {
                    rb.AddForceAtPosition(suspension.transform.forward * C_breaking * Mathf.Sign(-speed) * d, suspension.hitPos);
                }
                rb.AddForceAtPosition(suspension.transform.forward * throttle * engineForce * d2 * d, suspension.hitPos);
                var a2 = a * rb.mass * d * num2;
                rb.AddForceAtPosition(-a2, suspension.hitPos);
                rb.AddForceAtPosition(suspension.transform.forward * a2.magnitude * 0.25f, suspension.hitPos);
                var num6 = Mathf.Clamp(1f - num2, 0f, 1f);
                if (Mathf.Sign(dir) != Mathf.Sign(throttle) && speed > 2f)
                {
                    num6 = Mathf.Clamp(num6 + 0.5f, 0f, 1f);
                }
                suspension.traction = num6;
                var force = -C_drag * vector;
                rb.AddForce(force);
                var force2 = -C_rollFriction * vector;
                rb.AddForce(force2);
            }
        }
        StandStill();
        lastVelocity = vector2;
    }

    // Token: 0x0600003A RID: 58 RVA: 0x00003234 File Offset: 0x00001434
    private void StandStill()
    {
        if (Mathf.Abs(speed) >= 1f || !grounded || throttle != 0f)
        {
            rb.drag = 0f;
            return;
        }
        var flag = true;
        var array = wheelPositions;
        for (var i = 0; i < array.Length; i++)
        {
            if (Vector3.Angle(array[i].hitNormal, Vector3.up) > 1f)
            {
                flag = false;
                break;
            }
        }
        if (flag)
        {
            rb.drag = (1f - Mathf.Abs(speed)) * 30f;
            return;
        }
        rb.drag = 0f;
    }

    // Token: 0x0600003B RID: 59 RVA: 0x000032E8 File Offset: 0x000014E8
    private void Steering()
    {
        foreach (var suspension in wheelPositions)
        {
            if (!suspension.rearWheel)
            {
                suspension.steeringAngle = steering * (37f - Mathf.Clamp(speed * 0.35f - 2f, 0f, 17f)) * Chaos.Scale.value;
                steerAngle = suspension.steeringAngle;
            }
        }
    }

    // Token: 0x0600003C RID: 60 RVA: 0x00003356 File Offset: 0x00001556
    private Vector3 XZVector(Vector3 v) => new Vector3(v.x, 0f, v.z);

    // Token: 0x0600003D RID: 61 RVA: 0x00003370 File Offset: 0x00001570
    private void InitWheels()
    {
        foreach (var suspension in wheelPositions)
        {
            suspension.wheelObject = UnityEngine.Object.Instantiate<GameObject>(wheel).transform;
            suspension.wheelObject.parent = suspension.transform;
            suspension.wheelObject.transform.localPosition = Vector3.zero;
            suspension.wheelObject.transform.localRotation = Quaternion.identity;
            suspension.wheelObject.localScale = Vector3.one * suspensionLength * 2f;
        }
    }

    // Token: 0x0600003E RID: 62 RVA: 0x00003410 File Offset: 0x00001610
    private void MoveWheels()
    {
        foreach (var suspension in wheelPositions)
        {
            var num = suspensionLength;
            var hitHeight = suspension.hitHeight;
            var y = Mathf.Lerp(suspension.wheelObject.transform.localPosition.y, -hitHeight + num, Time.deltaTime * 20f);
            suspension.wheelObject.transform.localPosition = new Vector3(0f, y, 0f);
            suspension.wheelObject.Rotate(Vector3.right, XZVector(rb.velocity).magnitude * 1f * dir);
            suspension.wheelObject.localScale = Vector3.one * (suspensionLength * 2f);
            suspension.transform.localScale = Vector3.one / base.transform.localScale.x;
        }
    }

    // Token: 0x0600003F RID: 63 RVA: 0x00003548 File Offset: 0x00001748
    private void CheckGrounded()
    {
        grounded = false;
        var array = wheelPositions;
        for (var i = 0; i < array.Length; i++)
        {
            if (array[i].grounded)
            {
                grounded = true;
            }
        }
    }

    private void WrapAround()
    {
        if (rb.position.y < -500f)
        {
            var cam = CameraController.Instance.transform;
            var diff = cam.position - rb.position;
            rb.position = new Vector3(rb.position.x, 200f, rb.position.z);
            cam.position = rb.position + diff * 100f;
        }
        if (rb.position.y > 100f) rb.drag = 0.2f;
    }

    // Token: 0x0400003A RID: 58
    [Header("Misc")]
    public Transform centerOfMass;

    // Token: 0x0400003B RID: 59
    public Suspension[] wheelPositions;

    // Token: 0x0400003C RID: 60
    public GameObject wheel;

    // Token: 0x0400003D RID: 61
    public TextMeshProUGUI text;
    public new GameObject collider;

    // Token: 0x0400003F RID: 63
    [Header("Suspension Variables")]
    public bool autoValues;

    // Token: 0x04000040 RID: 64
    public float suspensionLength;

    // Token: 0x04000041 RID: 65
    public float restHeight;

    // Token: 0x04000042 RID: 66
    public float suspensionForce;

    // Token: 0x04000043 RID: 67
    public float suspensionDamping;

    // Token: 0x04000047 RID: 71
    [Header("Car specs")]
    public float engineForce = 5000f;

    // Token: 0x04000048 RID: 72
    public float steerForce = 1f;

    // Token: 0x04000049 RID: 73
    public float antiRoll = 5000f;

    // Token: 0x0400004A RID: 74
    public float stability;

    // Token: 0x0400004B RID: 75
    [Header("Drift specs")]
    public float driftMultiplier = 1f;

    // Token: 0x0400004C RID: 76
    public float driftThreshold = 0.5f;

    // Token: 0x0400004D RID: 77
    private readonly float C_drag = 3.5f;

    // Token: 0x0400004E RID: 78
    private readonly float C_rollFriction = 105f;

    // Token: 0x0400004F RID: 79
    private readonly float C_breaking = 3000f;

    // Token: 0x04000050 RID: 80
    [Header("Audio Sources")]
    public AudioSource accelerate;

    // Token: 0x04000051 RID: 81
    public AudioSource deaccelerate;

    // Token: 0x04000053 RID: 83
    private float dir;

    // Token: 0x04000054 RID: 84
    private Vector3 lastVelocity;

    // Token: 0x04000055 RID: 85
    private bool grounded;

    // Token: 0x04000065 RID: 101
    private readonly float yawGripThreshold = 0.6f;

    // Token: 0x04000066 RID: 102
    private readonly float yawGripMultiplier = 0.15f;

    public float firstPersonDistance;
    public float firstPersonHeight;
    public int suspensionLayers = Physics.AllLayers;
    public bool drifting;
}
