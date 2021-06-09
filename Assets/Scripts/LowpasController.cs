using UnityEngine;

// Token: 0x02000022 RID: 34
public class LowpasController : MonoBehaviour
{
    // Token: 0x060000C3 RID: 195 RVA: 0x00005A82 File Offset: 0x00003C82
    private void Awake()
    {
        lowpass = base.GetComponent<AudioLowPassFilter>();
        MonoBehaviour.print("got lowpass: " + lowpass);
    }

    // Token: 0x060000C4 RID: 196 RVA: 0x00005AA8 File Offset: 0x00003CA8
    private void Update()
    {
        if (Pause.Instance.paused)
        {
            desiredFreq = 200f;
        }
        else if (Time.timeScale < 1f)
        {
            desiredFreq = 500f;
        }
        else
        {
            desiredFreq = 22000f;
        }
        lowpass.cutoffFrequency = Mathf.Lerp(lowpass.cutoffFrequency, desiredFreq, Time.fixedDeltaTime * 4f);
    }

    // Token: 0x040000E3 RID: 227
    private AudioLowPassFilter lowpass;

    // Token: 0x040000E4 RID: 228
    private float desiredFreq;
}
