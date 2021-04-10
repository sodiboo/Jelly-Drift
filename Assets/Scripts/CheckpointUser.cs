using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class CheckpointUser : MonoBehaviour
{
	// Token: 0x0600013A RID: 314 RVA: 0x0000751A File Offset: 0x0000571A
	private void Awake()
	{
		this.checkedPoints = new bool[GameController.Instance.checkPoints.childCount];
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00007538 File Offset: 0x00005738
	public int GetCurrentCheckpoint(bool loopMap)
	{
		int num = 0;
		int num2 = 1;
		if (!loopMap)
		{
			num2 = 0;
		}
		int num3 = num2;
		while (num3 < this.checkedPoints.Length && this.checkedPoints[num3])
		{
			num++;
			num3++;
		}
		if (!loopMap)
		{
			return num - 1;
		}
		return num;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00007578 File Offset: 0x00005778
	public bool CheckPoint(CheckPoint p)
	{
		if (p.nr != GameController.Instance.finalCheckpoint)
		{
			this.ClearCheckpoint(p.nr);
			return true;
		}
		if (this.ReadyToFinish())
		{
			GameController.Instance.FinishRace(this.player, base.transform);
			this.ClearCheckpoint(p.nr);
			return true;
		}
		return false;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x000075D4 File Offset: 0x000057D4
	private void ClearCheckpoint(int n)
	{
		if (this.checkedPoints[n])
		{
			return;
		}
		if (GameController.Instance.finalCheckpoint > 0 && n == 0)
		{
			this.checkedPoints[0] = true;
			return;
		}
		this.checkedPoints[n] = true;
		if (!this.player)
		{
			return;
		}
		SplitUi component = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.splitUi).GetComponent<SplitUi>();
		component.transform.SetParent(UIManager.Instance.splitPos);
		component.transform.localPosition = Vector3.zero;
		component.SetSplit(Timer.GetFormattedTime(Timer.Instance.GetTimer()));
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00007665 File Offset: 0x00005865
	public void ForceCheckpoint(int n)
	{
		this.checkedPoints[n] = true;
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00007670 File Offset: 0x00005870
	private bool ReadyToFinish()
	{
		int num = 0;
		for (int i = 0; i < this.checkedPoints.Length; i++)
		{
			if (this.checkedPoints[i])
			{
				num++;
			}
		}
		return num >= this.checkedPoints.Length - 1;
	}

	// Token: 0x04000148 RID: 328
	private bool[] checkedPoints;

	// Token: 0x04000149 RID: 329
	public bool player = true;
}
