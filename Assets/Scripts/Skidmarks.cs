using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000043 RID: 67
public class Skidmarks : MonoBehaviour
{
	// Token: 0x06000176 RID: 374 RVA: 0x00007DBA File Offset: 0x00005FBA
	protected void Awake()
	{
		Skidmarks.Instance = this;
		if (base.transform.position != Vector3.zero)
		{
			base.transform.position = Vector3.zero;
			base.transform.rotation = Quaternion.identity;
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00007DFC File Offset: 0x00005FFC
	protected void Start()
	{
		this.skidmarks = new Skidmarks.MarkSection[1024];
		for (int i = 0; i < 1024; i++)
		{
			this.skidmarks[i] = new Skidmarks.MarkSection();
		}
		this.mf = base.GetComponent<MeshFilter>();
		this.mr = base.GetComponent<MeshRenderer>();
		if (this.mr == null)
		{
			this.mr = base.gameObject.AddComponent<MeshRenderer>();
		}
		this.marksMesh = new Mesh();
		this.marksMesh.MarkDynamic();
		if (this.mf == null)
		{
			this.mf = base.gameObject.AddComponent<MeshFilter>();
		}
		this.mf.sharedMesh = this.marksMesh;
		this.vertices = new Vector3[4096];
		this.normals = new Vector3[4096];
		this.tangents = new Vector4[4096];
		this.colors = new Color32[4096];
		this.uvs = new Vector2[4096];
		this.triangles = new int[6144];
		this.mr.shadowCastingMode = ShadowCastingMode.Off;
		this.mr.receiveShadows = false;
		this.mr.material = this.skidmarksMaterial;
		this.mr.lightProbeUsage = LightProbeUsage.Off;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00007F48 File Offset: 0x00006148
	protected void LateUpdate()
	{
		if (!this.meshUpdated)
		{
			return;
		}
		this.meshUpdated = false;
		this.marksMesh.vertices = this.vertices;
		this.marksMesh.normals = this.normals;
		this.marksMesh.tangents = this.tangents;
		this.marksMesh.triangles = this.triangles;
		this.marksMesh.colors32 = this.colors;
		this.marksMesh.uv = this.uvs;
		if (!this.haveSetBounds)
		{
			this.marksMesh.bounds = new Bounds(new Vector3(0f, 0f, 0f), new Vector3(10000f, 10000f, 10000f));
			this.haveSetBounds = true;
		}
		this.mf.sharedMesh = this.marksMesh;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00008023 File Offset: 0x00006223
	public int AddSkidMark(Vector3 pos, Vector3 normal, float opacity, int lastIndex)
	{
		if (opacity > 1f)
		{
			opacity = 1f;
		}
		else if (opacity < 0f)
		{
			return -1;
		}
		this.black.a = (byte)(opacity * 255f);
		return this.AddSkidMark(pos, normal, this.black, lastIndex);
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00008064 File Offset: 0x00006264
	public int AddSkidMark(Vector3 pos, Vector3 normal, Color32 colour, int lastIndex)
	{
		if (colour.a == 0)
		{
			return -1;
		}
		Skidmarks.MarkSection markSection = null;
		Vector3 lhs = Vector3.zero;
		Vector3 vector = pos + normal * 0.02f;
		if (lastIndex != -1)
		{
			markSection = this.skidmarks[lastIndex];
			lhs = vector - markSection.Pos;
			if (lhs.sqrMagnitude < 0.0625f)
			{
				return lastIndex;
			}
			if (lhs.sqrMagnitude > 0.625f)
			{
				lastIndex = -1;
				markSection = null;
			}
		}
		colour.a = (byte)((float)colour.a * 1f);
		Skidmarks.MarkSection markSection2 = this.skidmarks[this.markIndex];
		markSection2.Pos = vector;
		markSection2.Normal = normal;
		markSection2.Colour = colour;
		markSection2.LastIndex = lastIndex;
		if (markSection != null)
		{
			Vector3 normalized = Vector3.Cross(lhs, normal).normalized;
			markSection2.Posl = markSection2.Pos + normalized * 0.25f * 0.5f;
			markSection2.Posr = markSection2.Pos - normalized * 0.25f * 0.5f;
			markSection2.Tangent = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
			if (markSection.LastIndex == -1)
			{
				markSection.Tangent = markSection2.Tangent;
				markSection.Posl = markSection2.Pos + normalized * 0.25f * 0.5f;
				markSection.Posr = markSection2.Pos - normalized * 0.25f * 0.5f;
			}
		}
		this.UpdateSkidmarksMesh();
		int result = this.markIndex;
		int num = this.markIndex + 1;
		this.markIndex = num;
		this.markIndex = num % 1024;
		return result;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00008230 File Offset: 0x00006430
	private void UpdateSkidmarksMesh()
	{
		Skidmarks.MarkSection markSection = this.skidmarks[this.markIndex];
		if (markSection.LastIndex == -1)
		{
			return;
		}
		Skidmarks.MarkSection markSection2 = this.skidmarks[markSection.LastIndex];
		this.vertices[this.markIndex * 4] = markSection2.Posl;
		this.vertices[this.markIndex * 4 + 1] = markSection2.Posr;
		this.vertices[this.markIndex * 4 + 2] = markSection.Posl;
		this.vertices[this.markIndex * 4 + 3] = markSection.Posr;
		this.normals[this.markIndex * 4] = markSection2.Normal;
		this.normals[this.markIndex * 4 + 1] = markSection2.Normal;
		this.normals[this.markIndex * 4 + 2] = markSection.Normal;
		this.normals[this.markIndex * 4 + 3] = markSection.Normal;
		this.tangents[this.markIndex * 4] = markSection2.Tangent;
		this.tangents[this.markIndex * 4 + 1] = markSection2.Tangent;
		this.tangents[this.markIndex * 4 + 2] = markSection.Tangent;
		this.tangents[this.markIndex * 4 + 3] = markSection.Tangent;
		this.colors[this.markIndex * 4] = markSection2.Colour;
		this.colors[this.markIndex * 4 + 1] = markSection2.Colour;
		this.colors[this.markIndex * 4 + 2] = markSection.Colour;
		this.colors[this.markIndex * 4 + 3] = markSection.Colour;
		this.uvs[this.markIndex * 4] = new Vector2(0f, 0f);
		this.uvs[this.markIndex * 4 + 1] = new Vector2(1f, 0f);
		this.uvs[this.markIndex * 4 + 2] = new Vector2(0f, 1f);
		this.uvs[this.markIndex * 4 + 3] = new Vector2(1f, 1f);
		this.triangles[this.markIndex * 6] = this.markIndex * 4;
		this.triangles[this.markIndex * 6 + 2] = this.markIndex * 4 + 1;
		this.triangles[this.markIndex * 6 + 1] = this.markIndex * 4 + 2;
		this.triangles[this.markIndex * 6 + 3] = this.markIndex * 4 + 2;
		this.triangles[this.markIndex * 6 + 5] = this.markIndex * 4 + 1;
		this.triangles[this.markIndex * 6 + 4] = this.markIndex * 4 + 3;
		this.meshUpdated = true;
	}

	// Token: 0x04000179 RID: 377
	[SerializeField]
	private Material skidmarksMaterial;

	// Token: 0x0400017A RID: 378
	private const int MAX_MARKS = 1024;

	// Token: 0x0400017B RID: 379
	private const float MARK_WIDTH = 0.25f;

	// Token: 0x0400017C RID: 380
	private const float GROUND_OFFSET = 0.02f;

	// Token: 0x0400017D RID: 381
	private const float MIN_DISTANCE = 0.25f;

	// Token: 0x0400017E RID: 382
	private const float MIN_SQR_DISTANCE = 0.0625f;

	// Token: 0x0400017F RID: 383
	private const float MAX_OPACITY = 1f;

	// Token: 0x04000180 RID: 384
	private int markIndex;

	// Token: 0x04000181 RID: 385
	private Skidmarks.MarkSection[] skidmarks;

	// Token: 0x04000182 RID: 386
	private Mesh marksMesh;

	// Token: 0x04000183 RID: 387
	private MeshRenderer mr;

	// Token: 0x04000184 RID: 388
	private MeshFilter mf;

	// Token: 0x04000185 RID: 389
	private Vector3[] vertices;

	// Token: 0x04000186 RID: 390
	private Vector3[] normals;

	// Token: 0x04000187 RID: 391
	private Vector4[] tangents;

	// Token: 0x04000188 RID: 392
	private Color32[] colors;

	// Token: 0x04000189 RID: 393
	private Vector2[] uvs;

	// Token: 0x0400018A RID: 394
	private int[] triangles;

	// Token: 0x0400018B RID: 395
	private bool meshUpdated;

	// Token: 0x0400018C RID: 396
	private bool haveSetBounds;

	// Token: 0x0400018D RID: 397
	private Color32 black = Color.black;

	// Token: 0x0400018E RID: 398
	public static Skidmarks Instance;

	// Token: 0x02000065 RID: 101
	private class MarkSection
	{
		// Token: 0x0400024A RID: 586
		public Vector3 Pos = Vector3.zero;

		// Token: 0x0400024B RID: 587
		public Vector3 Normal = Vector3.zero;

		// Token: 0x0400024C RID: 588
		public Vector4 Tangent = Vector4.zero;

		// Token: 0x0400024D RID: 589
		public Vector3 Posl = Vector3.zero;

		// Token: 0x0400024E RID: 590
		public Vector3 Posr = Vector3.zero;

		// Token: 0x0400024F RID: 591
		public Color32 Colour;

		// Token: 0x04000250 RID: 592
		public int LastIndex;
	}
}
