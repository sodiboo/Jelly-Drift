using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class CarSkin : MonoBehaviour
{
    // Token: 0x0600005F RID: 95 RVA: 0x000020AB File Offset: 0x000002AB
    private void Start()
    {
    }

    // Token: 0x06000060 RID: 96 RVA: 0x00004208 File Offset: 0x00002408
    public void SetSkin(int n)
    {
        if (skinsToChange.Length == 0)
        {
            return;
        }
        MonoBehaviour.print("n: " + n);
        var i = 0;
        while (i < skinsToChange[n].myArray.Length)
        {
            var num = skinsToChange[n].myArray[i++];
            var num2 = skinsToChange[n].myArray[i++];
            var num3 = skinsToChange[n].myArray[i++];
            var array = renderers[num].materials;
            array[num2] = materials[num3];
            renderers[num].materials = array;
        }
    }

    // Token: 0x06000061 RID: 97 RVA: 0x000042B0 File Offset: 0x000024B0
    public string GetSkinName(int n) => materials[n].name;

    // Token: 0x0400008F RID: 143
    private readonly int currentSkin;

    // Token: 0x04000090 RID: 144
    public Renderer[] renderers;

    // Token: 0x04000091 RID: 145
    public Material[] materials;

    // Token: 0x04000092 RID: 146
    public CarSkin.SkinArray[] skinsToChange;

    // Token: 0x0200005F RID: 95
    [Serializable]
    public class SkinArray
    {
        // Token: 0x04000236 RID: 566
        public int[] myArray;
    }
}
