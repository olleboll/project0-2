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

	private Vector3 direction;
	private UnityEngine.Tilemaps.Tilemap map;

	void Start(){
		this.map = GameObject.Find("Tilemap").GetComponent<UnityEngine.Tilemaps.Tilemap>();
	}

	void Update()
	{
		if (!this.isDashing) {
			return;
		}

		Vector3 newPosition;
		Vector3 nextStep = this.direction * this.speed * Time.deltaTime;

		if (this.distanceTraveled + nextStep.magnitude >= this.distance) {
			this.isDashing = false;
			newPosition = transform.position;
		} else {
			newPosition = transform.position + nextStep;
			this.distanceTraveled += nextStep.magnitude;
		}

		var grid = map.layoutGrid;
		var tilePosition = grid.WorldToCell(newPosition);
		var tile = map.GetTile(tilePosition);
		if (tile == null) {
			this.isDashing = false;
			return;
		} else if (!this.isDashing && (tile.name == "Water" || tile.name == "AbyssGrass")) {
			GetComponent<HealthPoints>().takeDamage(10000);
			return;
		}


		transform.position = newPosition;

		if (this.dashGust != null) {
			this.dashGust.transform.position = newPosition;
		}
	}

	public void dash(Vector3 dir){
		if (this.isDashing || this.onCooldown || dir.magnitude < 1) {
			return;
		}

		this.direction = dir;
		this.targetDestination = transform.position + dir * this.distance;
		this.distanceTraveled = 0f;

		float angle = ( Mathf.Atan2( dir.y,  dir.x) - Mathf.PI/2 ) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		this.dashGust = Instantiate(this.dashGustPrefab, transform.position, rotation);

		this.isDashing = true;
		this.onCooldown = true;
		StartCoroutine(offCooldown());
	}

	void OnCollisionEnter2D(Collision2D col){
		Debug.Log("COLLISION");
		this.isDashing = false;
	}

	IEnumerator offCooldown(){
		yield return new WaitForSeconds(this.cooldown);
		this.onCooldown = false;
	}
}
