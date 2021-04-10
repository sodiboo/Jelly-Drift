using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class SaveManager : MonoBehaviour
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x0600016D RID: 365 RVA: 0x00007CB4 File Offset: 0x00005EB4
	// (set) Token: 0x0600016E RID: 366 RVA: 0x00007CBB File Offset: 0x00005EBB
	public static SaveManager Instance { get; set; }

	// Token: 0x0600016F RID: 367 RVA: 0x00007CC3 File Offset: 0x00005EC3
	private void Awake()
	{
		if (SaveManager.Instance != null && SaveManager.Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			SaveManager.Instance = this;
		}
		this.Load();
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00007CF8 File Offset: 0x00005EF8
	public void Save()
	{
		PlayerPrefs.SetString("save", this.Serialize<PlayerSave>(this.state));
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00007D10 File Offset: 0x00005F10
	public void Load()
	{
		if (PlayerPrefs.HasKey("save"))
		{
			this.state = this.Deserialize<PlayerSave>(PlayerPrefs.GetString("save"));
			return;
		}
		this.NewSave();
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00007D3B File Offset: 0x00005F3B
	public void NewSave()
	{
		this.state = new PlayerSave();
		this.Save();
		MonoBehaviour.print("Creating new save file");
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00007D58 File Offset: 0x00005F58
	public string Serialize<T>(T toSerialize)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		StringWriter stringWriter = new StringWriter();
		xmlSerializer.Serialize(stringWriter, toSerialize);
		return stringWriter.ToString();
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00007D8C File Offset: 0x00005F8C
	public T Deserialize<T>(string toDeserialize)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		StringReader textReader = new StringReader(toDeserialize);
		return (T)((object)xmlSerializer.Deserialize(textReader));
	}

	// Token: 0x04000177 RID: 375
	public PlayerSave state;
}
