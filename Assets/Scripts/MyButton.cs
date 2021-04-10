using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200002D RID: 45
public class MyButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x060000EC RID: 236 RVA: 0x00006395 File Offset: 0x00004595
	public void OnPointerDown(PointerEventData eventData)
	{
		this.value = 1;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0000639E File Offset: 0x0000459E
	public void OnPointerUp(PointerEventData eventData)
	{
		this.value = 0;
	}

	// Token: 0x0400010E RID: 270
	public int value;
}
