using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000034 RID: 52
public class RaceDetails : MonoBehaviour
{
	// Token: 0x0600010D RID: 269 RVA: 0x00006978 File Offset: 0x00004B78
	public void UpdateStars(int map)
	{
		int num = SaveManager.Instance.state.races[map] + 1;
		if (num <= 0)
		{
			this.text.text = "None";
		}
		else
		{
			this.text.text = ((DifficultyCycle.Difficulty)(num - 1)).ToString();
		}
		for (int i = 0; i < this.pbStars.Length; i++)
		{
			if (i < num)
			{
				this.pbStars[i].color = Color.yellow;
			}
			else
			{
				this.pbStars[i].color = Color.gray;
			}
		}
	}

	// Token: 0x0400012A RID: 298
	public Image[] pbStars;

	// Token: 0x0400012B RID: 299
	public TextMeshProUGUI text;
}
