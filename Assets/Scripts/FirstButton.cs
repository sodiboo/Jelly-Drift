using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000019 RID: 25
public class FirstButton : MonoBehaviour
{
	// Token: 0x06000087 RID: 135 RVA: 0x000051F5 File Offset: 0x000033F5
	private void Awake()
	{
		this.btn = base.GetComponent<Button>();
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00005203 File Offset: 0x00003403
	public void SelectButton()
	{
		this.btn.Select();
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00005203 File Offset: 0x00003403
	private void Start()
	{
		this.btn.Select();
	}

	// Token: 0x040000BF RID: 191
	private Button btn;
}
