using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000018 RID: 24
public class FinishController : MonoBehaviour
{
	// Token: 0x06000081 RID: 129 RVA: 0x00004AF3 File Offset: 0x00002CF3
	private void Awake()
	{
		FinishController.Instance = this;
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00004AFC File Offset: 0x00002CFC
	public void Open(bool victory)
	{
		InputManager.Instance.layout = InputManager.Layout.Menu;
		if (GameState.Instance.gamemode == Gamemode.TimeTrial)
		{
			this.timePanel.SetActive(true);
			int map = GameState.Instance.map;
			this.mapName.text = MapManager.Instance.maps[map].name;
			float num = Timer.Instance.GetTimer();
			float num2 = SaveManager.Instance.state.times[map];
			if (num < num2 || num2 == 0f)
			{
				SaveManager.Instance.state.times[map] = num;
				SaveManager.Instance.Save();
				this.newBest.gameObject.SetActive(true);
				ReplayController.Instance.Save();
			}
			int num3 = MapManager.Instance.GetStars(map, num);
			MonoBehaviour.print(string.Concat(new object[]
			{
				"checking stars for time: ",
				num,
				", stars: ",
				num3
			}));
			for (int i = 0; i < num3; i++)
			{
				this.stars[i].color = Color.yellow;
			}
			this.timer.text = Timer.GetFormattedTime(num);
			this.pbTimer.text = "Best | " + Timer.GetFormattedTime(SaveManager.Instance.state.times[map]);
		}
		else if (GameState.Instance.gamemode == Gamemode.Race)
		{
			this.racePanel.SetActive(true);
			if (victory)
			{
				this.victoryText.text = "Victory";
			}
			else
			{
				this.victoryText.text = "Defeat";
			}
		}
		this.CheckUnlocks(victory);
		int num4 = 50;
		int num5 = 50;
		if (GameState.Instance.gamemode == Gamemode.Race)
		{
			this.progressRace.SetProgress(SaveManager.Instance.state.xp, SaveManager.Instance.state.xp + num4, SaveManager.Instance.state.GetLevel(), SaveManager.Instance.state.money, SaveManager.Instance.state.money + num5);
		}
		else
		{
			this.progressTime.SetProgress(SaveManager.Instance.state.xp, SaveManager.Instance.state.xp + num4, SaveManager.Instance.state.GetLevel(), SaveManager.Instance.state.money, SaveManager.Instance.state.money + num5);
		}
		SaveManager.Instance.state.xp += num4;
		SaveManager.Instance.state.money += num5;
		if (this.unlockManager.unlocks.Count > 0)
		{
			this.unlockManager.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00004DB0 File Offset: 0x00002FB0
	private void CheckUnlocks(bool victory)
	{
		int map = GameState.Instance.map;
		if (GameState.Instance.gamemode == Gamemode.Race && victory)
		{
			int num = SaveManager.Instance.state.races[map];
			int difficulty = (int)GameState.Instance.difficulty;
			if (difficulty > num)
			{
				SaveManager.Instance.state.races[map] = difficulty;
				SaveManager.Instance.Save();
			}
			int num2 = GameState.Instance.map + 1;
			if (GameState.Instance.difficulty >= DifficultyCycle.Difficulty.Easy && num2 < MapManager.Instance.maps.Length && !SaveManager.Instance.state.mapsUnlocked[num2])
			{
				SaveManager.Instance.state.mapsUnlocked[num2] = true;
				SaveManager.Instance.Save();
				this.unlockManager.unlocks.Add(new UnlockManager.Unlock(UnlockManager.UnlockType.Map, num2, 0));
			}
			if (GameState.Instance.difficulty >= DifficultyCycle.Difficulty.Normal && !SaveManager.Instance.state.carsUnlocked[num2])
			{
				SaveManager.Instance.state.carsUnlocked[num2] = true;
				SaveManager.Instance.Save();
				this.unlockManager.unlocks.Add(new UnlockManager.Unlock(UnlockManager.UnlockType.Car, num2, 0));
			}
			if (GameState.Instance.difficulty >= DifficultyCycle.Difficulty.Hard && !SaveManager.Instance.state.skins[GameState.Instance.map][1])
			{
				SaveManager.Instance.state.skins[GameState.Instance.map][1] = true;
				SaveManager.Instance.Save();
				this.unlockManager.unlocks.Add(new UnlockManager.Unlock(UnlockManager.UnlockType.Skin, GameState.Instance.map, 1));
			}
		}
		if (GameState.Instance.gamemode == Gamemode.TimeTrial && MapManager.Instance.GetStars(GameState.Instance.map, Timer.Instance.GetTimer()) == 3 && !SaveManager.Instance.state.skins[map][2])
		{
			SaveManager.Instance.state.skins[map][2] = true;
			SaveManager.Instance.Save();
			this.unlockManager.unlocks.Add(new UnlockManager.Unlock(UnlockManager.UnlockType.Skin, map, 2));
		}
		if (!SaveManager.Instance.state.carsUnlocked[6])
		{
			int num3 = 0;
			while (num3 < MapManager.Instance.maps.Length && SaveManager.Instance.state.races[num3] >= 2)
			{
				if (num3 == MapManager.Instance.maps.Length - 1)
				{
					SaveManager.Instance.state.carsUnlocked[6] = true;
					SaveManager.Instance.Save();
					this.unlockManager.unlocks.Add(new UnlockManager.Unlock(UnlockManager.UnlockType.Car, 6, 0));
				}
				num3++;
			}
		}
		if (!SaveManager.Instance.state.carsUnlocked[7])
		{
			int num4 = 0;
			while (num4 < MapManager.Instance.maps.Length && MapManager.Instance.GetStars(num4, SaveManager.Instance.state.times[num4]) >= 3)
			{
				if (num4 == MapManager.Instance.maps.Length - 1)
				{
					SaveManager.Instance.state.carsUnlocked[7] = true;
					SaveManager.Instance.Save();
					this.unlockManager.unlocks.Add(new UnlockManager.Unlock(UnlockManager.UnlockType.Car, 7, 0));
				}
				num4++;
			}
		}
		SaveManager.Instance.Save();
		MonoBehaviour.print("1");
		if (!SaveManager.Instance.state.skins[5][1])
		{
			MonoBehaviour.print("2");
			for (int i = 0; i < MapManager.Instance.maps.Length; i++)
			{
				float num5 = SaveManager.Instance.state.times[i];
				float num6 = SaveManager.Instance.state.daniTimes[i];
				MonoBehaviour.print("i: " + i);
				if (num5 > num6)
				{
					break;
				}
				if (i == MapManager.Instance.maps.Length - 1)
				{
					SaveManager.Instance.state.skins[5][1] = true;
					SaveManager.Instance.Save();
					this.unlockManager.unlocks.Add(new UnlockManager.Unlock(UnlockManager.UnlockType.Skin, 5, 1));
				}
			}
		}
	}

	// Token: 0x06000084 RID: 132 RVA: 0x000051C9 File Offset: 0x000033C9
	public void Restart()
	{
		GameController.Instance.RestartGame();
		Time.timeScale = 1f;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x000051DF File Offset: 0x000033DF
	public void Menu()
	{
		SceneManager.LoadScene("Menu");
		Time.timeScale = 1f;
	}

	// Token: 0x040000B3 RID: 179
	public TextMeshProUGUI timer;

	// Token: 0x040000B4 RID: 180
	public TextMeshProUGUI pbTimer;

	// Token: 0x040000B5 RID: 181
	public TextMeshProUGUI mapName;

	// Token: 0x040000B6 RID: 182
	public TextMeshProUGUI newBest;

	// Token: 0x040000B7 RID: 183
	public TextMeshProUGUI victoryText;

	// Token: 0x040000B8 RID: 184
	public GameObject timePanel;

	// Token: 0x040000B9 RID: 185
	public GameObject racePanel;

	// Token: 0x040000BA RID: 186
	public ProgressController progressTime;

	// Token: 0x040000BB RID: 187
	public ProgressController progressRace;

	// Token: 0x040000BC RID: 188
	public UnlockManager unlockManager;

	// Token: 0x040000BD RID: 189
	public Image[] stars;

	// Token: 0x040000BE RID: 190
	public static FinishController Instance;
}
