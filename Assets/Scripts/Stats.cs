using System;
using TMPro;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class Stats : MonoBehaviour
{
	// Token: 0x060001C1 RID: 449 RVA: 0x00009474 File Offset: 0x00007674
	private void OnEnable()
	{
		MonoBehaviour.print("text:  " + this.text);
		this.text.text = "<size=110%>Times\n<size=75%>";
		for (int i = 0; i < MapManager.Instance.maps.Length; i++)
		{
			string name = MapManager.Instance.maps[i].name;
			string formattedTime = Timer.GetFormattedTime(SaveManager.Instance.state.times[i]);
			TextMeshProUGUI textMeshProUGUI = this.text;
			textMeshProUGUI.text = string.Concat(new string[]
			{
				textMeshProUGUI.text,
				name,
				" - ",
				formattedTime,
				"\n"
			});
		}
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00008C58 File Offset: 0x00006E58
	public void DeleteSave()
	{
		SaveManager.Instance.NewSave();
		SaveManager.Instance.Save();
	}

	// Token: 0x040001C8 RID: 456
	public TextMeshProUGUI text;
}
