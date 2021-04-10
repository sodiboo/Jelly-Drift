using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class Water : MonoBehaviour
{
	// Token: 0x060001D5 RID: 469 RVA: 0x00009CA6 File Offset: 0x00007EA6
	private void Start()
	{
		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			base.GetComponent<MeshRenderer>().material = this.bad;
		}
	}

	// Token: 0x040001FC RID: 508
	public Material bad;
}
