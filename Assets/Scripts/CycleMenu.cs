using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000015 RID: 21
public class CycleMenu : MonoBehaviour
{
	public InputActionAsset inputs;
	InputActionMap actions;
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
		inputs = inputs ?? PrefabManager.Instance.inputs;
		actions = inputs.FindActionMap("Menu");
		move = actions.FindAction("Move", true);
		submit = actions.FindAction("Submit", true);
		cancel = actions.FindAction("Cancel", true);
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

	InputAction move;
	InputAction submit;
	InputAction cancel;

	// Token: 0x06000073 RID: 115 RVA: 0x0000477C File Offset: 0x0000297C
	private void OnEnable()
	{
		this.selected = this.startSelect;
		this.horizontalDone = false;
		this.verticalDone = false;
		this.UpdateSelected();
		move.performed += Move;
		submit.performed += Submit;
		cancel.performed += Cancel;
	}

    private void OnDisable()
    {
		move.performed -= Move;
		submit.performed -= Submit;
		cancel.performed -= Cancel;
	}

    public void Move(InputAction.CallbackContext context) {
		if (UnlockManager.Instance && UnlockManager.Instance.gameObject.activeInHierarchy) return;
		if (!isActiveAndEnabled) return;
		var dir = context.ReadValue<Vector2>();
		var x = Math.Sign(dir.x);
		var y = Math.Sign(-dir.y);
		if (x != 0)
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
		if (y != 0)
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
	}

	public void Cancel(InputAction.CallbackContext context)
    {
		if (UnlockManager.Instance && UnlockManager.Instance.gameObject.activeInHierarchy) return;
		if (!isActiveAndEnabled) return;
		var cancel = context.ReadValueAsButton();
		if (cancel)
        {
			cycles[backBtn].Cycle(1);
			SoundManager.Instance.PlayCycle();
        }
    }

	public void Submit(InputAction.CallbackContext context)
    {
		if (UnlockManager.Instance && UnlockManager.Instance.gameObject.activeInHierarchy) return;
		if (!isActiveAndEnabled) return;
		var submit = context.ReadValueAsButton();
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

    //// Token: 0x06000075 RID: 117 RVA: 0x000047A8 File Offset: 0x000029A8
    //private void PlayerInput()
    //{

    //    int num = (int)Input.GetAxisRaw("HorizontalMenu");
    //    int num2 = (int)(-(int)Input.GetAxisRaw("VerticalMenu"));
    //    bool buttonDown = Input.GetButtonDown("Submit");
    //    bool buttonDown2 = Input.GetButtonDown("Cancel");
    //    if ((num != 0 && !this.horizontalDone) || buttonDown)
    //    {
    //        if (this.cycles[this.selected].activeCycle)
    //        {
    //            this.cycles[this.selected].Cycle(num);
    //            SoundManager.Instance.PlayCycle();
    //        }
    //        else
    //        {
    //            SoundManager.Instance.PlayError();
    //        }
    //    }
    //    if (num2 != 0 && !this.verticalDone)
    //    {
    //        this.cycleText[this.selected].color = Color.white;
    //        if (this.correspondingText.Length != 0 && !this.correspondingText[this.selected].gameObject.CompareTag("Ignore"))
    //        {
    //            this.correspondingText[this.selected].color = Color.white;
    //        }
    //        this.selected += num2;
    //        if (this.selected >= this.cycles.Count)
    //        {
    //            this.selected = 0;
    //        }
    //        else if (this.selected < 0)
    //        {
    //            this.selected = this.cycles.Count - 1;
    //        }
    //        this.cycleText[this.selected].color = Color.black;
    //        if (this.correspondingText.Length != 0 && !this.correspondingText[this.selected].gameObject.CompareTag("Ignore"))
    //        {
    //            this.correspondingText[this.selected].color = Color.black;
    //        }
    //        SoundManager.Instance.PlayMenuNavigate();
    //    }
    //    if (buttonDown2)
    //    {
    //        this.cycles[this.backBtn].Cycle(1);
    //        SoundManager.Instance.PlayCycle();
    //    }
    //    this.horizontalDone = (num != 0);
    //    this.verticalDone = (num2 != 0);
    //}

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

	// Token: 0x040000AD RID: 173
	private bool horizontalDone;

	// Token: 0x040000AE RID: 174
	private bool verticalDone;
}
