using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class MapManager : MonoBehaviour
{
    // Token: 0x060000CF RID: 207 RVA: 0x00005F5E File Offset: 0x0000415E
    private void Awake()
    {
        if (MapManager.Instance != null && MapManager.Instance != this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            return;
        }
        MapManager.Instance = this;
    }

    // Token: 0x060000D0 RID: 208 RVA: 0x00005F8C File Offset: 0x0000418C
    public int GetStars(int map, float time)
    {
        var result = 0;
        if (time <= maps[map].times[2])
        {
            result = 3;
        }
        else if (time <= maps[map].times[1])
        {
            result = 2;
        }
        else if (time <= maps[map].times[0])
        {
            result = 1;
        }
        if (time <= 0f)
        {
            result = 0;
        }
        return result;
    }

    // Token: 0x040000F3 RID: 243
    public MapManager.MapInformation[] maps;

    // Token: 0x040000F4 RID: 244
    public static MapManager Instance;

    // Token: 0x02000062 RID: 98
    [Serializable]
    public class MapInformation
    {
        // Token: 0x0400023F RID: 575
        public int index;

        // Token: 0x04000240 RID: 576
        public string name;

        // Token: 0x04000241 RID: 577
        public Sprite image;

        // Token: 0x04000242 RID: 578
        public Color themeColor;

        // Token: 0x04000243 RID: 579
        public float[] times;
    }
}
