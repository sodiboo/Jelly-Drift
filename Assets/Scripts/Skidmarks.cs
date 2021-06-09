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
        skidmarks = new Skidmarks.MarkSection[1024];
        for (var i = 0; i < 1024; i++)
        {
            skidmarks[i] = new Skidmarks.MarkSection();
        }
        mf = base.GetComponent<MeshFilter>();
        mr = base.GetComponent<MeshRenderer>();
        if (mr == null)
        {
            mr = base.gameObject.AddComponent<MeshRenderer>();
        }
        marksMesh = new Mesh();
        marksMesh.MarkDynamic();
        if (mf == null)
        {
            mf = base.gameObject.AddComponent<MeshFilter>();
        }
        mf.sharedMesh = marksMesh;
        vertices = new Vector3[4096];
        normals = new Vector3[4096];
        tangents = new Vector4[4096];
        colors = new Color32[4096];
        uvs = new Vector2[4096];
        triangles = new int[6144];
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;
        mr.material = skidmarksMaterial;
        mr.lightProbeUsage = LightProbeUsage.Off;
    }

    // Token: 0x06000178 RID: 376 RVA: 0x00007F48 File Offset: 0x00006148
    protected void LateUpdate()
    {
        if (!meshUpdated)
        {
            return;
        }
        meshUpdated = false;
        marksMesh.vertices = vertices;
        marksMesh.normals = normals;
        marksMesh.tangents = tangents;
        marksMesh.triangles = triangles;
        marksMesh.colors32 = colors;
        marksMesh.uv = uvs;
        if (!haveSetBounds)
        {
            marksMesh.bounds = new Bounds(new Vector3(0f, 0f, 0f), new Vector3(10000f, 10000f, 10000f));
            haveSetBounds = true;
        }
        mf.sharedMesh = marksMesh;
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
        black.a = (byte)(opacity * 255f);
        return AddSkidMark(pos, normal, black, lastIndex);
    }

    // Token: 0x0600017A RID: 378 RVA: 0x00008064 File Offset: 0x00006264
    public int AddSkidMark(Vector3 pos, Vector3 normal, Color32 colour, int lastIndex)
    {
        if (colour.a == 0)
        {
            return -1;
        }
        Skidmarks.MarkSection markSection = null;
        var lhs = Vector3.zero;
        var vector = pos + normal * 0.02f;
        if (lastIndex != -1)
        {
            markSection = skidmarks[lastIndex];
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
        colour.a = (byte)(colour.a * 1f);
        var markSection2 = skidmarks[markIndex];
        markSection2.Pos = vector;
        markSection2.Normal = normal;
        markSection2.Colour = colour;
        markSection2.LastIndex = lastIndex;
        if (markSection != null)
        {
            var normalized = Vector3.Cross(lhs, normal).normalized;
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
        UpdateSkidmarksMesh();
        var result = markIndex;
        var num = markIndex + 1;
        markIndex = num;
        markIndex = num % 1024;
        return result;
    }

    // Token: 0x0600017B RID: 379 RVA: 0x00008230 File Offset: 0x00006430
    private void UpdateSkidmarksMesh()
    {
        var markSection = skidmarks[markIndex];
        if (markSection.LastIndex == -1)
        {
            return;
        }
        var markSection2 = skidmarks[markSection.LastIndex];
        vertices[markIndex * 4] = markSection2.Posl;
        vertices[markIndex * 4 + 1] = markSection2.Posr;
        vertices[markIndex * 4 + 2] = markSection.Posl;
        vertices[markIndex * 4 + 3] = markSection.Posr;
        normals[markIndex * 4] = markSection2.Normal;
        normals[markIndex * 4 + 1] = markSection2.Normal;
        normals[markIndex * 4 + 2] = markSection.Normal;
        normals[markIndex * 4 + 3] = markSection.Normal;
        tangents[markIndex * 4] = markSection2.Tangent;
        tangents[markIndex * 4 + 1] = markSection2.Tangent;
        tangents[markIndex * 4 + 2] = markSection.Tangent;
        tangents[markIndex * 4 + 3] = markSection.Tangent;
        colors[markIndex * 4] = markSection2.Colour;
        colors[markIndex * 4 + 1] = markSection2.Colour;
        colors[markIndex * 4 + 2] = markSection.Colour;
        colors[markIndex * 4 + 3] = markSection.Colour;
        uvs[markIndex * 4] = new Vector2(0f, 0f);
        uvs[markIndex * 4 + 1] = new Vector2(1f, 0f);
        uvs[markIndex * 4 + 2] = new Vector2(0f, 1f);
        uvs[markIndex * 4 + 3] = new Vector2(1f, 1f);
        triangles[markIndex * 6] = markIndex * 4;
        triangles[markIndex * 6 + 2] = markIndex * 4 + 1;
        triangles[markIndex * 6 + 1] = markIndex * 4 + 2;
        triangles[markIndex * 6 + 3] = markIndex * 4 + 2;
        triangles[markIndex * 6 + 5] = markIndex * 4 + 1;
        triangles[markIndex * 6 + 4] = markIndex * 4 + 3;
        meshUpdated = true;
    }

    // Token: 0x04000179 RID: 377
    [SerializeField]
    private Material skidmarksMaterial;

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
