using UnityEngine;

// Token: 0x02000035 RID: 53
public class RandomBGColor : MonoBehaviour
{
    // Token: 0x0600010F RID: 271 RVA: 0x00006A08 File Offset: 0x00004C08
    private void Awake() => camera = base.GetComponentInChildren<Camera>();

    // Token: 0x06000110 RID: 272 RVA: 0x00006A18 File Offset: 0x00004C18
    private void RandomColor()
    {
        var backgroundColor = colors[UnityEngine.Random.Range(0, colors.Length)];
        camera.backgroundColor = backgroundColor;
    }

    // Token: 0x06000111 RID: 273 RVA: 0x00006A4B File Offset: 0x00004C4B
    private void OnEnable() => RandomColor();

    // Token: 0x0400012C RID: 300
    private new Camera camera;

    // Token: 0x0400012D RID: 301
    private readonly Color[] colors = new Color[]
    {
        new Color(1f, 0.65f, 0.4f),
        new Color(1f, 0.4f, 0.41f),
        new Color(1f, 0.4f, 0.66f),
        new Color(0.95f, 0.48f, 1f),
        new Color(0.45f, 0.45f, 1f),
        new Color(0.316f, 0.7123f, 1f),
        new Color(0.35f, 1f, 0.48f)
    };
}
