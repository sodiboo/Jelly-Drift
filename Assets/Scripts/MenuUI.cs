using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000029 RID: 41
public class MenuUI : MonoBehaviour
{
	// Token: 0x060000DE RID: 222 RVA: 0x000061BA File Offset: 0x000043BA
	private void Start()
	{
		Application.targetFrameRate = 144;
		QualitySettings.vSyncCount = 0;
		MonoBehaviour.print("counting");
		if (InitializeAds.Instance)
		{
			InitializeAds.Instance.MenuCount();
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000061EC File Offset: 0x000043EC
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000061F4 File Offset: 0x000043F4
	public void Quit()
	{
		Application.Quit(1);
	}
}
