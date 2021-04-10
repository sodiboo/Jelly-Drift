using System;
using TMPro;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class MenuStats : MonoBehaviour
{
	// Token: 0x060000DA RID: 218 RVA: 0x000060A4 File Offset: 0x000042A4
	private void Start()
	{
		MenuStats.Instance = this;
		this.UpdateStats();
		this.currMoney = (float)SaveManager.Instance.state.money;
		this.money.text = "$" + this.currMoney;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x000060F4 File Offset: 0x000042F4
	public void UpdateStats()
	{
		float x = SaveManager.Instance.state.LevelProgress();
		this.currentXp.transform.localScale = new Vector3(x, 1f, 1f);
		this.level.text = "Lvl" + SaveManager.Instance.state.GetLevel();
	}

	// Token: 0x060000DC RID: 220 RVA: 0x0000615C File Offset: 0x0000435C
	private void Update()
	{
		this.currMoney = Mathf.Lerp(this.currMoney, (float)SaveManager.Instance.state.money, Time.deltaTime * 3f);
		this.money.text = "$" + Mathf.CeilToInt(this.currMoney);
	}

	// Token: 0x04000100 RID: 256
	public TextMeshProUGUI level;

	// Token: 0x04000101 RID: 257
	public TextMeshProUGUI money;

	// Token: 0x04000102 RID: 258
	public Transform currentXp;

	// Token: 0x04000103 RID: 259
	public static MenuStats Instance;

	// Token: 0x04000104 RID: 260
	private float currMoney;
}
