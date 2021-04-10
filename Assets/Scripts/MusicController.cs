using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class MusicController : MonoBehaviour
{
	// Token: 0x060000E7 RID: 231 RVA: 0x00006310 File Offset: 0x00004510
	private void Awake()
	{
		if (MusicController.Instance != null && MusicController.Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			MusicController.Instance = this;
		}
		this.music = base.GetComponent<AudioSource>();
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000634B File Offset: 0x0000454B
	private void Start()
	{
		if (!MusicController.Instance)
		{
			return;
		}
		this.UpdateMusic((float)SaveState.Instance.music);
		AudioListener.volume = (float)SaveState.Instance.volume / 10f;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x000020AB File Offset: 0x000002AB
	private void Update()
	{
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00006381 File Offset: 0x00004581
	public void UpdateMusic(float f)
	{
		this.music.volume = f / 10f;
	}

	// Token: 0x0400010C RID: 268
	private AudioSource music;

	// Token: 0x0400010D RID: 269
	public static MusicController Instance;
}
