using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000011 RID: 17
public class CarStats : MonoBehaviour
{
    // Token: 0x06000063 RID: 99 RVA: 0x000042C0 File Offset: 0x000024C0
    public void SetStats(int car)
    {
        var component = PrefabManager.Instance.cars[car].GetComponent<Car>();
        var engineForce = component.engineForce;
        var driftMultiplier = component.driftMultiplier;
        var num = component.stability;
        dSpeed = (engineForce - minSpeed) / (maxSpeed - minSpeed);
        dDrift = (driftMultiplier - minDrift) / (maxDrift - minDrift);
        dStability = (num - minStab) / (maxStab - minStab);
    }

    // Token: 0x06000064 RID: 100 RVA: 0x00004348 File Offset: 0x00002548
    private void Update()
    {
        speed.transform.localScale = Vector3.Lerp(speed.transform.localScale, new Vector3(dSpeed, 1f, 1f), Time.deltaTime * 4f);
        drift.transform.localScale = Vector3.Lerp(drift.transform.localScale, new Vector3(dDrift, 1f, 1f), Time.deltaTime * 4f);
        stability.transform.localScale = Vector3.Lerp(stability.transform.localScale, new Vector3(dStability, 1f, 1f), Time.deltaTime * 4f);
    }

    // Token: 0x04000093 RID: 147
    public Image speed;

    // Token: 0x04000094 RID: 148
    public Image drift;

    // Token: 0x04000095 RID: 149
    public Image stability;

    // Token: 0x04000096 RID: 150
    public float minSpeed;

    // Token: 0x04000097 RID: 151
    public float maxSpeed;

    // Token: 0x04000098 RID: 152
    public float minDrift;

    // Token: 0x04000099 RID: 153
    public float maxDrift;

    // Token: 0x0400009A RID: 154
    public float minStab;

    // Token: 0x0400009B RID: 155
    public float maxStab;

    // Token: 0x0400009C RID: 156
    private float dSpeed;

    // Token: 0x0400009D RID: 157
    private float dDrift;

    // Token: 0x0400009E RID: 158
    private float dStability;
}
