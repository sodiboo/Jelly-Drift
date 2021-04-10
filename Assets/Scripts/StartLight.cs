using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class StartLight : MonoBehaviour
{
	// Token: 0x060001BB RID: 443 RVA: 0x000092F8 File Offset: 0x000074F8
	private void Start()
	{
		this.rend = base.GetComponent<MeshRenderer>();
		this.colors = this.rend.materials;
		this.SetColor(-1);
		base.Invoke("NextColor", GameController.Instance.startTime / 3f);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00009344 File Offset: 0x00007544
	private void NextColor()
	{
		this.SetColor(this.c);
		if (this.audio)
		{
			this.audio.pitch = 1f + (float)this.c * 0.5f / 3f;
			this.audio.Play();
		}
		this.c++;
		if (this.c < 3)
		{
			base.Invoke("NextColor", GameController.Instance.startTime / 3f);
		}
	}

	// Token: 0x060001BD RID: 445 RVA: 0x000093CC File Offset: 0x000075CC
	private void SetColor(int c)
	{
		Material[] array = new Material[this.colors.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.colors[i];
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (j == c + 1)
			{
				array[j] = this.colors[j];
			}
			else
			{
				array[j] = this.colors[0];
			}
		}
		this.rend.materials = array;
	}

	// Token: 0x040001C3 RID: 451
	private MeshRenderer rend;

	// Token: 0x040001C4 RID: 452
	public Material[] colors;

	// Token: 0x040001C5 RID: 453
	public AudioSource audio;

	// Token: 0x040001C6 RID: 454
	private int c;
}
