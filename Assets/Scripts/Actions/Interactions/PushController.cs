using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{

	private Rigidbody2D body;
	private PlayerInputController input;
	private PlayerMovementController player;
	private bool canBePushed = false;
	private bool isBeingPushed = false;
	void Start(){
		this.body = GetComponent<Rigidbody2D>();
		this.player = GameObject.Find("Player").GetComponent<PlayerMovementController>();
		this.input = Object.FindObjectOfType<PlayerInputController>();
	}

	void Update(){
		if (this.canBePushed && this.input.action) {
			this.isBeingPushed = true;
		} else {
			this.isBeingPushed = false;
		}
	}

	void FixedUpdate(){
		if (this.isBeingPushed) {
			Vector3 rawDir = this.input.moveDirection;

			Vector3 dir = Vector3.zero;

			if (Mathf.Abs(rawDir.x) > Mathf.Abs(rawDir.y)) {
				dir.x = rawDir.x;
			} else {
				dir.y = rawDir.y;
			}

			Vector3 newPosition = new Vector3(this.body.position.x, this.body.position.y, 0) + dir * this.player.getSpeed() * Time.fixedDeltaTime;
			this.body.MovePosition(newPosition);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("YEAYEYA");
		Debug.Log(other);
		if (other.gameObject.name == "Player") {
			this.canBePushed = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.name == "Player") {
			this.canBePushed = false;
		}
	}

}
