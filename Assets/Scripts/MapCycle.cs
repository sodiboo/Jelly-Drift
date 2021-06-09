using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000023 RID: 35
public class MapCycle : ItemCycle
{
    // Token: 0x060000C6 RID: 198 RVA: 0x00005B1E File Offset: 0x00003D1E
    private void Awake() => base.selected = SaveManager.Instance.state.lastMap;

    // Token: 0x060000C7 RID: 199 RVA: 0x00005B38 File Offset: 0x00003D38
    private void Start()
    {
        if (starsDetails)
        {
            starTimes = starsDetails.GetComponentsInChildren<TextMeshProUGUI>();
        }
        SetMap(base.selected);
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
        SetMap(base.selected);
    }

    // Token: 0x060000C9 RID: 201 RVA: 0x00005BE2 File Offset: 0x00003DE2
    public override void Cycle(int n)
    {
        base.Cycle(n);
        SetMap(base.selected);
        GameState.Instance.map = base.selected;
    }

    // Token: 0x060000CA RID: 202 RVA: 0x00005C08 File Offset: 0x00003E08
    private void Update()
    {
        if (lockUi.activeInHierarchy)
        {
            overlay.color = Color.Lerp(overlay.color, new Color(1f, 1f, 1f, 0.55f), Time.deltaTime * 1.2f);
            return;
        }
        overlay.color = Color.Lerp(overlay.color, MapManager.Instance.maps[base.selected].themeColor, Time.deltaTime * 0.9f);
    }

    // Token: 0x060000CB RID: 203 RVA: 0x00005CA0 File Offset: 0x00003EA0
    private void SetMap(int n)
    {
        if (raceDetails)
        {
            raceDetails.UpdateStars(base.selected);
        }
        if (SaveManager.Instance.state.mapsUnlocked[n])
        {
            lockUi.SetActive(false);
            mapImg.sprite = MapManager.Instance.maps[n].image;
            name.text = "| " + MapManager.Instance.maps[n].name;
            time.text = "PB - " + Timer.GetFormattedTime(SaveManager.Instance.state.times[n]);
            time.enableWordWrapping = false;
            if (ghostCycle)
            {
                ghostCycle.UpdateText();
            }
            if (difficultyCycle)
            {
                difficultyCycle.UpdateTextOnly();
            }
            if (starsDetails)
            {
                UpdateStars();
            }
            GameState.Instance.map = base.selected;
            nextButton.enabled = true;
            nextButton.GetComponent<ItemCycle>().activeCycle = true;
            SaveManager.Instance.state.lastMap = base.selected;
            SaveManager.Instance.Save();
            return;
        }
        lockUi.SetActive(true);
        mapImg.sprite = MapManager.Instance.maps[n].image;
        name.text = "| <size=60%>Complete " + MapManager.Instance.maps[n - 1].name + " on easy difficulty";
        time.text = "";
        ghostText.text = "| ";
        nextButton.enabled = false;
        nextButton.GetComponent<ItemCycle>().activeCycle = false;
    }

    // Token: 0x060000CC RID: 204 RVA: 0x00005E7C File Offset: 0x0000407C
    private void UpdateStars()
    {
        MonoBehaviour.print(starTimes.Length);
        for (var i = 0; i < starTimes.Length; i++)
        {
            starTimes[i].text = Timer.GetFormattedTime(MapManager.Instance.maps[base.selected].times[i]);
        }
        var stars = MapManager.Instance.GetStars(base.selected, SaveManager.Instance.state.times[base.selected]);
        for (var j = 0; j < pbStars.Length; j++)
        {
            if (j < stars)
            {
                pbStars[j].color = Color.yellow;
            }
            else
            {
                pbStars[j].color = Color.gray;
            }
        }
    }

    // Token: 0x060000CD RID: 205 RVA: 0x00005F3C File Offset: 0x0000413C
    public void SaveMap() => GameState.Instance.map = base.selected;

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
