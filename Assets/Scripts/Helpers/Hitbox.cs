using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	public Color color;
	public float radius;

	void OnDrawGizmosSelected()
	{
		Gizmos.color = this.color;
		Gizmos.DrawWireSphere(transform.position, this.radius);
	}

}
