using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class SaveState : MonoBehaviour
{
	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000126 RID: 294 RVA: 0x00007382 File Offset: 0x00005582
	// (set) Token: 0x06000127 RID: 295 RVA: 0x0000738A File Offset: 0x0000558A
	public int quality { get; set; }

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000128 RID: 296 RVA: 0x00007393 File Offset: 0x00005593
	// (set) Token: 0x06000129 RID: 297 RVA: 0x0000739B File Offset: 0x0000559B
	public int dof { get; set; }

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x0600012A RID: 298 RVA: 0x000073A4 File Offset: 0x000055A4
	// (set) Token: 0x0600012B RID: 299 RVA: 0x000073AC File Offset: 0x000055AC
	public int motionBlur { get; set; }

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x0600012C RID: 300 RVA: 0x000073B5 File Offset: 0x000055B5
	// (set) Token: 0x0600012D RID: 301 RVA: 0x000073BD File Offset: 0x000055BD
	public int cameraMode { get; set; }

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x0600012E RID: 302 RVA: 0x000073C6 File Offset: 0x000055C6
	// (set) Token: 0x0600012F RID: 303 RVA: 0x000073CE File Offset: 0x000055CE
	public int cameraShake { get; set; }

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000130 RID: 304 RVA: 0x000073D7 File Offset: 0x000055D7
	// (set) Token: 0x06000131 RID: 305 RVA: 0x000073DF File Offset: 0x000055DF
	public int muted { get; set; }

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000132 RID: 306 RVA: 0x000073E8 File Offset: 0x000055E8
	// (set) Token: 0x06000133 RID: 307 RVA: 0x000073F0 File Offset: 0x000055F0
	public int volume { get; set; }

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000134 RID: 308 RVA: 0x000073F9 File Offset: 0x000055F9
	// (set) Token: 0x06000135 RID: 309 RVA: 0x00007401 File Offset: 0x00005601
	public int music { get; set; }

	// Token: 0x06000136 RID: 310 RVA: 0x0000740A File Offset: 0x0000560A
	private void Awake()
	{
		if (SaveState.Instance != null && SaveState.Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SaveState.Instance = this;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00007438 File Offset: 0x00005638
	private void Start()
	{
		if (!SaveState.Instance)
		{
			return;
		}
		this.LoadSettings();
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00007450 File Offset: 0x00005650
	private void LoadSettings()
	{
		this.graphics = SaveManager.Instance.state.graphics;
		this.quality = SaveManager.Instance.state.quality;
		this.motionBlur = SaveManager.Instance.state.motionBlur;
		this.dof = SaveManager.Instance.state.dof;
		this.cameraMode = SaveManager.Instance.state.cameraMode;
		this.cameraShake = SaveManager.Instance.state.cameraShake;
		this.muted = SaveManager.Instance.state.muted;
		this.volume = SaveManager.Instance.state.volume;
		this.music = SaveManager.Instance.state.music;
	}

	// Token: 0x0400013E RID: 318
	public int graphics { get; set; }

	// Token: 0x04000147 RID: 327
	public static SaveState Instance;
}
