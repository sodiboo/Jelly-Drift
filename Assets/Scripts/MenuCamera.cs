using UnityEngine;

// Token: 0x02000025 RID: 37
public class MenuCamera : MonoBehaviour
{
    // Token: 0x060000D2 RID: 210 RVA: 0x00005FE8 File Offset: 0x000041E8
    private void Update()
    {
        if (!CarDisplay.Instance || !CarDisplay.Instance.currentCar)
        {
            return;
        }
        base.transform.RotateAround(carDisplay.currentCar.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        base.transform.LookAt(carDisplay.currentCar.transform.position + offset);
    }

    // Token: 0x040000F5 RID: 245
    public float rotationSpeed;

    // Token: 0x040000F6 RID: 246
    public Transform target;

    // Token: 0x040000F7 RID: 247
    public Vector3 offset;

    // Token: 0x040000F8 RID: 248
    public CarDisplay carDisplay;
}
