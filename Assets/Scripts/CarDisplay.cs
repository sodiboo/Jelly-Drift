using UnityEngine;

// Token: 0x0200000F RID: 15
public class CarDisplay : MonoBehaviour
{
    // Token: 0x17000008 RID: 8
    // (get) Token: 0x06000056 RID: 86 RVA: 0x0000407E File Offset: 0x0000227E
    // (set) Token: 0x06000057 RID: 87 RVA: 0x00004086 File Offset: 0x00002286
    public int nCars { get; set; }

    // Token: 0x06000058 RID: 88 RVA: 0x0000408F File Offset: 0x0000228F
    private void Awake()
    {
        if (!(CarDisplay.Instance != null) || !(CarDisplay.Instance != this))
        {
            CarDisplay.Instance = this;
        }
        nCars = PrefabManager.Instance.cars.Length;
        SpawnCar(0);
    }

    // Token: 0x0600005A RID: 90 RVA: 0x000040CA File Offset: 0x000022CA
    public void SetSkin(int n)
    {
        skin.SetSkin(n);
        MonoBehaviour.print("setting skin to: " + n);
    }

    // Token: 0x0600005B RID: 91 RVA: 0x000040ED File Offset: 0x000022ED
    public void Hide() => currentCar.gameObject.SetActive(false);

    // Token: 0x0600005C RID: 92 RVA: 0x00004100 File Offset: 0x00002300
    public void Show() => currentCar.gameObject.SetActive(true);

    // Token: 0x0600005D RID: 93 RVA: 0x00004114 File Offset: 0x00002314
    public void SpawnCar(int n)
    {
        UnityEngine.Object.Destroy(currentCar);
        currentCar = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.cars[n], base.transform.position, base.transform.rotation);
        currentCar.name = PrefabManager.Instance.cars[n].name;
        skin = currentCar.GetComponent<CarSkin>();
        UnityEngine.Object.Destroy(currentCar.GetComponent<CheckpointUser>());
        if (!SaveManager.Instance.state.carsUnlocked[n])
        {
            foreach (var renderer in currentCar.GetComponentsInChildren<Renderer>())
            {
                var array = new Material[renderer.materials.Length];
                for (var j = 0; j < array.Length; j++)
                {
                    array[j] = black;
                }
                renderer.materials = array;
            }
        }
    }

    // Token: 0x0400008A RID: 138
    public GameObject currentCar;

    // Token: 0x0400008B RID: 139
    private CarSkin skin;

    // Token: 0x0400008D RID: 141
    public static CarDisplay Instance;

    // Token: 0x0400008E RID: 142
    public Material black;
}
