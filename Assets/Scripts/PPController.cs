using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200002F RID: 47
public class PPController : MonoBehaviour
{
    // Token: 0x060000F3 RID: 243 RVA: 0x000063F0 File Offset: 0x000045F0
    private void Awake()
    {
        PPController.Instance = this;
        volume = base.GetComponent<PostProcessVolume>();
        profile = volume.profile;
        motionBlur = profile.GetSetting<MotionBlur>();
        dof = profile.GetSetting<DepthOfField>();
    }

    // Token: 0x060000F4 RID: 244 RVA: 0x00006442 File Offset: 0x00004642
    private void Start() => LoadSettings();

    // Token: 0x060000F5 RID: 245 RVA: 0x0000644C File Offset: 0x0000464C
    public void LoadSettings()
    {
        if (SaveState.Instance.graphics != 1)
        {
            volume.enabled = false;
            return;
        }
        volume.enabled = true;
        if (SaveState.Instance.motionBlur == 1)
        {
            motionBlur.enabled.value = true;
        }
        else
        {
            motionBlur.enabled.value = false;
        }
        if (SaveState.Instance.dof == 1)
        {
            dof.enabled.value = true;
            return;
        }
        dof.enabled.value = false;
    }

    // Token: 0x04000110 RID: 272
    private PostProcessProfile profile;

    // Token: 0x04000111 RID: 273
    private PostProcessVolume volume;

    // Token: 0x04000112 RID: 274
    private MotionBlur motionBlur;

    // Token: 0x04000113 RID: 275
    public DepthOfField dof;

    // Token: 0x04000114 RID: 276
    public static PPController Instance;
}
