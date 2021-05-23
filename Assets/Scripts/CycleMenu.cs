using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000015 RID: 21
public class CycleMenu : MonoBehaviour
{
	// Token: 0x06000070 RID: 112 RVA: 0x000045D4 File Offset: 0x000027D4
	private void Awake()
	{
		this.selected = this.startSelect;
		this.cycles = new List<ItemCycle>();
		this.cycleText = new List<TextMeshProUGUI>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			ItemCycle component = base.transform.GetChild(i).GetComponent<ItemCycle>();
			if (component)
			{
				this.cycles.Add(component);
				TextMeshProUGUI componentInChildren = component.GetComponentInChildren<TextMeshProUGUI>();
				this.cycleText.Add(componentInChildren);
				componentInChildren.color = Color.white;
			}
		}
		this.cycleText[this.selected].color = Color.black;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00004678 File Offset: 0x00002878
	private void Start()
	{
		SaveManager.Instance.state.skins[5][1] = true;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00004690 File Offset: 0x00002890
	private void UpdateSelected()
	{
		foreach (TextMeshProUGUI textMeshProUGUI in this.cycleText)
		{
			textMeshProUGUI.color = Color.white;
			if (this.correspondingText.Length != 0 && !this.correspondingText[this.selected].gameObject.CompareTag("Ignore"))
			{
				this.correspondingText[this.selected].color = Color.white;
			}
		}
		this.cycleText[this.selected].color = Color.black;
		if (this.correspondingText.Length != 0 && !this.correspondingText[this.selected].gameObject.CompareTag("Ignore"))
		{
			this.correspondingText[this.selected].color = Color.black;
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0000477C File Offset: 0x0000297C
	private void OnEnable()
	{
		this.selected = this.startSelect;
		this.UpdateSelected();
		InputManager.Instance.menu += Move;
		InputManager.Instance.submit += Submit;
		InputManager.Instance.cancel += Cancel;
	}

    private void OnDisable()
	{
		InputManager.Instance.menu -= Move;
		InputManager.Instance.submit -= Submit;
		InputManager.Instance.cancel -= Cancel;
	}

	bool verticalDone;
	bool horizontalDone;

    public void Move(Vector2 dir)
	{
		if (UnlockManager.Instance && UnlockManager.Instance.gameObject.activeInHierarchy) return;
		if (!isActiveAndEnabled) return;
		var x = Math.Sign(dir.x);
		var y = Math.Sign(-dir.y);
		if (x != 0 && !horizontalDone)
        {
			if (cycles[selected].activeCycle)
            {
				cycles[selected].Cycle(x);
				SoundManager.Instance.PlayCycle();
            }
			else
            {
				SoundManager.Instance.PlayError();
            }
        }
		if (y != 0 && !verticalDone)
        {
			cycleText[selected].color = Color.white;
			if (correspondingText.Length != 0 && !correspondingText[selected].gameObject.CompareTag("Ignore"))
            {
				correspondingText[selected].color = Color.white;
            }
			selected += y + cycles.Count;
			selected %= cycles.Count;
			cycleText[selected].color = Color.black;
			if (correspondingText.Length != 0 && !correspondingText[selected].gameObject.CompareTag("Ignore"))
            {
				correspondingText[selected].color = Color.black;
            }
			SoundManager.Instance.PlayMenuNavigate();
        }

		horizontalDone = x != 0;
		verticalDone = y != 0;
	}

	public void Cancel(bool cancel)
    {
		if (UnlockManager.Instance && UnlockManager.Instance.gameObject.activeInHierarchy) return;
		if (!isActiveAndEnabled) return;
		if (cancel)
        {
			cycles[backBtn].Cycle(1);
			SoundManager.Instance.PlayCycle();
        }
    }

	public void Submit(bool submit)
	{
		if (UnlockManager.Instance && UnlockManager.Instance.gameObject.activeInHierarchy) return;
		if (!isActiveAndEnabled) return;
		if (submit)
		{
			if (cycles[selected].activeCycle)
			{
				cycles[selected].Cycle(1);
				SoundManager.Instance.PlayCycle();
			}
			else
			{
				SoundManager.Instance.PlayError();
			}
		}
	}

    // Token: 0x040000A7 RID: 167
    private List<ItemCycle> cycles;

	// Token: 0x040000A8 RID: 168
	private List<TextMeshProUGUI> cycleText;

	// Token: 0x040000A9 RID: 169
	public TextMeshProUGUI[] correspondingText;

	// Token: 0x040000AA RID: 170
	private int selected;

	// Token: 0x040000AB RID: 171
	public int startSelect;

	// Token: 0x040000AC RID: 172
	public int backBtn;
}
