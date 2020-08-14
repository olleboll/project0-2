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

	private Vector3 direction;
	private UnityEngine.Tilemaps.Tilemap map;

	private PlayerData playerData;

	void Start(){
		this.map = GameObject.Find("Tilemap").GetComponent<UnityEngine.Tilemaps.Tilemap>();
		this.playerData = Object.FindObjectOfType<PlayerData>();
	}

	void Update()
	{
		if (!this.isDashing) {
			return;
		}

		Vector3 newPosition = transform.position + this.direction * this.speed * Time.deltaTime;

		var grid = map.layoutGrid;
		var tilePosition = grid.WorldToCell(newPosition);
		var tile = map.GetTile(tilePosition);
		if (tile == null) {
			this.stopDash();
			return;
		}


		transform.position = newPosition;

		if (this.dashGust != null) {
			this.dashGust.transform.position = newPosition;
		}
	}

	public void dash(Vector3 dir){
		if (this.isDashing || dir.magnitude < 1) {
			return;
		}

		this.direction = dir;

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
