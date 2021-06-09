using UnityEngine;

// Token: 0x02000013 RID: 19
public class CinematicCamera : MonoBehaviour
{
    // Token: 0x0600006B RID: 107 RVA: 0x00004494 File Offset: 0x00002694
    private void Update()
    {
        base.transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        base.transform.LookAt(target.position + offset);
    }

    // Token: 0x040000A2 RID: 162
    public float rotationSpeed;

    // Token: 0x040000A3 RID: 163
    public Transform target;

    // Token: 0x040000A4 RID: 164
    public Vector3 offset;
}
