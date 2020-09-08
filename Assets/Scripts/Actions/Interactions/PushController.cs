using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
	Rigidbody2D body;

	void Start(){
		this.body = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate(){
		if (this.body.velocity.x < this.body.velocity.y) {
			this.body.velocity = new Vector3(0, this.body.velocity.y);
		} else {
			this.body.velocity = new Vector3(this.body.velocity.x, 0);
		}
	}

	void OnCollisionExit2D(Collision2D other){
		this.body.velocity = Vector3.zero;
	}

}
