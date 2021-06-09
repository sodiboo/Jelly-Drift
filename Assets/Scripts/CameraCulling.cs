using UnityEngine;

// Token: 0x0200000A RID: 10
public class CameraCulling : MonoBehaviour
{
    // Token: 0x06000024 RID: 36 RVA: 0x000029EE File Offset: 0x00000BEE
    private void Awake()
    {
        CameraCulling.Instance = this;
        cam = base.GetComponent<Camera>();
        UpdateCulling();
    }

    // Token: 0x06000025 RID: 37 RVA: 0x00002A08 File Offset: 0x00000C08
    public void UpdateCulling()
    {
        var array = new float[32];
        var quality = SaveState.Instance.quality;
        if (quality == 0)
        {
            array[12] = 120f;
        }
        else if (quality == 1)
        {
            array[12] = 300f;
        }
        else
        {
            array[12] = 1000f;
        }
        cam.layerCullDistances = array;
        cam.layerCullSpherical = true;
    }

    // Token: 0x04000037 RID: 55
    public static CameraCulling Instance;

    // Token: 0x04000038 RID: 56
    private Camera cam;
}
