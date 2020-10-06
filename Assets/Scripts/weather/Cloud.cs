using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
	public float speed = 3f;

	void Update()
	{
		this.transform.position += new Vector3(this.speed * Time.deltaTime, 0,0);
		if (this.transform.position.x > 40) {
			this.transform.position = new Vector3( -40, this.transform.position.y, 0);
		}
		if (this.transform.position.x < -40) {
			this.transform.position = new Vector3(40, this.transform.position.y, 0);
		}
	}
}
