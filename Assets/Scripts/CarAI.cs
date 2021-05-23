using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class CarAI : MonoBehaviour
{
	// Token: 0x06000041 RID: 65 RVA: 0x00003654 File Offset: 0x00001854
	private void Start()
	{
		this.difficulty = (int)GameState.Instance.difficulty;
		MonoBehaviour.print(string.Concat(new object[]
		{
			"d: ",
			GameState.Instance.difficulty,
			", a: ",
			this.difficulty
		}));
		this.car.engineForce = (float)this.difficultyConfig[this.difficulty];
		base.InvokeRepeating("AdjustSpeed", 0.5f, 0.5f);
		if (GameController.Instance.finalCheckpoint != 0)
		{
			base.GetComponent<CheckpointUser>().ForceCheckpoint(0);
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000036F8 File Offset: 0x000018F8
	public void Recover()
	{
		this.car.rb.velocity = Vector3.zero;
		base.transform.position = this.nodes[this.FindClosestNode(3, base.transform)].position;
		int num = this.currentNode % this.nodes.Length;
		int num2 = (num + 1) % this.nodes.Length;
		Vector3 normalized = (this.nodes[num2].position - this.nodes[num].position).normalized;
		base.transform.rotation = Quaternion.LookRotation(normalized);
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003794 File Offset: 0x00001994
	private void CheckRecover()
	{
		if (!GameController.Instance.playing)
		{
			return;
		}
		if (base.transform.position.y < (float)this.respawnHeight)
		{
			this.Recover();
		}
		if (base.IsInvoking("Recover"))
		{
			if (this.car.speed > 3f)
			{
				base.CancelInvoke("Recover");
			}
			return;
		}
		if (this.car.speed < 3f)
		{
			base.Invoke("Recover", this.recoverTime);
			return;
		}
		base.CancelInvoke("Recover");
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003827 File Offset: 0x00001A27
	private void Update()
	{
		if (!GameController.Instance.playing || !this.path)
		{
			return;
		}
		this.NewAI();
		this.CheckRecover();
	}

	// Token: 0x06000045 RID: 69 RVA: 0x0000384F File Offset: 0x00001A4F
	public void SetPath(Transform p)
	{
		this.path = p;
		this.nodes = this.path.GetComponentsInChildren<Transform>();
		this.car = base.GetComponent<Car>();
		this.currentNode = this.FindClosestNode(this.nodes.Length, base.transform);
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003890 File Offset: 0x00001A90
	private int FindNextTurn()
	{
		for (int i = this.currentNode; i < this.currentNode + this.turnLookAhead; i++)
		{
			int num = i % this.nodes.Length;
			int num2 = (num + 1) % this.nodes.Length;
			int num3 = (num2 + 1) % this.nodes.Length;
			Vector3 vector = this.nodes[num2].position - this.nodes[num].position;
			Vector3 vector2 = this.nodes[num3].position - this.nodes[num2].position;
			float f = Vector3.SignedAngle(vector.normalized, vector2.normalized, Vector3.up);
			if (Mathf.Abs(f) > 20f)
			{
				this.turnDir = (int)Mathf.Sign(f);
				this.nextTurnLength = this.FindNextStraight(num2);
				return num2;
			}
		}
		return -1;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003970 File Offset: 0x00001B70
	private int FindNextStraight(int startNode)
	{
		for (int i = startNode; i < startNode + this.turnLookAhead; i++)
		{
			int num = i % this.nodes.Length;
			int num2 = (num + 1) % this.nodes.Length;
			int num3 = (num2 + 1) % this.nodes.Length;
			Vector3 from = this.nodes[num2].position - this.nodes[num].position;
			Vector3 to = this.nodes[num3].position - this.nodes[num2].position;
			if (Mathf.Abs(Vector3.SignedAngle(from, to, Vector3.up)) < 15f)
			{
				return num2 - startNode;
			}
		}
		return 3;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003A18 File Offset: 0x00001C18
	private void NewAI()
	{
		int num = this.FindClosestNode(this.maxLookAhead, base.transform);
		this.currentNode = num;
		int num2 = (num + 1) % this.nodes.Length;
		if (this.currentNode > this.nextTurnStart + this.nextTurnLength)
		{
			this.nextTurnStart = this.FindNextTurn();
		}
		if (num2 < this.nextTurnStart)
		{
			this.xOffset = 0.13f * (float)this.turnDir;
		}
		else if (num2 >= this.nextTurnStart && num2 < this.nextTurnStart + this.nextTurnLength)
		{
			this.xOffset = -0.13f * (float)this.turnDir;
		}
		else
		{
			this.xOffset = 0f;
		}
		Vector3 b = Vector3.Cross(this.nodes[num2].position - this.nodes[num].position, Vector3.up) * this.xOffset;
		Vector3 vector = this.nodes[num2].position + b - base.transform.position;
		vector = base.transform.InverseTransformDirection(vector);
		float num3 = 1f + Mathf.Clamp(this.car.speed * 0.01f * this.speedSteerMultiplier, 0f, 1f);
		this.car.steering = Mathf.Clamp(vector.x * 0.05f * num3, -1f, 1f) * num3;
		this.car.throttle = 1f;
		this.car.throttle = 1f - Mathf.Abs(this.car.steering * Mathf.Clamp(this.car.speed - (float)this.maxTurnSpeed, 0f, 100f) * 0.06f);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00003BE0 File Offset: 0x00001DE0
	private void AdjustSpeed()
	{
		float num = (float)this.FindClosestNode(this.nodes.Length, base.transform) / (float)this.nodes.Length;
		float num2 = (float)this.FindClosestNode(this.nodes.Length, GameController.Instance.currentCar.transform) / (float)this.nodes.Length;
		float num3 = num - num2;
		if (num3 < 0f)
		{
			num3 *= this.speedupM;
		}
		if (num3 > 0f)
		{
			num3 *= this.slowdownM;
		}
		float num4 = (float)this.difficultyConfig[this.difficulty] - Mathf.Clamp(num3 * 1000f * this.speedAdjustMultiplier, -8000f, 4000f);
		num4 = Mathf.Clamp(num4, 1000f, 8000f);
		this.car.engineForce = num4;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00003CA4 File Offset: 0x00001EA4
	public int FindClosestNode(int maxLook, Transform target)
	{
		float num = float.PositiveInfinity;
		int result = 0;
		for (int i = 0; i < maxLook; i++)
		{
			int num2 = (this.currentNode + i) % this.nodes.Length;
			float num3 = Vector3.Distance(target.position, this.nodes[num2].position);
			if (num3 < num)
			{
				num = num3;
				result = num2;
			}
		}
		return result;
	}

	// Token: 0x04000069 RID: 105
	[ExecuteInEditMode]
	public Transform path;

	// Token: 0x0400006A RID: 106
	public Transform[] nodes;

	// Token: 0x0400006B RID: 107
	private Car car;

	// Token: 0x0400006C RID: 108
	private LineRenderer line;

	// Token: 0x0400006D RID: 109
	private float roadWidth = 0.4f;

	// Token: 0x0400006E RID: 110
	private float maxOffset = 0.36f;

	// Token: 0x0400006F RID: 111
	private int lookAhead = 4;

	// Token: 0x04000070 RID: 112
	private int maxLookAhead = 6;

	// Token: 0x04000071 RID: 113
	private int currentDriftNode;

	// Token: 0x04000072 RID: 114
	public int respawnHeight;

	// Token: 0x04000073 RID: 115
	private int difficulty;

	// Token: 0x04000074 RID: 116
	public int[] difficultyConfig;

	// Token: 0x04000075 RID: 117
	private float recoverTime = 1.5f;

	// Token: 0x04000076 RID: 118
	private int turnLookAhead = 6;

	// Token: 0x04000077 RID: 119
	private int turnDir;

	// Token: 0x04000078 RID: 120
	private int nextTurnStart;

	// Token: 0x04000079 RID: 121
	private int nextTurnLength;

	// Token: 0x0400007A RID: 122
	public float xOffset;

	// Token: 0x0400007B RID: 123
	public float speedSteerMultiplier = 1f;

	// Token: 0x0400007C RID: 124
	public float steerMultiplier = 1f;

	// Token: 0x0400007D RID: 125
	public int maxTurnSpeed = 50;

	// Token: 0x0400007E RID: 126
	private float speedAdjustMultiplier = 5f;

	// Token: 0x0400007F RID: 127
	private float speedupM = 15f;

	// Token: 0x04000080 RID: 128
	private float slowdownM = 5f;

	// Token: 0x04000081 RID: 129
	public int currentNode;
}
