using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class MoveImage : MonoBehaviour
{
	// Token: 0x060000E5 RID: 229 RVA: 0x000062C6 File Offset: 0x000044C6
	private void Update()
	{
		base.transform.localPosition += new Vector3(this.speed, 0f, 0f) * Time.deltaTime;
	}

	// Token: 0x0400010B RID: 267
	private float speed = 3f;
}
