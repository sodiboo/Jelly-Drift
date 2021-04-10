using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class BallCar : MonoBehaviour
{
	// Token: 0x06000016 RID: 22 RVA: 0x000023FE File Offset: 0x000005FE
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000240C File Offset: 0x0000060C
	private void Update()
	{
		this.PlayerInput();
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002414 File Offset: 0x00000614
	private void FixedUpdate()
	{
		Vector3 vector = base.transform.InverseTransformDirection(this.rb.velocity);
		Vector3 vector2 = base.transform.InverseTransformDirection((this.rb.velocity - this.lastVelocity) / Time.fixedDeltaTime);
		this.rb.AddTorque(base.transform.up * this.steering * this.steeringPower);
		this.rb.AddForce(this.throttle * this.orientation.forward * this.speed);
		Vector3 a = Vector3.Project(this.rb.velocity, this.orientation.right);
		float d = 1.5f;
		this.rb.AddForce(-a * this.rb.mass * d);
		this.lastVelocity = this.rb.velocity;
		float num = vector2.z * 0.25f;
		float z = vector2.x * 0.5f;
		this.car.transform.localRotation = Quaternion.Euler(-num, 0f, z);
		Vector3 force = -this.C_drag * vector.z * Mathf.Abs(vector.z) * this.rb.velocity.normalized;
		this.rb.AddForce(force);
		Vector3 force2 = -this.C_rollFriction * vector.z * this.rb.velocity.normalized;
		this.rb.AddForce(force2);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000025C5 File Offset: 0x000007C5
	private void PlayerInput()
	{
		this.steering = Input.GetAxisRaw("Horizontal");
		this.throttle = Input.GetAxis("Vertical");
		this.breaking = Input.GetButton("Breaking");
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

	// Token: 0x0400001F RID: 31
	private bool breaking;

	// Token: 0x04000020 RID: 32
	private float C_drag = 3.5f;

	// Token: 0x04000021 RID: 33
	private float C_rollFriction = 91f;

	// Token: 0x04000022 RID: 34
	private float C_breaking = 3000f;

	// Token: 0x04000023 RID: 35
	private float speed = 18000f;

	// Token: 0x04000024 RID: 36
	private float steeringPower = 6000f;

	// Token: 0x04000025 RID: 37
	private Vector3 lastVelocity;
}
