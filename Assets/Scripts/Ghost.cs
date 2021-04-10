using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class Ghost : MonoBehaviour
{
	// Token: 0x060000A6 RID: 166 RVA: 0x000055C8 File Offset: 0x000037C8
	private void Start()
	{
		this.ghost = PrefabManager.Instance.ghostMat;
		Renderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
		this.renderers = componentsInChildren;
		foreach (Renderer renderer in this.renderers)
		{
			Material[] materials = renderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				Material material = new Material(this.ghost);
				material.color = materials[j].color;
				material.color = new Color(material.color.r, material.color.g, material.color.b, 0.2f);
				materials[j] = material;
			}
			renderer.materials = materials;
		}
	}

	// Token: 0x040000D2 RID: 210
	private Renderer[] renderers;

	// Token: 0x040000D3 RID: 211
	public Material ghost;
}
