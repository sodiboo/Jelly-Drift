using UnityEngine;

// Token: 0x0200004E RID: 78
public class StartLight : MonoBehaviour
{
    // Token: 0x060001BB RID: 443 RVA: 0x000092F8 File Offset: 0x000074F8
    private void Start()
    {
        rend = base.GetComponent<MeshRenderer>();
        colors = rend.materials;
        SetColor(-1);
        base.Invoke(nameof(NextColor), GameController.Instance.startTime / 3f);
    }

    // Token: 0x060001BC RID: 444 RVA: 0x00009344 File Offset: 0x00007544
    private void NextColor()
    {
        SetColor(c);
        if (audio)
        {
            audio.pitch = 1f + c * 0.5f / 3f;
            audio.Play();
        }
        c++;
        if (c < 3)
        {
            base.Invoke(nameof(NextColor), GameController.Instance.startTime / 3f);
        }
    }

    // Token: 0x060001BD RID: 445 RVA: 0x000093CC File Offset: 0x000075CC
    private void SetColor(int c)
    {
        var array = new Material[colors.Length];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = colors[i];
        }
        for (var j = 0; j < array.Length; j++)
        {
            if (j == c + 1)
            {
                array[j] = colors[j];
            }
            else
            {
                array[j] = colors[0];
            }
        }
        rend.materials = array;
    }

    // Token: 0x040001C3 RID: 451
    private MeshRenderer rend;

    // Token: 0x040001C4 RID: 452
    public Material[] colors;

    // Token: 0x040001C5 RID: 453
    public new AudioSource audio;

    // Token: 0x040001C6 RID: 454
    private int c;
}
