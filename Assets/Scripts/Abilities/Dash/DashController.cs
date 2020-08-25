using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
	public float speed = 0f;
	public float distance = 0f;
	public float cooldown = 1f;
	public bool isDashing = false;
	public GameObject dashGustPrefab;


	private GameObject dashGust;
	private Vector3 targetDestination;
	private float distanceTraveled = 0f;
	private bool onCooldown = false;

	private Vector2 direction;
	private Vector3 newPosition;
	private UnityEngine.Tilemaps.Tilemap dashableTilemap;
	private UnityEngine.Tilemaps.Tilemap wallsTilemap;
	private Rigidbody2D body;

	private PlayerData playerData;

	void Start(){
		this.body = GetComponent<Rigidbody2D>();
		this.dashableTilemap = GameObject.Find("collision_jumpable").GetComponent<UnityEngine.Tilemaps.Tilemap>();
		this.wallsTilemap = GameObject.Find("collision_walls").GetComponent<UnityEngine.Tilemaps.Tilemap>();
		this.playerData = Object.FindObjectOfType<PlayerData>();
	}


	void FixedUpdate(){
		if (!this.isDashing) {
			return;
		}
		Vector2 nextStep = this.direction * this.speed * Time.fixedDeltaTime;

		Vector2 newPosition = this.body.position;

		if (this.distanceTraveled + nextStep.magnitude >= this.distance) {
			this.isDashing = false;
		} else {
			newPosition = this.body.position + nextStep;
			this.distanceTraveled += nextStep.magnitude;
			this.playerData.SetLastPlayerMovement(nextStep);
		}

		this.body.MovePosition(newPosition);
		if (this.dashGust != null) {
			this.dashGust.transform.position = newPosition;
		}
	}

	private void stopDash(){
		this.isDashing = false;
	}

	public void dash(Vector3 dir){
		if (this.isDashing || this.onCooldown) {
			return;
		}
		dir.Normalize();
		this.direction = new Vector2(dir.x, dir.y);
		this.targetDestination = transform.position + dir * this.distance;
		this.distanceTraveled = 0f;

		float angle = ( Mathf.Atan2( dir.y,  dir.x) - Mathf.PI/2 ) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		this.dashGust = Instantiate(this.dashGustPrefab, transform.position, rotation);

		this.isDashing = true;
		this.onCooldown = true;
		StartCoroutine(offCooldown());
	}

	IEnumerator offCooldown(){
		yield return new WaitForSeconds(this.cooldown);
		this.onCooldown = false;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, this.distance);
	}
}
