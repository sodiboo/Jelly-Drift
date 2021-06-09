using UnityEngine;

// Token: 0x02000041 RID: 65
public class PlayerSave
{
    // Token: 0x17000023 RID: 35
    // (get) Token: 0x06000154 RID: 340 RVA: 0x000079BB File Offset: 0x00005BBB
    // (set) Token: 0x06000155 RID: 341 RVA: 0x000079C3 File Offset: 0x00005BC3
    public int graphics { get; set; } = 1;

    // Token: 0x17000024 RID: 36
    // (get) Token: 0x06000156 RID: 342 RVA: 0x000079CC File Offset: 0x00005BCC
    // (set) Token: 0x06000157 RID: 343 RVA: 0x000079D4 File Offset: 0x00005BD4
    public int quality { get; set; } = 2;

    // Token: 0x17000025 RID: 37
    // (get) Token: 0x06000158 RID: 344 RVA: 0x000079DD File Offset: 0x00005BDD
    // (set) Token: 0x06000159 RID: 345 RVA: 0x000079E5 File Offset: 0x00005BE5
    public int motionBlur { get; set; } = 1;

    // Token: 0x17000026 RID: 38
    // (get) Token: 0x0600015A RID: 346 RVA: 0x000079EE File Offset: 0x00005BEE
    // (set) Token: 0x0600015B RID: 347 RVA: 0x000079F6 File Offset: 0x00005BF6
    public int dof { get; set; } = 1;

    // Token: 0x17000027 RID: 39
    // (get) Token: 0x0600015C RID: 348 RVA: 0x000079FF File Offset: 0x00005BFF
    // (set) Token: 0x0600015D RID: 349 RVA: 0x00007A07 File Offset: 0x00005C07
    public int cameraMode { get; set; }

    // Token: 0x17000028 RID: 40
    // (get) Token: 0x0600015E RID: 350 RVA: 0x00007A10 File Offset: 0x00005C10
    // (set) Token: 0x0600015F RID: 351 RVA: 0x00007A18 File Offset: 0x00005C18
    public int cameraShake { get; set; } = 1;

    // Token: 0x17000029 RID: 41
    // (get) Token: 0x06000160 RID: 352 RVA: 0x00007A21 File Offset: 0x00005C21
    // (set) Token: 0x06000161 RID: 353 RVA: 0x00007A29 File Offset: 0x00005C29
    public int muted { get; set; }

    // Token: 0x1700002A RID: 42
    // (get) Token: 0x06000162 RID: 354 RVA: 0x00007A32 File Offset: 0x00005C32
    // (set) Token: 0x06000163 RID: 355 RVA: 0x00007A3A File Offset: 0x00005C3A
    public int volume { get; set; } = 3;

    // Token: 0x1700002B RID: 43
    // (get) Token: 0x06000164 RID: 356 RVA: 0x00007A43 File Offset: 0x00005C43
    // (set) Token: 0x06000165 RID: 357 RVA: 0x00007A4B File Offset: 0x00005C4B
    public int music { get; set; } = 4;

    public int speedometer { get; set; } = 0;

    // Token: 0x06000166 RID: 358 RVA: 0x00007A54 File Offset: 0x00005C54
    public PlayerSave()
    {
        lastSkin = new int[15];
        skins = new bool[carsUnlocked.Length][];
        for (var i = 0; i < skins.Length; i++)
        {
            if (i == 5)
            {
                skins[i] = new bool[2];
            }
            else if (i > 5)
            {
                skins[i] = new bool[1];
            }
            else
            {
                skins[i] = new bool[5];
            }
            skins[i][0] = true;
        }
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            graphics = 0;
            quality = 0;
            motionBlur = 0;
            dof = 0;
        }
        daniTimes[0] = 42.11238f;
        daniTimes[1] = 51.41264f;
        daniTimes[2] = 76.41264f;
        daniTimes[3] = 79.27263f;
        daniTimes[4] = 114.1815f;
        daniTimes[5] = 126.096527f;
        mapsUnlocked[0] = true;
        carsUnlocked[0] = true;
        for (var j = 0; j < races.Length; j++)
        {
            races[j] = -1;
        }
    }

    // Token: 0x06000167 RID: 359 RVA: 0x00007BF6 File Offset: 0x00005DF6
    public int GetLevel(int xp) => Mathf.FloorToInt(PlayerSave.NthRoot(xp, y) * x);

    // Token: 0x06000168 RID: 360 RVA: 0x00007C11 File Offset: 0x00005E11
    public int GetLevel() => Mathf.FloorToInt(PlayerSave.NthRoot(xp, y) * x);

    // Token: 0x06000169 RID: 361 RVA: 0x00007C31 File Offset: 0x00005E31
    public int XpForLevel(int level) => (int)Mathf.Pow(level / x, y);

    // Token: 0x0600016A RID: 362 RVA: 0x00007C48 File Offset: 0x00005E48
    public float LevelProgress()
    {
        var num = (float)(xp - XpForLevel(GetLevel()));
        var num2 = XpForLevel(GetLevel() + 1) - XpForLevel(GetLevel());
        return num / num2;
    }

    // Token: 0x0600016B RID: 363 RVA: 0x00007C88 File Offset: 0x00005E88
    private static float NthRoot(float A, float N) => Mathf.Pow(A, 1f / N);

    // Token: 0x0600016C RID: 364 RVA: 0x00007C97 File Offset: 0x00005E97
    public static int GetSkinPrice(int car, int skin)
    {
        if (car < 5)
        {
            return 500 * (skin - 2) + car * 200;
        }
        return 9999;
    }

    // Token: 0x04000168 RID: 360
    public float[] times = new float[100];

    // Token: 0x04000169 RID: 361
    public int[] races = new int[100];

    // Token: 0x0400016A RID: 362
    public float[] daniTimes = new float[100];

    // Token: 0x0400016B RID: 363
    public bool[][] skins;

    // Token: 0x0400016C RID: 364
    public bool[] carsUnlocked = new bool[15];

    // Token: 0x0400016D RID: 365
    public bool[] mapsUnlocked = new bool[15];

    // Token: 0x0400016E RID: 366
    public int xp;

    // Token: 0x0400016F RID: 367
    public int money;

    // Token: 0x04000170 RID: 368
    public int lastCar;

    // Token: 0x04000171 RID: 369
    public int[] lastSkin;

    // Token: 0x04000172 RID: 370
    public int lastMap;

    // Token: 0x04000173 RID: 371
    public int lastDifficulty;

    // Token: 0x04000174 RID: 372
    public int lastGhost;

    // Token: 0x04000175 RID: 373
    private readonly float x = 0.07f;

    // Token: 0x04000176 RID: 374
    private readonly float y = 1.55f;
}
