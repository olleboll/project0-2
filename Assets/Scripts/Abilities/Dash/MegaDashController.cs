using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaDashController : MonoBehaviour
{
	public float speed = 0f;
	public bool isDashing = false;
	public GameObject dashGustPrefab;
	private GameObject dashGust;

	private Vector3 targetDestination;

	private Vector2 direction;
	private Vector3 newPosition;
	private Rigidbody2D body;

	private PlayerData playerData;

	void Start(){
		this.body = GetComponent<Rigidbody2D>();
		this.playerData = Object.FindObjectOfType<PlayerData>();
	}

	void FixedUpdate(){
		if (!this.isDashing) {
			return;
		}
		Vector2 nextStep = this.direction * this.speed * Time.fixedDeltaTime;

		Vector2 newPosition = this.body.position + nextStep;

		this.body.MovePosition(newPosition);
		this.playerData.SetLastPlayerMovement(nextStep);
		if (this.dashGust != null) {
			this.dashGust.transform.position = newPosition;
		}
	}

	public void dash(Vector3 dir){
		if (this.isDashing || dir.magnitude < 1) {
			return;
		}
		dir.Normalize();

		this.direction = new Vector2(dir.x, dir.y);

		float angle = ( Mathf.Atan2( dir.y,  dir.x) - Mathf.PI/2 ) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		this.dashGust = Instantiate(this.dashGustPrefab, transform.position, rotation);

		this.isDashing = true;

		this.playerData.setMegaDashDirection(this.direction);
	}

	public void stopDash() {
		this.isDashing = false;
		this.direction = new Vector3(0,0,0);
		Destroy(this.dashGust);
		this.playerData.setMegaDashDirection(this.direction);
	}

	void OnCollisionEnter2D(Collision2D col){
		this.stopDash();
	}
}
