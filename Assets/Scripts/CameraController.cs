using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class CameraController : MonoBehaviour
{
	// Token: 0x0600001E RID: 30 RVA: 0x0000266C File Offset: 0x0000086C
	private void Awake()
	{
		CameraController.Instance = this;
		if (this.target != null)
		{
			this.AssignTarget(this.target);
		}
		this.mainCam = base.GetComponentInChildren<Camera>();
	}

	// Token: 0x0600001F RID: 31 RVA: 0x0000269A File Offset: 0x0000089A
	public void AssignTarget(Transform target)
	{
		MonoBehaviour.print("assinging target");
		this.target = target;
		this.targetRb = target.GetComponent<Rigidbody>();
		this.targetCar = target.GetComponent<Car>();
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000026C8 File Offset: 0x000008C8
	private void Update()
	{
		if (!this.target)
		{
			return;
		}
		if (Chaos.FirstPerson.value) return;

		Vector3 normalized = new Vector3(this.target.forward.x, 0f, this.target.forward.z).normalized;
		Vector3 a = new Vector3(this.targetRb.velocity.x, 0f, this.targetRb.velocity.z).normalized;
		if ((this.targetCar.speed < 5f && this.targetCar.speed > -15f) || SaveState.Instance.cameraMode == 1)
		{
			a = Vector3.zero;
		}
		Vector3 a2 = normalized * 0.2f + a * 0.8f;
		a2.Normalize();
		this.desiredPosition = this.target.position + -a2 * this.distFromTarget + Vector3.up * this.camHeight + this.offset;
		base.transform.position = Vector3.Lerp(base.transform.position, this.desiredPosition, Time.deltaTime * this.moveSpeed);
		float d = this.targetRb.velocity.magnitude * 0.25f;
		Vector3 forward = this.target.position - this.desiredPosition + d * a2 + d * Vector3.down * 0.3f;
		this.desiredLook = Quaternion.LookRotation(forward);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.desiredLook, Time.deltaTime * this.rotationSpeed);
		float b = (float)Mathf.Clamp(70 + (int)(this.targetRb.velocity.magnitude * 0.35f), 70, 85);
		this.fov = Mathf.Lerp(this.fov, b, Time.deltaTime * 5f);
		this.mainCam.fieldOfView = this.fov;
		this.offset = Vector3.Lerp(this.offset, Vector3.zero, Time.deltaTime * this.offsetSpeed);
		if (this.targetCar.acceleration.y > this.shakeThreshold)
		{
			float d2 = (Mathf.Clamp(this.targetCar.acceleration.y, this.shakeThreshold, 50f) - this.shakeThreshold / 2f) * 0.14f;
			this.OffsetCamera(Vector3.down * d2);
		}
	}

    // Token: 0x06000021 RID: 33 RVA: 0x00002982 File Offset: 0x00000B82
    public void OffsetCamera(Vector3 offset)
	{
		if (!this.readyToOffset)
		{
			return;
		}
		this.offset += offset;
		this.readyToOffset = false;
		base.Invoke("GetReady", 0.5f);
		ShakeController.Instance.Shake();
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000029C0 File Offset: 0x00000BC0
	private void GetReady()
	{
		this.readyToOffset = true;
	}

	// Token: 0x04000027 RID: 39
	public Transform target;

	// Token: 0x04000028 RID: 40
	private Rigidbody targetRb;

	// Token: 0x04000029 RID: 41
	private Car targetCar;

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
	private float shakeThreshold = 16f;

	// Token: 0x04000036 RID: 54
	private bool readyToOffset = true;
}
