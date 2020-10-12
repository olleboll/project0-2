using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed;

	private Rigidbody2D body;
	private Vector3 moveDirection;
	private Vector3 lastMoveDirection;
	private PlayerData playerData;

	private PlayerState playerState;
	private PlayerInputController input;
	private AudioSource walkingSound;
	private PlayerAnimationHandler anim;
	private UnityEngine.Tilemaps.Tilemap groundTilemap;

	private Vector2 maxTilemapBounds;
	private Vector2 minTilemapBounds;

	void Start()
	{
		this.input = Object.FindObjectOfType<PlayerInputController>();
		this.body = GetComponent<Rigidbody2D>();
		this.anim = GetComponent<PlayerAnimationHandler>();
		this.walkingSound = GetComponent<AudioSource>();
		this.playerData = Object.FindObjectOfType<PlayerData>();
		this.playerState = Object.FindObjectOfType<PlayerState>();
		this.groundTilemap = GameObject.Find("ground").GetComponent<UnityEngine.Tilemaps.Tilemap>();

		this.maxTilemapBounds = this.groundTilemap.localBounds.max;
		this.minTilemapBounds = this.groundTilemap.localBounds.min;
	}

	void Update()
	{
		this.playerState.setState(PlayerState.State.moving);
		this.moveDirection = this.input.moveDirection;
		float currentSpeed = moveDirection.magnitude;

		if (this.input.aimThrow) {
			currentSpeed = 0f;
			this.moveDirection = Vector3.zero;
		}

		Vector3 currentDirection;

		if (currentSpeed > 0) {
			currentDirection = moveDirection;
			this.lastMoveDirection = moveDirection;

			if (!walkingSound.isPlaying) {
				walkingSound.Play();
			}
		} else {
			currentDirection = lastMoveDirection;
			walkingSound.Stop();
		}
		this.anim.updateAnimation(currentDirection, currentSpeed);
	}

	void FixedUpdate(){

		if (this.playerState.state != PlayerState.State.moving) {
			//this.anim.SetFloat("speed", 0);
			walkingSound.Stop();
			return;
		}

		this.move();
	}

	public void move(){
		if (!(this.input.moveDirection.magnitude > 0)) {
			walkingSound.Stop();
			this.playerState.resetState(PlayerState.State.moving);
			return;
		}

		float newXMovement = this.moveDirection.x * speed * Time.fixedDeltaTime;
		float newYMovement = this.moveDirection.y * speed * Time.fixedDeltaTime;
		float newX = this.body.position.x + newXMovement;
		float newY = this.body.position.y + newYMovement;

		var grid = this.groundTilemap.layoutGrid;
		var tilePositionX = grid.WorldToCell(new Vector3(newX, this.body.position.y, 0));
		var tileX = this.groundTilemap.GetTile(tilePositionX);

		var tilePositionY = grid.WorldToCell(new Vector3(this.body.position.x, newY, 0));
		var tileY = this.groundTilemap.GetTile(tilePositionY);
		// Make an outOfBounds function in statis helper functions
		if (tileX == null ||
		    StaticHelperFunctions.isNonGroundTile(tileX.name) ||
		    newX < (this.minTilemapBounds.x + 1.2) ||
		    newX > (this.maxTilemapBounds.x - 2)) {
			newX = this.body.position.x;
			newXMovement = 0;
		}

		if (tileY == null || StaticHelperFunctions.isNonGroundTile(tileY.name) ||
		    newY < (this.minTilemapBounds.y + 1) ||
		    newY > (this.maxTilemapBounds.y - 3.5)) {
			newY = this.body.position.y;
			newYMovement = 0;
		}

		Vector3 newPosition = new Vector3(newX, newY, 0);
		this.body.MovePosition(newPosition);

		this.playerData.SetLastPlayerMovement(new Vector3(newXMovement, newYMovement));
	}

	private bool shouldUpdate(){
		if (this.playerState.state == PlayerState.State.moving) {
			return true;
		}
		if (this.input.moveDirection.magnitude > 0) {
			return this.playerState.setState(PlayerState.State.moving);
		}
		return false;
	}
}
