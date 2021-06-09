using UnityEngine;

// Token: 0x02000021 RID: 33
public class LookAtTarget : MonoBehaviour
{
    // Token: 0x060000C0 RID: 192 RVA: 0x000059E2 File Offset: 0x00003BE2
    private void Start() => cam = base.GetComponent<Camera>();

    // Token: 0x060000C1 RID: 193 RVA: 0x000059F0 File Offset: 0x00003BF0
    private void Update()
    {
        base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.LookRotation(target.position - base.transform.position), Time.deltaTime * 10f);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * 5.5f);
    }

    // Token: 0x040000E0 RID: 224
    public Transform target;

    // Token: 0x040000E1 RID: 225
    private Camera cam;

    // Token: 0x040000E2 RID: 226
    private readonly float targetFov = 15f;
}
