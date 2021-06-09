using UnityEngine;

// Token: 0x02000039 RID: 57
public class RoadObstacle : MonoBehaviour
{
    // Token: 0x06000124 RID: 292 RVA: 0x00007324 File Offset: 0x00005524
    private void OnTriggerEnter(Collider other)
    {
        if (!ready)
        {
            return;
        }
        UnityEngine.Object.Instantiate<GameObject>(particles, base.transform.position, particles.transform.rotation);
        UnityEngine.Object.Destroy(base.gameObject);
        ready = false;
    }

    // Token: 0x0400013C RID: 316
    public GameObject particles;

    // Token: 0x0400013D RID: 317
    private bool ready = true;
}
