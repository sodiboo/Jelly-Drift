using System;
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
		this.rb = base.GetComponent<Rigidbody>();
		if (this.autoValues)
		{
			this.suspensionLength = 0.3f;
			this.suspensionForce = 10f * this.rb.mass;
			this.suspensionDamping = 4f * this.rb.mass;
		}
		AntiRoll[] componentsInChildren = base.gameObject.GetComponentsInChildren<AntiRoll>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].antiRoll = this.antiRoll;
		}
		if (this.centerOfMass)
		{
			this.rb.centerOfMass = this.centerOfMass.localPosition;
		}
		this.c = base.GetComponentInChildren<Collider>();
		this.wheelBase = Vector3.Distance(this.wheelPositions[0].transform.position, this.wheelPositions[2].transform.position);
		this.CG = this.c.bounds.center;
		this.cgHeight = this.c.bounds.extents.y + this.suspensionLength;
		this.cgToFrontAxle = Vector3.Distance(this.wheelPositions[0].transform.position + (this.wheelPositions[1].transform.position - this.wheelPositions[0].transform.position) * 0.5f, this.CG);
		this.cgToRearAxle = Vector3.Distance(this.wheelPositions[2].transform.position + (this.wheelPositions[3].transform.position - this.wheelPositions[2].transform.position) * 0.5f, this.CG);
		this.wheelRadius = this.suspensionLength / 2f;
		this.InitWheels();
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002CB8 File Offset: 0x00000EB8
	private void Update()
	{
		this.MoveWheels();
		this.Audio();
		this.CheckGrounded();
		this.Steering();
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002CD2 File Offset: 0x00000ED2
	private void FixedUpdate()
	{
		this.Movement();
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002CDC File Offset: 0x00000EDC
	private void Audio()
	{
		this.accelerate.volume = Mathf.Lerp(this.accelerate.volume, Mathf.Abs(this.throttle) + Mathf.Abs(this.speed / 80f), Time.deltaTime * 6f);
		this.deaccelerate.volume = Mathf.Lerp(this.deaccelerate.volume, this.speed / 40f - this.throttle * 0.5f, Time.deltaTime * 4f);
		this.accelerate.pitch = Mathf.Lerp(this.accelerate.pitch, 0.65f + Mathf.Clamp(Mathf.Abs(this.speed / 160f), 0f, 1f), Time.deltaTime * 5f);
		if (!this.grounded)
		{
			this.accelerate.pitch = Mathf.Lerp(this.accelerate.pitch, 1.5f, Time.deltaTime * 8f);
		}
		this.deaccelerate.pitch = Mathf.Lerp(this.deaccelerate.pitch, 0.5f + this.speed / 40f, Time.deltaTime * 2f);
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000037 RID: 55 RVA: 0x00002E22 File Offset: 0x00001022
	// (set) Token: 0x06000038 RID: 56 RVA: 0x00002E2A File Offset: 0x0000102A
	public Vector3 acceleration { get; private set; }

	// Token: 0x06000039 RID: 57 RVA: 0x00002E34 File Offset: 0x00001034
	private void Movement()
	{
		Vector3 vector = this.XZVector(this.rb.velocity);
		Vector3 vector2 = base.transform.InverseTransformDirection(this.XZVector(this.rb.velocity));
		this.acceleration = (this.lastVelocity - vector2) / Time.fixedDeltaTime;
		this.dir = Mathf.Sign(base.transform.InverseTransformDirection(vector).z);
		this.speed = vector.magnitude * 3.6f * this.dir;
		float num = Mathf.Abs(this.rb.angularVelocity.y);
		foreach (Suspension suspension in this.wheelPositions)
		{
			if (suspension.grounded)
			{
				Vector3 vector3 = this.XZVector(this.rb.GetPointVelocity(suspension.hitPos));
				base.transform.InverseTransformDirection(vector3);
				Vector3 a = Vector3.Project(vector3, suspension.transform.right);
				float d = 1f;
				float num2 = 1f;
				if (suspension.terrain)
				{
					num2 = 0.6f;
					d = 0.75f;
				}
				float f = Mathf.Atan2(vector2.x, vector2.z);
				if (this.breaking)
				{
					num2 -= 0.6f;
				}
				float num3 = this.driftThreshold;
				if (num > 1f)
				{
					num3 -= 0.2f;
				}
				bool flag = false;
				if (Mathf.Abs(f) > num3)
				{
					float num4 = Mathf.Clamp(Mathf.Abs(f) * 2.4f - num3, 0f, 1f);
					num2 = Mathf.Clamp(1f - num4, 0.05f, 1f);
					float magnitude = this.rb.velocity.magnitude;
					flag = true;
					if (magnitude < 8f)
					{
						num2 += (8f - magnitude) / 8f;
					}
					if (num < this.yawGripThreshold)
					{
						float num5 = (this.yawGripThreshold - num) / this.yawGripThreshold;
						num2 += num5 * this.yawGripMultiplier;
					}
					if (Mathf.Abs(this.throttle) < 0.3f)
					{
						num2 += 0.1f;
					}
					num2 = Mathf.Clamp(num2, 0f, 1f);
				}
				float d2 = 1f;
				if (flag)
				{
					d2 = this.driftMultiplier;
				}
				if (this.breaking)
				{
					this.rb.AddForceAtPosition(suspension.transform.forward * this.C_breaking * Mathf.Sign(-this.speed) * d, suspension.hitPos);
				}
				this.rb.AddForceAtPosition(suspension.transform.forward * this.throttle * this.engineForce * d2 * d, suspension.hitPos);
				Vector3 a2 = a * this.rb.mass * d * num2;
				this.rb.AddForceAtPosition(-a2, suspension.hitPos);
				this.rb.AddForceAtPosition(suspension.transform.forward * a2.magnitude * 0.25f, suspension.hitPos);
				float num6 = Mathf.Clamp(1f - num2, 0f, 1f);
				if (Mathf.Sign(this.dir) != Mathf.Sign(this.throttle) && this.speed > 2f)
				{
					num6 = Mathf.Clamp(num6 + 0.5f, 0f, 1f);
				}
				suspension.traction = num6;
				Vector3 force = -this.C_drag * vector;
				this.rb.AddForce(force);
				Vector3 force2 = -this.C_rollFriction * vector;
				this.rb.AddForce(force2);
			}
		}
		this.StandStill();
		this.lastVelocity = vector2;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00003234 File Offset: 0x00001434
	private void StandStill()
	{
		if (Mathf.Abs(this.speed) >= 1f || !this.grounded || this.throttle != 0f)
		{
			this.rb.drag = 0f;
			return;
		}
		bool flag = true;
		Suspension[] array = this.wheelPositions;
		for (int i = 0; i < array.Length; i++)
		{
			if (Vector3.Angle(array[i].hitNormal, Vector3.up) > 1f)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			this.rb.drag = (1f - Mathf.Abs(this.speed)) * 30f;
			return;
		}
		this.rb.drag = 0f;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000032E8 File Offset: 0x000014E8
	private void Steering()
	{
		foreach (Suspension suspension in this.wheelPositions)
		{
			if (!suspension.rearWheel)
			{
				suspension.steeringAngle = this.steering * (37f - Mathf.Clamp(this.speed * 0.35f - 2f, 0f, 17f));
				this.steerAngle = suspension.steeringAngle * ChaosController.Instance?.size ?? 1;
			}
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003356 File Offset: 0x00001556
	private Vector3 XZVector(Vector3 v)
	{
		return new Vector3(v.x, 0f, v.z);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00003370 File Offset: 0x00001570
	private void InitWheels()
	{
		foreach (Suspension suspension in this.wheelPositions)
		{
			suspension.wheelObject = UnityEngine.Object.Instantiate<GameObject>(this.wheel).transform;
			suspension.wheelObject.parent = suspension.transform;
			suspension.wheelObject.transform.localPosition = Vector3.zero;
			suspension.wheelObject.transform.localRotation = Quaternion.identity;
			suspension.wheelObject.localScale = Vector3.one * this.suspensionLength * 2f;
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003410 File Offset: 0x00001610
	private void MoveWheels()
	{
		foreach (Suspension suspension in this.wheelPositions)
		{
			float num = this.suspensionLength;
			float hitHeight = suspension.hitHeight;
			float y = Mathf.Lerp(suspension.wheelObject.transform.localPosition.y, -hitHeight + num, Time.deltaTime * 20f);
			float num2 = 0.2f * this.suspensionLength * 2f;
			if (suspension.transform.localPosition.x < 0f)
			{
				num2 = -num2;
			}
			num2 = 0f;
			suspension.wheelObject.transform.localPosition = new Vector3(num2, y, 0f);
			suspension.wheelObject.Rotate(Vector3.right, this.XZVector(this.rb.velocity).magnitude * 1f * this.dir);
			suspension.wheelObject.localScale = Vector3.one * (this.suspensionLength * 2f);
			suspension.transform.localScale = Vector3.one / base.transform.localScale.x;
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00003548 File Offset: 0x00001748
	private void CheckGrounded()
	{
		this.grounded = false;
		Suspension[] array = this.wheelPositions;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].grounded)
			{
				this.grounded = true;
			}
		}
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

	// Token: 0x0400003E RID: 62
	private Collider c;

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
	private float C_drag = 3.5f;

	// Token: 0x0400004E RID: 78
	private float C_rollFriction = 105f;

	// Token: 0x0400004F RID: 79
	private float C_breaking = 3000f;

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

	// Token: 0x04000056 RID: 86
	private Vector3 CG;

	// Token: 0x04000057 RID: 87
	private float cgHeight;

	// Token: 0x04000058 RID: 88
	private float wheelBase;

	// Token: 0x04000059 RID: 89
	private float axleWeightRatioFront = 0.5f;

	// Token: 0x0400005A RID: 90
	private float axleWeightRatioRear = 0.5f;

	// Token: 0x0400005B RID: 91
	private float wheelRadius;

	// Token: 0x0400005D RID: 93
	private float yawRate;

	// Token: 0x0400005E RID: 94
	private float weightTransfer = 0.2f;

	// Token: 0x0400005F RID: 95
	private float cgToRearAxle;

	// Token: 0x04000060 RID: 96
	private float cgToFrontAxle;

	// Token: 0x04000061 RID: 97
	private float tireGrip = 2f;

	// Token: 0x04000062 RID: 98
	private float lockGrip = 0.7f;

	// Token: 0x04000063 RID: 99
	private float cornerStiffnessFront = 5f;

	// Token: 0x04000064 RID: 100
	private float cornerStiffnessRear = 5.2f;

	// Token: 0x04000065 RID: 101
	private float yawGripThreshold = 0.6f;

	// Token: 0x04000066 RID: 102
	private float yawGripMultiplier = 0.15f;

	// Token: 0x04000068 RID: 104
	public bool yes;
}
