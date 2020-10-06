using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
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
	private UnityEngine.Tilemaps.Tilemap groundTilemap;
	private Rigidbody2D body;
	private PlayerInputController input;
	private PlayerData playerData;
	private PlayerState playerState;

	private MoveTowardsTarget moveTowardsTarget;

	void Start()
	{
		this.body = GetComponent<Rigidbody2D>();
		// Can be used to figure out if we are on "bad" ground after dash.
		// Or we do it in the player movenent script
		this.groundTilemap = GameObject.Find("ground").GetComponent<UnityEngine.Tilemaps.Tilemap>();
		this.playerData = Object.FindObjectOfType<PlayerData>();
		this.playerState = Object.FindObjectOfType<PlayerState>();
		this.input = Object.FindObjectOfType<PlayerInputController>();
	}

	void Update(){
		if (this.input.dash && !this.onCooldown) {
			this.playerState.setState(PlayerState.State.dashing);
		}
	}

	void FixedUpdate()
	{
		if (this.playerState.state != PlayerState.State.dashing) {
			return;
		}

		if (!this.isDashing) {
			this.startDash(this.input.moveDirection);
		}

		Vector3 newPosition = this.moveTowardsTarget.nextPosition(Time.fixedDeltaTime);
		if (this.moveTowardsTarget.reachedTarget) {
			this.isDashing = false;
			this.playerState.resetState(PlayerState.State.dashing);
		}

		this.body.MovePosition(newPosition);
		if (this.dashGust != null) {
			this.dashGust.transform.position = newPosition;
		}
	}

	private void startDash(Vector3 dir){
		if (this.isDashing || this.onCooldown) {
			this.playerState.resetState(PlayerState.State.dashing);
			return;
		}
		dir.Normalize();
		this.direction = new Vector2(dir.x, dir.y);
		this.targetDestination = transform.position + dir * this.distance;
		this.distanceTraveled = 0f;

		float angle = ( Mathf.Atan2( dir.y,  dir.x) - Mathf.PI/2 ) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		this.dashGust = Instantiate(this.dashGustPrefab, transform.position, rotation);

		this.moveTowardsTarget = new MoveTowardsTarget(this.body.position, dir, this.distance, this.speed);

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
