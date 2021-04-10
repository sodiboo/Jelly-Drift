using System;
using TMPro;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class AdButton : MonoBehaviour
{
	// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
	private void Awake()
	{
		AdButton.Instance = this;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002068 File Offset: 0x00000268
	private void Start()
	{
		if (!InitializeAds.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.text.text = "Watch Ad\n<size=70%>+500$";
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002092 File Offset: 0x00000292
	public void WatchAd()
	{
		if (!InitializeAds.Instance)
		{
			return;
		}
		InitializeAds.Instance.ShowRewardedVideo();
	}

	// Token: 0x04000001 RID: 1
	public TextMeshProUGUI text;

	// Token: 0x04000002 RID: 2
	public static AdButton Instance;
}
