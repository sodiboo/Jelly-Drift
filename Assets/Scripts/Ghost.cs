using System;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	private Renderer[] renderers;
	private Material[][] materials;
	public Material ghost;

    private void Awake()
    {
		ghost = PrefabManager.Instance.ghostMat;
		renderers = GetComponentsInChildren<MeshRenderer>();
		materials = new Material[renderers.Length][];
    }

    private void OnEnable()
	{
		for (var i = 0; i < renderers.Length; i++)
		{
			materials[i] = new Material[renderers[i].materials.Length];
			var newMats = new Material[renderers[i].materials.Length];
			for (int j = 0; j < newMats.Length; j++)
			{
				materials[i][j] = renderers[i].materials[j];
				Material material = new Material(this.ghost);
				material.color = materials[i][j].color;
				material.color = new Color(material.color.r, material.color.g, material.color.b, 0.2f);
				newMats[j] = material;
			}
			renderers[i].materials = newMats;
		}
	}

    private void OnDisable()
    {
        for (var i = 0; i < renderers.Length; i++)
        {
			renderers[i].materials = materials[i];
        }
    }
}
