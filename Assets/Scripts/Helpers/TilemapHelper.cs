using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TilemapHelper : MonoBehaviour
{
	public float width = 50;
	public float height = 50;

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube( new Vector3(0, 0, 1), new Vector3(this.width, this.height, 1));
	}

}
