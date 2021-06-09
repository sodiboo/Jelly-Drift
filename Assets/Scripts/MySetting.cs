using System.Linq;
using TMPro;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class MySetting : MonoBehaviour
{
    // Token: 0x17000015 RID: 21
    // (get) Token: 0x060000EF RID: 239 RVA: 0x000063A7 File Offset: 0x000045A7
    // (set) Token: 0x060000F0 RID: 240 RVA: 0x000063AF File Offset: 0x000045AF
    public TextMeshProUGUI[] options { get; set; }

    // Token: 0x060000F1 RID: 241 RVA: 0x000063B8 File Offset: 0x000045B8
    private void Awake() => options = (from r in base.GetComponentsInChildren<TextMeshProUGUI>()
                                       where !r.CompareTag("Ignore")
                                       select r).ToArray<TextMeshProUGUI>();
}
