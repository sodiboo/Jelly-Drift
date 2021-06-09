using UnityEngine;

// Token: 0x02000020 RID: 32
public class KeepRotation : MonoBehaviour
{
    // Token: 0x060000BE RID: 190 RVA: 0x000059A4 File Offset: 0x00003BA4
    private void Update()
    {
        var eulerAngles = base.transform.rotation.eulerAngles;
        eulerAngles.x = 0f;
        base.transform.rotation = Quaternion.Euler(eulerAngles);
    }
}
