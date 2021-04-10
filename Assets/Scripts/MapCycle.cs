using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000023 RID: 35
public class MapCycle : ItemCycle
{
	// Token: 0x060000C6 RID: 198 RVA: 0x00005B1E File Offset: 0x00003D1E
	private void Awake()
	{
		base.selected = SaveManager.Instance.state.lastMap;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00005B38 File Offset: 0x00003D38
	private void Start()
	{
		if (this.starsDetails)
		{
			this.starTimes = this.starsDetails.GetComponentsInChildren<TextMeshProUGUI>();
		}
		this.SetMap(base.selected);
		base.max = MapManager.Instance.maps.Length;
		CarDisplay.Instance.Hide();
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00005B8C File Offset: 0x00003D8C
	private void OnEnable()
	{
		if (!CarDisplay.Instance || !CarDisplay.Instance.currentCar)
		{
			return;
		}
		CarDisplay.Instance.Hide();
		base.selected = SaveManager.Instance.state.lastMap;
		this.SetMap(base.selected);
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005BE2 File Offset: 0x00003DE2
	public override void Cycle(int n)
	{
		base.Cycle(n);
		this.SetMap(base.selected);
		GameState.Instance.map = base.selected;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005C08 File Offset: 0x00003E08
	private void Update()
	{
		if (this.lockUi.activeInHierarchy)
		{
			this.overlay.color = Color.Lerp(this.overlay.color, new Color(1f, 1f, 1f, 0.55f), Time.deltaTime * 1.2f);
			return;
		}
		this.overlay.color = Color.Lerp(this.overlay.color, MapManager.Instance.maps[base.selected].themeColor, Time.deltaTime * 0.9f);
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005CA0 File Offset: 0x00003EA0
	private void SetMap(int n)
	{
		if (this.raceDetails)
		{
			this.raceDetails.UpdateStars(base.selected);
		}
		if (SaveManager.Instance.state.mapsUnlocked[n])
		{
			this.lockUi.SetActive(false);
			this.mapImg.sprite = MapManager.Instance.maps[n].image;
			this.name.text = "| " + MapManager.Instance.maps[n].name;
			this.time.text = "PB - " + Timer.GetFormattedTime(SaveManager.Instance.state.times[n]);
			if (this.ghostCycle)
			{
				this.ghostCycle.UpdateText();
			}
			if (this.difficultyCycle)
			{
				this.difficultyCycle.UpdateTextOnly();
			}
			if (this.starsDetails)
			{
				this.UpdateStars();
			}
			GameState.Instance.map = base.selected;
			this.nextButton.enabled = true;
			this.nextButton.GetComponent<ItemCycle>().activeCycle = true;
			SaveManager.Instance.state.lastMap = base.selected;
			SaveManager.Instance.Save();
			return;
		}
		this.lockUi.SetActive(true);
		this.mapImg.sprite = MapManager.Instance.maps[n].image;
		this.name.text = "| <size=60%>Complete " + MapManager.Instance.maps[n - 1].name + " on easy difficulty";
		this.time.text = "";
		this.ghostText.text = "| ";
		this.nextButton.enabled = false;
		this.nextButton.GetComponent<ItemCycle>().activeCycle = false;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00005E7C File Offset: 0x0000407C
	private void UpdateStars()
	{
		MonoBehaviour.print(this.starTimes.Length);
		for (int i = 0; i < this.starTimes.Length; i++)
		{
			this.starTimes[i].text = Timer.GetFormattedTime(MapManager.Instance.maps[base.selected].times[i]);
		}
		int stars = MapManager.Instance.GetStars(base.selected, SaveManager.Instance.state.times[base.selected]);
		for (int j = 0; j < this.pbStars.Length; j++)
		{
			if (j < stars)
			{
				this.pbStars[j].color = Color.yellow;
			}
			else
			{
				this.pbStars[j].color = Color.gray;
			}
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005F3C File Offset: 0x0000413C
	public void SaveMap()
	{
		GameState.Instance.map = base.selected;
		GameState.Instance.gamemode = this.gamemode;
	}

	// Token: 0x040000E5 RID: 229
	public Image mapImg;

	// Token: 0x040000E6 RID: 230
	public Image overlay;

	// Token: 0x040000E7 RID: 231
	public new TextMeshProUGUI name;

	// Token: 0x040000E8 RID: 232
	public TextMeshProUGUI time;

	// Token: 0x040000E9 RID: 233
	public GhostCycle ghostCycle;

	// Token: 0x040000EA RID: 234
	public DifficultyCycle difficultyCycle;

	// Token: 0x040000EB RID: 235
	public TextMeshProUGUI ghostText;

	// Token: 0x040000EC RID: 236
	public Button nextButton;

	// Token: 0x040000ED RID: 237
	public Transform starsDetails;

	// Token: 0x040000EE RID: 238
	public TextMeshProUGUI[] starTimes;

	// Token: 0x040000EF RID: 239
	public Image[] pbStars;

	// Token: 0x040000F0 RID: 240
	public GameObject lockUi;

	// Token: 0x040000F1 RID: 241
	public Gamemode gamemode;

	// Token: 0x040000F2 RID: 242
	public RaceDetails raceDetails;
}
