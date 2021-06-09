using UnityEngine;

// Token: 0x0200004D RID: 77
public class SoundManager : MonoBehaviour
{
    // Token: 0x060001B2 RID: 434 RVA: 0x0000924C File Offset: 0x0000744C
    private void Awake()
    {
        if (SoundManager.Instance != null && SoundManager.Instance != this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            return;
        }
        SoundManager.Instance = this;
    }

    // Token: 0x060001B3 RID: 435 RVA: 0x0000927A File Offset: 0x0000747A
    public void PlayCycle() => PlaySound(cycle);

    // Token: 0x060001B4 RID: 436 RVA: 0x00009288 File Offset: 0x00007488
    public void PlayUnlock() => PlaySoundDelayed(unlock, 0.1f);

    // Token: 0x060001B5 RID: 437 RVA: 0x0000929B File Offset: 0x0000749B
    public void PlayError() => PlaySound(error);

    // Token: 0x060001B6 RID: 438 RVA: 0x000092A9 File Offset: 0x000074A9
    public void PlayMoney() => PlaySound(buy);

    // Token: 0x060001B7 RID: 439 RVA: 0x000092B7 File Offset: 0x000074B7
    public void PlayMenuNavigate() => PlaySound(menu);

    // Token: 0x060001B8 RID: 440 RVA: 0x000092C5 File Offset: 0x000074C5
    public void PlaySound(AudioClip c)
    {
        audio.clip = c;
        audio.Play();
    }

    // Token: 0x060001B9 RID: 441 RVA: 0x000092DE File Offset: 0x000074DE
    public void PlaySoundDelayed(AudioClip c, float d)
    {
        audio.clip = c;
        audio.PlayDelayed(d);
    }

    // Token: 0x040001BC RID: 444
    public static SoundManager Instance;

    // Token: 0x040001BD RID: 445
    public AudioClip cycle;

    // Token: 0x040001BE RID: 446
    public AudioClip menu;

    // Token: 0x040001BF RID: 447
    public AudioClip buy;

    // Token: 0x040001C0 RID: 448
    public AudioClip unlock;

    // Token: 0x040001C1 RID: 449
    public AudioClip error;

    // Token: 0x040001C2 RID: 450
    public AudioSource audio;
}
