using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class MenuSounds : MonoBehaviour
{
	// Token: 0x060000D8 RID: 216 RVA: 0x00006097 File Offset: 0x00004297
	public void Play()
	{
		SoundManager.Instance.PlayMenuNavigate();
	}
}
