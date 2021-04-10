using System;
using TMPro;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class Timer : MonoBehaviour
{
	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000142 RID: 322 RVA: 0x000076EF File Offset: 0x000058EF
	// (set) Token: 0x06000143 RID: 323 RVA: 0x000076F6 File Offset: 0x000058F6
	public static Timer Instance { get; set; }

	// Token: 0x06000144 RID: 324 RVA: 0x000076FE File Offset: 0x000058FE
	private void Awake()
	{
		Timer.Instance = this;
		this.text = base.GetComponent<TextMeshProUGUI>();
		this.stop = false;
		this.StartTimer();
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000771F File Offset: 0x0000591F
	public void StartTimer()
	{
		this.stop = false;
		this.timer = 0f;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00007734 File Offset: 0x00005934
	private void Update()
	{
		if (!GameController.Instance)
		{
			return;
		}
		if (!GameController.Instance.playing || this.stop)
		{
			return;
		}
		this.timer += Time.deltaTime;
		AutoSplitterData.inGameTime = (double)this.timer;
		this.text.text = Timer.GetFormattedTime(this.timer);
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00007798 File Offset: 0x00005998
	public static string GetFormattedTime(float f)
	{
		if (f == 0f)
		{
			return "no time";
		}
		string arg = Mathf.Floor(f / 60f).ToString("00");
		string arg2 = Mathf.Floor(f % 60f).ToString("00");
		(f * 1000f % 1000f).ToString("000");
		string text = (f * 100f % 100f).ToString("00");
		if (text.Equals("100"))
		{
			text = "99";
		}
		return string.Format("{0}:{1}:{2}", arg, arg2, text);
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000783D File Offset: 0x00005A3D
	public float GetTimer()
	{
		return this.timer;
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00007845 File Offset: 0x00005A45
	public void Stop()
	{
		this.stop = true;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x0000784E File Offset: 0x00005A4E
	public int GetMinutes()
	{
		return (int)Mathf.Floor(this.timer / 60f);
	}

	// Token: 0x04000153 RID: 339
	private TextMeshProUGUI text;

	// Token: 0x04000154 RID: 340
	private float timer;

	// Token: 0x04000155 RID: 341
	private bool stop;
}
