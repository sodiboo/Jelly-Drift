using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class CollisionSfx : MonoBehaviour
{
	// Token: 0x0600006D RID: 109 RVA: 0x000044EC File Offset: 0x000026EC
	private void OnCollisionEnter(Collision other)
	{
		if (!this.crashAudio || !this.ready)
		{
			return;
		}
		if (other.relativeVelocity.magnitude < 6f)
		{
			return;
		}
		if (other.contacts.Length != 0)
		{
			Quaternion rotation = Quaternion.LookRotation(other.contacts[0].normal);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.crashParticles, other.contacts[0].point, rotation);
			Renderer component = other.gameObject.GetComponent<Renderer>();
			if (component)
			{
				Material material = component.materials[0];
				gameObject.GetComponent<ParticleSystem>().GetComponent<Renderer>().material = material;
			}
		}
		this.crashAudio.Randomize();
		this.ready = false;
		base.Invoke("GetReady", 0.5f);
	}

	// Token: 0x0600006E RID: 110 RVA: 0x000045BA File Offset: 0x000027BA
	private void GetReady()
	{
		this.ready = true;
	}

	// Token: 0x040000A5 RID: 165
	public RandomSfx crashAudio;

	// Token: 0x040000A6 RID: 166
	private bool ready = true;
}
