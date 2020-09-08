using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAnimalController : MonoBehaviour
{
	public int hp = 15;
	public float speed = 4f;
	public float radius = 10f;
	public float moveCooldown = 10f;

	private Vector3 startPosition;
	private Vector3 targetPosition;
	private bool isMoving = false;
	private bool canMove = true;

	private UnityEngine.Tilemaps.Tilemap groundTilemap;

	private Animator anim;

	void Start()
	{
		this.startPosition = transform.position;
		this.anim = GetComponent<Animator>();
		this.groundTilemap = GameObject.Find("ground").GetComponent<UnityEngine.Tilemaps.Tilemap>();
	}

	void Update()
	{
		if (this.isMoving) {

			Vector3 dir = this.targetPosition - transform.position;
			dir.Normalize();

			float newXMovement = dir.x * this.speed * Time.deltaTime;
			float newYMovement = dir.y * this.speed * Time.deltaTime;
			float newX = transform.position.x + newXMovement;
			float newY = transform.position.y + newYMovement;

			var grid = this.groundTilemap.layoutGrid;
			var tilePositionX = grid.WorldToCell(new Vector3(newX, transform.position.y, 0));
			var tileX = this.groundTilemap.GetTile(tilePositionX);

			var tilePositionY = grid.WorldToCell(new Vector3(transform.position.x, newY, 0));
			var tileY = this.groundTilemap.GetTile(tilePositionY);

			if (StaticHelperFunctions.isNonGroundTile(tileX.name)) {
				newX = transform.position.x;
				newXMovement = 0;
				this.targetPosition.x = newX;
			}

			if (StaticHelperFunctions.isNonGroundTile(tileY.name)) {
				newY = transform.position.y;
				newYMovement = 0;
				this.targetPosition.y = newY;
			}

			transform.position = new Vector3(newX, newY, 0);

			this.anim.SetFloat("dirX", dir.x);
			this.anim.SetFloat("dirY", dir.y);
			this.anim.SetFloat("speed", this.speed);

			float distanceToTarget = Vector3.Distance(transform.position, this.targetPosition);

			if (distanceToTarget < 1) {
				this.isMoving = false;
				this.canMove = false;
				StartCoroutine(offCooldown());
				this.anim.SetFloat("speed", 0);
			}

		} else if (this.canMove) {

			// set new move
			float newX = Random.Range(this.startPosition.x - radius, this.startPosition.x + radius);
			float newY = Random.Range(this.startPosition.y - radius, this.startPosition.y + radius);

			this.targetPosition = new Vector3(newX, newY, 0);
			this.isMoving = true;
		}
	}

	IEnumerator offCooldown(){
		yield return new WaitForSeconds(moveCooldown);
		this.canMove = true;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, this.radius);
	}
}
