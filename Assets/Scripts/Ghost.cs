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
            for (var j = 0; j < newMats.Length; j++)
            {
                materials[i][j] = renderers[i].materials[j];
                var material = new Material(ghost);
                var color = materials[i][j].color;
                material.color = new Color(color.r, color.g, color.b, 0.2f);
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
