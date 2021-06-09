using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000032 RID: 50
public class PrefabManager : MonoBehaviour
{
    // Token: 0x06000105 RID: 261 RVA: 0x0000663D File Offset: 0x0000483D
    private void Awake()
    {
        if (PrefabManager.Instance != null && PrefabManager.Instance != this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            return;
        }
        PrefabManager.Instance = this;
    }

    // Token: 0x04000119 RID: 281
    public static PrefabManager Instance;

    // Token: 0x0400011A RID: 282
    public GameObject[] cars;

    // Token: 0x0400011B RID: 283
    public GameObject splitUi;

    // Token: 0x0400011C RID: 284
    public GameObject crashParticles;

    // Token: 0x0400011D RID: 285
    public Material ghostMat;

    public InputActionAsset inputs;

    public GameObject[] Ais;
    public Material sunMat;
    public GameObject speedometer;
}
