using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class Suspension : MonoBehaviour
{
	// Token: 0x060001C4 RID: 452 RVA: 0x00009520 File Offset: 0x00007720
	private void Start()
	{
		this.car = base.transform.parent.GetComponent<Car>();
		this.bodyRb = this.car.GetComponent<Rigidbody>();
		this.raycastOffset = this.car.suspensionLength * 0.5f;
		this.smokeEmitting = this.smokeFx.emission;
		this.spinEmitting = this.spinFx.emission;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000958D File Offset: 0x0000778D
	private void FixedUpdate()
	{
		this.NewSuspension();
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00009598 File Offset: 0x00007798
	private void Update()
	{
		this.DebugTraction();
		if (this.rearWheel)
		{
			return;
		}
		this.wheelAngleVelocity = Mathf.Lerp(this.wheelAngleVelocity, this.steeringAngle, this.steerTime * Time.deltaTime);
		base.transform.localRotation = Quaternion.Euler(Vector3.up * this.wheelAngleVelocity);
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x000020AB File Offset: 0x000002AB
	private void DebugTraction()
	{
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x060001C8 RID: 456 RVA: 0x000095F7 File Offset: 0x000077F7
	// (set) Token: 0x060001C9 RID: 457 RVA: 0x000095FF File Offset: 0x000077FF
	public bool terrain { get; set; }

	// Token: 0x060001CA RID: 458 RVA: 0x00009608 File Offset: 0x00007808
	private void NewSuspension()
	{
		this.minLength = this.restLength - this.springTravel;
		this.maxLength = this.restLength + this.springTravel;
		float suspensionLength = this.car.suspensionLength;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, -base.transform.up, out raycastHit, this.maxLength + suspensionLength))
		{
			this.lastLength = this.springLength;
			this.springLength = raycastHit.distance - suspensionLength;
			this.springLength = Mathf.Clamp(this.springLength, this.minLength, this.maxLength);
			this.springVelocity = (this.lastLength - this.springLength) / Time.fixedDeltaTime;
			this.springForce = this.springStiffness * (this.restLength - this.springLength);
			this.damperForce = this.damperStiffness * this.springVelocity;
			Vector3 force = (this.springForce + this.damperForce) * base.transform.up;
			this.bodyRb.AddForceAtPosition(force, raycastHit.point);
			this.terrain = raycastHit.collider.gameObject.CompareTag("Terrain");
			if (ChaosController.Instance?.invertTerrain == true) this.terrain = !this.terrain;
			this.hitPos = raycastHit.point;
			this.hitNormal = raycastHit.normal;
			this.hitHeight = raycastHit.distance;
			this.grounded = true;
			return;
		}
		this.grounded = false;
		this.hitHeight = this.car.suspensionLength + this.car.restHeight;
	}

	// Token: 0x060001CB RID: 459 RVA: 0x00009794 File Offset: 0x00007994
	private void LateUpdate()
	{
		if (!this.showFx)
		{
			return;
		}
		if (this.traction > 0.05f && this.hitPos != Vector3.zero && this.grounded)
		{
			this.smokeEmitting.enabled = true;
			if (Skidmarks.Instance)
			{
				this.lastSkid = Skidmarks.Instance.AddSkidMark(this.hitPos + this.bodyRb.velocity * Time.fixedDeltaTime, this.hitNormal, this.traction * 0.9f, this.lastSkid);
			}
		}
		else
		{
			this.smokeEmitting.enabled = false;
			this.lastSkid = -1;
		}
		if (this.skidSfx)
		{
			float num = 1f;
			if (this.bodyRb.velocity.magnitude < 2f)
			{
				num = 0f;
			}
			this.skidSfx.volume = this.traction * num;
			this.skidSfx.pitch = 0.3f + 0.4f * Mathf.Clamp(this.traction * 0.5f, 0f, 1f);
		}
		if (!this.rearWheel)
		{
			return;
		}
		if (this.traction > 0.15f && this.grounded)
		{
			this.spinEmitting.enabled = true;
			this.spinEmitting.rateOverTime = Mathf.Clamp(this.traction * 60f, 20f, 400f);
			return;
		}
		this.spinEmitting.enabled = false;
	}

	// Token: 0x040001C9 RID: 457
	private Car car;

	// Token: 0x040001CA RID: 458
	private Rigidbody bodyRb;

	// Token: 0x040001CB RID: 459
	public Transform wheelObject;

	// Token: 0x040001CC RID: 460
	public bool rearWheel;

	// Token: 0x040001CD RID: 461
	private int lastSkid;

	// Token: 0x040001CE RID: 462
	[HideInInspector]
	public bool skidding;

	// Token: 0x040001CF RID: 463
	[HideInInspector]
	public float grip;

	// Token: 0x040001D0 RID: 464
	public bool showFx = true;

	// Token: 0x040001D1 RID: 465
	public AudioSource skidSfx;

	// Token: 0x040001D2 RID: 466
	public ParticleSystem smokeFx;

	// Token: 0x040001D3 RID: 467
	public ParticleSystem spinFx;

	// Token: 0x040001D4 RID: 468
	private ParticleSystem.EmissionModule smokeEmitting;

	// Token: 0x040001D5 RID: 469
	private ParticleSystem.EmissionModule spinEmitting;

	// Token: 0x040001D6 RID: 470
	private float wheelAngleVelocity;

	// Token: 0x040001D7 RID: 471
	public float steeringAngle;

	// Token: 0x040001D8 RID: 472
	public float traction;

	// Token: 0x040001D9 RID: 473
	private float steerTime = 15f;

	// Token: 0x040001DA RID: 474
	public bool spinning;

	// Token: 0x040001DB RID: 475
	public LayerMask whatIsGround;

	// Token: 0x040001DC RID: 476
	private MeshRenderer mesh;

	// Token: 0x040001DD RID: 477
	public Vector3 hitPos;

	// Token: 0x040001DE RID: 478
	public Vector3 hitNormal;

	// Token: 0x040001DF RID: 479
	public float hitHeight;

	// Token: 0x040001E0 RID: 480
	public bool grounded;

	// Token: 0x040001E1 RID: 481
	public float lastCompression;

	// Token: 0x040001E2 RID: 482
	private float raycastOffset;

	// Token: 0x040001E3 RID: 483
	private float maxEmission;

	// Token: 0x040001E5 RID: 485
	public float restLength;

	// Token: 0x040001E6 RID: 486
	public float springTravel;

	// Token: 0x040001E7 RID: 487
	public float springStiffness;

	// Token: 0x040001E8 RID: 488
	public float damperStiffness;

	// Token: 0x040001E9 RID: 489
	private float minLength;

	// Token: 0x040001EA RID: 490
	private float maxLength;

	// Token: 0x040001EB RID: 491
	private float lastLength;

	// Token: 0x040001EC RID: 492
	private float springLength;

	// Token: 0x040001ED RID: 493
	private float springVelocity;

	// Token: 0x040001EE RID: 494
	private float springForce;

	// Token: 0x040001EF RID: 495
	private float damperForce;
}
