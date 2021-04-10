using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200001B RID: 27
public class GameState : MonoBehaviour
{
	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600009B RID: 155 RVA: 0x00005519 File Offset: 0x00003719
	// (set) Token: 0x0600009C RID: 156 RVA: 0x00005521 File Offset: 0x00003721
	public int car { get; set; } = 1;

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600009D RID: 157 RVA: 0x0000552A File Offset: 0x0000372A
	// (set) Token: 0x0600009E RID: 158 RVA: 0x00005532 File Offset: 0x00003732
	public int map { get; set; }

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x0600009F RID: 159 RVA: 0x0000553B File Offset: 0x0000373B
	// (set) Token: 0x060000A0 RID: 160 RVA: 0x00005543 File Offset: 0x00003743
	public Gamemode gamemode { get; set; }

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000554C File Offset: 0x0000374C
	// (set) Token: 0x060000A2 RID: 162 RVA: 0x00005554 File Offset: 0x00003754
	public int skin { get; set; } = 1;

	// Token: 0x060000A3 RID: 163 RVA: 0x0000555D File Offset: 0x0000375D
	private void Awake()
	{
		if (GameState.Instance != null && GameState.Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			GameState.Instance = this;
		}
		GameState.Instance = this;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00005592 File Offset: 0x00003792
	public void LoadMap()
	{
		SceneManager.LoadScene(string.Concat(this.map));
	}

	// Token: 0x040000CD RID: 205
	public GhostCycle.Ghost ghost;

	// Token: 0x040000CE RID: 206
	public DifficultyCycle.Difficulty difficulty = DifficultyCycle.Difficulty.Normal;

	// Token: 0x040000D1 RID: 209
	public static GameState Instance;
}
