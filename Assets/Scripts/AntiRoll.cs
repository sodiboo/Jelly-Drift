using UnityEngine;

// Token: 0x02000006 RID: 6
public class AntiRoll : MonoBehaviour
{
    // Token: 0x06000012 RID: 18 RVA: 0x000022F2 File Offset: 0x000004F2
    private void Awake() => bodyRb = base.GetComponent<Rigidbody>();

    // Token: 0x06000013 RID: 19 RVA: 0x00002300 File Offset: 0x00000500
    private void FixedUpdate() => StabilizerBars();

    // Token: 0x06000014 RID: 20 RVA: 0x00002308 File Offset: 0x00000508
    private void StabilizerBars()
    {
        float num;
        if (right.grounded)
        {
            num = right.lastCompression;
        }
        else
        {
            num = 1f;
        }
        float num2;
        if (left.grounded)
        {
            num2 = left.lastCompression;
        }
        else
        {
            num2 = 1f;
        }
        var num3 = (num2 - num) * antiRoll;
        if (right.grounded)
        {
            bodyRb.AddForceAtPosition(right.transform.up * -num3, right.transform.position);
        }
        if (left.grounded)
        {
            bodyRb.AddForceAtPosition(left.transform.up * num3, left.transform.position);
        }
    }

    // Token: 0x04000016 RID: 22
    public Suspension right;

    // Token: 0x04000017 RID: 23
    public Suspension left;

    // Token: 0x04000018 RID: 24
    public float antiRoll = 5000f;

    // Token: 0x04000019 RID: 25
    private Rigidbody bodyRb;
}
