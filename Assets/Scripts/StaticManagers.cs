using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class StaticManagers : MonoBehaviour
{
	// Token: 0x060001BF RID: 447 RVA: 0x00009438 File Offset: 0x00007638
	private void Awake()
	{
		if (StaticManagers.Instance != null && StaticManagers.Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		StaticManagers.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x040001C7 RID: 455
	public static StaticManagers Instance;
}
