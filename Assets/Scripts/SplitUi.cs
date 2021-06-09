using TMPro;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class SplitUi : MonoBehaviour
{
    // Token: 0x06000181 RID: 385 RVA: 0x000086B8 File Offset: 0x000068B8
    private void Awake()
    {
        text = base.GetComponentInChildren<TextMeshProUGUI>();
        base.Invoke("DestroySelf", 3f);
        base.Invoke("StartFade", 1.5f);
        desiredScale = Vector3.one * 1f;
        base.transform.localScale = Vector3.zero;
    }

    // Token: 0x06000182 RID: 386 RVA: 0x00008716 File Offset: 0x00006916
    private void StartFade() => text.CrossFadeAlpha(0f, 1.5f, true);

    // Token: 0x06000183 RID: 387 RVA: 0x00008730 File Offset: 0x00006930
    private void Update()
    {
        desiredScale = Vector3.Lerp(desiredScale, Vector3.zero, Time.deltaTime * speed * 0.1f);
        base.transform.localScale = Vector3.Lerp(base.transform.localScale, desiredScale, Time.deltaTime * speed * 7.5f);
        base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, Vector3.up, Time.deltaTime * speed);
    }

    // Token: 0x06000184 RID: 388 RVA: 0x000087C4 File Offset: 0x000069C4
    public void SetSplit(string t) => text.text = t;

    // Token: 0x06000185 RID: 389 RVA: 0x000087D2 File Offset: 0x000069D2
    private void DestroySelf() => UnityEngine.Object.Destroy(base.gameObject);

    // Token: 0x04000198 RID: 408
    private TextMeshProUGUI text;

    // Token: 0x04000199 RID: 409
    private readonly float speed = 1f;

    // Token: 0x0400019A RID: 410
    private Vector3 desiredScale;
}
