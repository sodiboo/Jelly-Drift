using System;
using TMPro;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class ProgressController : MonoBehaviour
{
	// Token: 0x06000107 RID: 263 RVA: 0x0000666B File Offset: 0x0000486B
	private void Awake()
	{
		this.defaultLevelScale = this.level.transform.localScale;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00006684 File Offset: 0x00004884
	public void SetProgress(int oldXp, int newXp, int oldLevel, int oldMoney, int newMoney)
	{
		this.xp = (float)oldXp;
		this.desiredXp = newXp;
		this.currentLevel = oldLevel;
		this.currentMoney = (float)oldMoney;
		this.desiredMoney = (float)newMoney;
		this.level.text = "Lvl " + oldLevel;
		this.money.text = "$" + oldMoney;
		this.xpGained.text = "+ " + (newXp - oldXp) + "xp";
		this.moneyGained.text = "+ " + (newMoney - oldMoney) + "$";
		float num = (float)((int)this.xp - SaveManager.Instance.state.XpForLevel(this.currentLevel));
		int num2 = SaveManager.Instance.state.XpForLevel(this.currentLevel + 1) - SaveManager.Instance.state.XpForLevel(this.currentLevel);
		float x = num / (float)num2;
		this.progress.localScale = new Vector3(x, 1f, 1f);
		base.Invoke("GetReady", 0.5f);
	}

	// Token: 0x06000109 RID: 265 RVA: 0x000067B1 File Offset: 0x000049B1
	private void GetReady()
	{
		this.ready = true;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x000067BC File Offset: 0x000049BC
	private void Update()
	{
		if (!this.ready || (UnlockManager.Instance && UnlockManager.Instance.gameObject.activeInHierarchy))
		{
			return;
		}
		this.xp = Mathf.Lerp(this.xp, (float)this.desiredXp, Time.fixedDeltaTime * 0.5f);
		this.currentMoney = Mathf.Lerp(this.currentMoney, this.desiredMoney, Time.fixedDeltaTime * 0.5f);
		float num = (float)((int)this.xp - SaveManager.Instance.state.XpForLevel(this.currentLevel));
		int num2 = SaveManager.Instance.state.XpForLevel(this.currentLevel + 1) - SaveManager.Instance.state.XpForLevel(this.currentLevel);
		float x = num / (float)num2;
		this.progress.localScale = new Vector3(x, 1f, 1f);
		this.money.text = "$" + Mathf.CeilToInt(this.currentMoney);
		if (SaveManager.Instance.state.GetLevel((int)this.xp) > this.currentLevel)
		{
			MonoBehaviour.print("levelled up!");
			this.level.transform.localScale = this.defaultLevelScale * 1.5f;
			this.currentLevel++;
			this.level.text = "Lvl " + this.currentLevel;
		}
		this.ScaleLevel();
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00006940 File Offset: 0x00004B40
	private void ScaleLevel()
	{
		this.level.transform.localScale = Vector3.Lerp(this.level.transform.localScale, this.defaultLevelScale, Time.deltaTime * 1f);
	}

	// Token: 0x0400011E RID: 286
	public Transform progress;

	// Token: 0x0400011F RID: 287
	public TextMeshProUGUI level;

	// Token: 0x04000120 RID: 288
	public TextMeshProUGUI money;

	// Token: 0x04000121 RID: 289
	public TextMeshProUGUI xpGained;

	// Token: 0x04000122 RID: 290
	public TextMeshProUGUI moneyGained;

	// Token: 0x04000123 RID: 291
	private float currentMoney;

	// Token: 0x04000124 RID: 292
	private float desiredMoney;

	// Token: 0x04000125 RID: 293
	private float xp;

	// Token: 0x04000126 RID: 294
	private int desiredXp;

	// Token: 0x04000127 RID: 295
	private int currentLevel;

	// Token: 0x04000128 RID: 296
	private bool ready;

	// Token: 0x04000129 RID: 297
	private Vector3 defaultLevelScale;
}
