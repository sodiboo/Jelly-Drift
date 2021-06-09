using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class ReplayController : MonoBehaviour
{
    // Token: 0x17000017 RID: 23
    // (get) Token: 0x06000117 RID: 279 RVA: 0x000070BB File Offset: 0x000052BB
    // (set) Token: 0x06000118 RID: 280 RVA: 0x000070C3 File Offset: 0x000052C3
    public int hz { get; set; } = 30;

    // Token: 0x17000018 RID: 24
    // (get) Token: 0x06000119 RID: 281 RVA: 0x000070CC File Offset: 0x000052CC
    // (set) Token: 0x0600011A RID: 282 RVA: 0x000070D4 File Offset: 0x000052D4
    public Car car { get; set; }

    // Token: 0x0600011B RID: 283 RVA: 0x000070DD File Offset: 0x000052DD
    private void Awake() => ReplayController.Instance = this;

    // Token: 0x0600011C RID: 284 RVA: 0x000070E8 File Offset: 0x000052E8
    private void Start()
    {
        var path = Application.persistentDataPath + "/replays";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        filePath = string.Concat(new object[]
        {
            Application.persistentDataPath,
            "/replays/pb",
            GameState.Instance.map,
            ".txt"
        });
        replay = new List<ReplayController.ReplayFrame>();
    }

    // Token: 0x0600011D RID: 285 RVA: 0x0000717C File Offset: 0x0000537C
    private void FixedUpdate()
    {
        if (!GameController.Instance || !car)
        {
            return;
        }
        replay.Add(new ReplayController.ReplayFrame(car.rb.position, car.rb.rotation.eulerAngles, car.steerAngle, Time.time));
    }

    // Token: 0x0600011E RID: 286 RVA: 0x000071EC File Offset: 0x000053EC
    public void Save()
    {
        MonoBehaviour.print("saving");
        var streamWriter = StreamWriter.Null;
        try
        {
            streamWriter = new StreamWriter(filePath, false);
            streamWriter.WriteLine(GameState.Instance.car + ", " + GameState.Instance.skin);
            foreach (var replayFrame in replay)
            {
                streamWriter.WriteLine(string.Concat(new object[]
                {
                    replayFrame.pos,
                    ", ",
                    replayFrame.rot,
                    ", ",
                    replayFrame.steerAngle,
                    ",",
                    replayFrame.time
                }));
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log(ex.Message);
        }
        finally
        {
            streamWriter.Close();
        }
    }

    // Token: 0x04000135 RID: 309
    private List<ReplayController.ReplayFrame> replay;

    // Token: 0x0400013A RID: 314
    private string filePath;

    // Token: 0x0400013B RID: 315
    public static ReplayController Instance;

    // Token: 0x02000064 RID: 100
    [Serializable]
    public class ReplayFrame
    {
        // Token: 0x0600022B RID: 555 RVA: 0x0000A7EE File Offset: 0x000089EE
        public ReplayFrame(Vector3 pos, Vector3 rot, float steerAngle, float time)
        {
            this.pos = pos;
            this.rot = rot;
            this.steerAngle = steerAngle;
            this.time = time;
        }

        // Token: 0x04000246 RID: 582
        public Vector3 pos;

        // Token: 0x04000247 RID: 583
        public Vector3 rot;

        // Token: 0x04000248 RID: 584
        public float steerAngle;

        // Token: 0x04000249 RID: 585
        public float time;
    }
}
