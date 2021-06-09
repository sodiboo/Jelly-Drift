using UnityEngine;

// Token: 0x02000005 RID: 5
public class AnimateUI : MonoBehaviour
{
    // Token: 0x0600000F RID: 15 RVA: 0x000021EE File Offset: 0x000003EE
    private void Awake()
    {
        defaultScale = base.transform.localScale;
        defaultRot = 0f;
        desiredScale = defaultScale;
    }

    // Token: 0x06000010 RID: 16 RVA: 0x00002218 File Offset: 0x00000418
    private void Update()
    {
        var d = 1f + (Mathf.PingPong(Time.time * scaleSpeed, scaleStrength) - scaleStrength / 2f);
        var target = Mathf.PingPong(Time.time * rotSpeed, rotStrength) - rotStrength / 2f;
        desiredScale = defaultScale * d;
        base.transform.localScale = Vector3.SmoothDamp(base.transform.localScale, desiredScale, ref scaleVel, scaleSmooth);
        rot = Mathf.SmoothDamp(rot, target, ref rotVel, rotSmooth);
        base.transform.localRotation = Quaternion.Euler(0f, 0f, rot);
    }

    // Token: 0x0400000A RID: 10
    private Vector3 defaultScale;

    // Token: 0x0400000B RID: 11
    private float defaultRot;

    // Token: 0x0400000C RID: 12
    private float rotVel;

    // Token: 0x0400000D RID: 13
    public float rotSpeed;

    // Token: 0x0400000E RID: 14
    public float rotStrength;

    // Token: 0x0400000F RID: 15
    public float rotSmooth;

    // Token: 0x04000010 RID: 16
    private Vector3 desiredScale;

    // Token: 0x04000011 RID: 17
    private Vector3 scaleVel;

    // Token: 0x04000012 RID: 18
    public float scaleSpeed;

    // Token: 0x04000013 RID: 19
    public float scaleStrength;

    // Token: 0x04000014 RID: 20
    public float scaleSmooth;

    // Token: 0x04000015 RID: 21
    private float rot;
}
