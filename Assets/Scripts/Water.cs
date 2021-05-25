using System;
using UnityEngine;

public class Water : MonoBehaviour
{
#if MOBILE
	private void Start()
	{
		GetComponent<MeshRenderer>().material = this.bad;
	}
#endif

	public Material bad;
}
