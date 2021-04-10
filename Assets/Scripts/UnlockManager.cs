using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class UnlockManager : MonoBehaviour
{
	// Token: 0x060001CF RID: 463 RVA: 0x00009944 File Offset: 0x00007B44
	private void Start()
	{
		UnlockManager.Instance = this;
		this.defaultSize = base.transform.localScale;
		this.desiredSize = Vector3.zero;
		base.transform.localScale = Vector3.zero;
		this.carDisplay = UnityEngine.Object.Instantiate<GameObject>(this.unlockDisplayPrefab).GetComponentInChildren<CarDisplay>();
		this.maps = this.carDisplay.transform.parent.GetChild(2);
		this.NextDisplay();
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x000099BC File Offset: 0x00007BBC
	public void NextDisplay()
	{
		this.readyToSkip = false;
		base.Invoke("SetSkipReady", 0.4f);
		if (this.n >= this.unlocks.Count)
		{
			base.gameObject.SetActive(false);
			return;
		}
		List<UnlockManager.Unlock> list = this.unlocks;
		int num = this.n;
		this.n = num + 1;
		this.DisplayUnlock(list[num]);
		SoundManager.Instance.PlayUnlock();
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00009A2C File Offset: 0x00007C2C
	public void DisplayUnlock(UnlockManager.Unlock u)
	{
		for (int i = 0; i < this.maps.childCount; i++)
		{
			this.maps.GetChild(i).gameObject.SetActive(false);
		}
		base.transform.localScale = Vector3.zero;
		this.desiredSize = this.defaultSize;
		string str = "";
		if (u.type == UnlockManager.UnlockType.Car)
		{
			SaveManager.Instance.state.carsUnlocked[u.index] = true;
			SaveManager.Instance.Save();
			str = "\"" + PrefabManager.Instance.cars[u.index].name + "\"";
			this.carDisplay.SpawnCar(u.index);
			this.carDisplay.SetSkin(SaveManager.Instance.state.skins[u.index].Length - 1);
		}
		else if (u.type == UnlockManager.UnlockType.Skin)
		{
			SaveManager.Instance.state.skins[u.index][u.subIndex] = true;
			SaveManager.Instance.Save();
			this.carDisplay.SpawnCar(u.index);
			this.carDisplay.SetSkin(u.subIndex);
			str = PrefabManager.Instance.cars[u.index].name + " \"" + this.carDisplay.currentCar.GetComponent<CarSkin>().GetSkinName(u.subIndex) + "\"";
		}
		else if (u.type == UnlockManager.UnlockType.Map)
		{
			SaveManager.Instance.state.mapsUnlocked[u.index] = true;
			SaveManager.Instance.Save();
			this.maps.GetChild(u.index).gameObject.SetActive(true);
			str = "\"" + MapManager.Instance.maps[u.index].name + "\"";
			this.carDisplay.Hide();
		}
		this.text.text = "<size=100%>unlocked:\n<b><size=80%>" + str;
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00009C3C File Offset: 0x00007E3C
	private void Update()
	{
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.desiredSize, Time.unscaledDeltaTime * 3f);
		if (Input.anyKey && this.readyToSkip)
		{
			this.NextDisplay();
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00009C8A File Offset: 0x00007E8A
	private void SetSkipReady()
	{
		this.readyToSkip = true;
	}

	// Token: 0x040001F2 RID: 498
	public TextMeshProUGUI text;

	// Token: 0x040001F3 RID: 499
	private Vector3 defaultSize;

	// Token: 0x040001F4 RID: 500
	private Vector3 desiredSize;

	// Token: 0x040001F5 RID: 501
	private Transform maps;

	// Token: 0x040001F6 RID: 502
	private CarDisplay carDisplay;

	// Token: 0x040001F7 RID: 503
	public GameObject unlockDisplayPrefab;

	// Token: 0x040001F8 RID: 504
	public List<UnlockManager.Unlock> unlocks = new List<UnlockManager.Unlock>();

	// Token: 0x040001F9 RID: 505
	public static UnlockManager Instance;

	// Token: 0x040001FA RID: 506
	private int n;

	// Token: 0x040001FB RID: 507
	private bool readyToSkip;

	// Token: 0x02000066 RID: 102
	public enum UnlockType
	{
		// Token: 0x04000252 RID: 594
		Car,
		// Token: 0x04000253 RID: 595
		Skin,
		// Token: 0x04000254 RID: 596
		Map
	}

	// Token: 0x02000067 RID: 103
	public class Unlock
	{
		// Token: 0x0600022D RID: 557 RVA: 0x0000A852 File Offset: 0x00008A52
		public Unlock(UnlockManager.UnlockType t, int index, int subIndex)
		{
			this.type = t;
			this.index = index;
			this.subIndex = subIndex;
		}

		// Token: 0x04000255 RID: 597
		public UnlockManager.UnlockType type;

		// Token: 0x04000256 RID: 598
		public int index;

		// Token: 0x04000257 RID: 599
		public int subIndex;
	}
}
