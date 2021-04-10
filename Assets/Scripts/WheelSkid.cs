using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
[RequireComponent(typeof(WheelCollider))]
public class WheelSkid : MonoBehaviour
{
	// Token: 0x0600017D RID: 381 RVA: 0x00008556 File Offset: 0x00006756
	protected void Awake()
	{
		this.wheelCollider = base.GetComponent<WheelCollider>();
		this.lastFixedUpdateTime = Time.time;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000856F File Offset: 0x0000676F
	protected void FixedUpdate()
	{
		this.lastFixedUpdateTime = Time.time;
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000857C File Offset: 0x0000677C
	protected void LateUpdate()
	{
		if (!this.wheelCollider.GetGroundHit(out this.wheelHitInfo))
		{
			this.lastSkid = -1;
			return;
		}
		float num = Mathf.Abs(base.transform.InverseTransformDirection(this.rb.velocity).x);
		float num2 = this.wheelCollider.radius * (6.2831855f * this.wheelCollider.rpm / 60f);
		float num3 = Vector3.Dot(this.rb.velocity, base.transform.forward);
		float num4 = Mathf.Abs(num3 - num2) * 10f;
		num4 = Mathf.Max(0f, num4 * (10f - Mathf.Abs(num3)));
		num += num4;
		if (num >= 0.5f)
		{
			float opacity = Mathf.Clamp01(num / 20f);
			Vector3 pos = this.wheelHitInfo.point + this.rb.velocity * (Time.time - this.lastFixedUpdateTime);
			this.lastSkid = this.skidmarksController.AddSkidMark(pos, this.wheelHitInfo.normal, opacity, this.lastSkid);
			return;
		}
		this.lastSkid = -1;
	}

	// Token: 0x0400018F RID: 399
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000190 RID: 400
	[SerializeField]
	private Skidmarks skidmarksController;

	// Token: 0x04000191 RID: 401
	private WheelCollider wheelCollider;

	// Token: 0x04000192 RID: 402
	private WheelHit wheelHitInfo;

	// Token: 0x04000193 RID: 403
	private const float SKID_FX_SPEED = 0.5f;

	// Token: 0x04000194 RID: 404
	private const float MAX_SKID_INTENSITY = 20f;

	// Token: 0x04000195 RID: 405
	private const float WHEEL_SLIP_MULTIPLIER = 10f;

	// Token: 0x04000196 RID: 406
	private int lastSkid = -1;

	// Token: 0x04000197 RID: 407
	private float lastFixedUpdateTime;
}
