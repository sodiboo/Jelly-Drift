using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class SettingsUi : MonoBehaviour
{
	// Token: 0x0600018D RID: 397 RVA: 0x000088F8 File Offset: 0x00006AF8
	private void Start()
	{
		this.LoadAllSettings();
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00008900 File Offset: 0x00006B00
	private void LoadAllSettings()
	{
		this.LoadSetting(this.motionBlur, SaveState.Instance.motionBlur);
		this.LoadSetting(this.dof, SaveState.Instance.dof);
		this.LoadSetting(this.graphics, SaveState.Instance.graphics);
		this.LoadSetting(this.quality, SaveState.Instance.quality);
		this.LoadSetting(this.camMode, SaveState.Instance.cameraMode);
		this.LoadSetting(this.camShake, SaveState.Instance.cameraShake);
		this.LoadSettingSlider(this.volume, SaveState.Instance.volume);
		this.LoadSettingSlider(this.music, SaveState.Instance.music);
		this.LoadSetting(this.speedometer, SaveState.Instance.speedometer);
	}

	// Token: 0x0600018F RID: 399 RVA: 0x000089BD File Offset: 0x00006BBD
	private void LoadSetting(SettingCycle s, int n)
	{
		s.selected = n;
		s.UpdateOptions();
	}

	// Token: 0x06000190 RID: 400 RVA: 0x000089CC File Offset: 0x00006BCC
	private void LoadSettingSlider(SliderSettingCycle s, int f)
	{
		s.selected = f;
		s.UpdateOptions();
	}

	// Token: 0x06000191 RID: 401 RVA: 0x000089DC File Offset: 0x00006BDC
	public void UpdateSettings()
	{
		this.MotionBlur(this.motionBlur.selected);
		this.DoF(this.dof.selected);
		this.Graphics(this.graphics.selected);
		this.Quality(this.quality.selected);
		this.CamMode(this.camMode.selected);
		this.CamShake(this.camShake.selected);
		this.Volume();
		this.Music();
		this.Speedometer();
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00008A5B File Offset: 0x00006C5B
	public void MotionBlur(int n)
	{
		SaveManager.Instance.state.motionBlur = n;
		SaveManager.Instance.Save();
		SaveState.Instance.motionBlur = n;
		PPController.Instance.LoadSettings();
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00008A8C File Offset: 0x00006C8C
	public void DoF(int n)
	{
		SaveManager.Instance.state.dof = n;
		SaveManager.Instance.Save();
		SaveState.Instance.dof = n;
		PPController.Instance.LoadSettings();
	}

	// Token: 0x06000194 RID: 404 RVA: 0x00008ABD File Offset: 0x00006CBD
	public void Graphics(int n)
	{
		SaveManager.Instance.state.graphics = n;
		SaveManager.Instance.Save();
		SaveState.Instance.graphics = n;
		PPController.Instance.LoadSettings();
	}

	// Token: 0x06000195 RID: 405 RVA: 0x00008AF0 File Offset: 0x00006CF0
	public void Quality(int n)
	{
		SaveManager.Instance.state.quality = n;
		SaveManager.Instance.Save();
		SaveState.Instance.quality = n;
		QualitySettings.SetQualityLevel(n + Mathf.Clamp(2 * n - 1, 0, 10));
		if (CameraCulling.Instance)
		{
			CameraCulling.Instance.UpdateCulling();
		}
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00008B4C File Offset: 0x00006D4C
	public void CamMode(int n)
	{
		SaveManager.Instance.state.cameraMode = n;
		SaveManager.Instance.Save();
		SaveState.Instance.cameraMode = n;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00008B73 File Offset: 0x00006D73
	public void CamShake(int n)
	{
		SaveManager.Instance.state.cameraShake = n;
		SaveManager.Instance.Save();
		SaveState.Instance.cameraShake = n;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00008B9C File Offset: 0x00006D9C
	public void Volume()
	{
		SaveManager.Instance.state.volume = this.volume.selected;
		SaveManager.Instance.Save();
		SaveState.Instance.volume = this.volume.selected;
		AudioListener.volume = (float)this.volume.selected / 10f;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00008BFC File Offset: 0x00006DFC
	public void Music()
	{
		SaveManager.Instance.state.music = this.music.selected;
		SaveManager.Instance.Save();
		SaveState.Instance.music = this.music.selected;
		MusicController.Instance.UpdateMusic((float)this.music.selected);
	}

	public void Speedometer()
    {
		SaveManager.Instance.state.speedometer = this.speedometer.selected;
		SaveManager.Instance.Save();
		SaveState.Instance.speedometer = this.speedometer.selected;
    }


	// Token: 0x0600019A RID: 410 RVA: 0x00008C58 File Offset: 0x00006E58
	public void ResetSave()
	{
		SaveManager.Instance.NewSave();
		SaveManager.Instance.Save();
	}

	// Token: 0x040001A0 RID: 416
	public SettingCycle motionBlur;

	// Token: 0x040001A1 RID: 417
	public SettingCycle graphics;

	// Token: 0x040001A2 RID: 418
	public SettingCycle quality;

	// Token: 0x040001A3 RID: 419
	public SettingCycle camMode;

	// Token: 0x040001A4 RID: 420
	public SettingCycle camShake;

	// Token: 0x040001A5 RID: 421
	public SettingCycle dof;

	// Token: 0x040001A6 RID: 422
	public SliderSettingCycle volume;

	// Token: 0x040001A7 RID: 423
	public SliderSettingCycle music;

	public SettingCycle speedometer;

	// Token: 0x040001A8 RID: 424
	private Color selected = Color.white;

	// Token: 0x040001A9 RID: 425
	private Color deselected = new Color(0f, 0f, 0f, 0.3f);
}
