using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
	public int damage = 0;
	public float range = 0f;
	public float throwingRange = 5f;
	public float throwingSpeed = 5f;


	public GameObject owner;
	public GameObject hitEffect;
	public GameObject soundEffect;

	private Animator anim;
	private bool isSlashing = false;
	private Vector3 direction;
	private Vector3 positionOffset;
	private AudioSource audio;
	private ContactFilter2D filter;
	private PlayerData playerData;
	private bool swinging = false;

	private bool thrown = false;
	private bool returning = false;
	private Vector3 throwDirection;
	private Vector3 throwVelocity;
	private float distanceTraveled = 0f;
	void Start()
	{
		this.anim = GetComponent<Animator>();
		this.playerData = Object.FindObjectOfType<PlayerData>();
		this.filter = new ContactFilter2D();
		if (transform.parent != null) {
			this.positionOffset = transform.position - transform.parent.position;
		}
	}

	void Update(){
		if (this.thrown) {
			if (this.returning) {
				Vector3 dir = transform.position - owner.transform.position;
				dir.Normalize();
				Vector3 newPosition = transform.position - dir * this.throwingSpeed * Time.deltaTime;
				Vector3 diff = owner.transform.position - newPosition;
				if (diff.magnitude < 1f) {
					this.distanceTraveled = 0f;
					transform.position = this.owner.transform.position;
					this.anim.SetBool("thrown", false);
					this.thrown = false;
					this.returning = false;
				} else {
					transform.position = newPosition;
				}
			} else {
				Vector3 nextStep = this.throwVelocity * Time.deltaTime;

				if (this.distanceTraveled + nextStep.magnitude >= this.throwingRange) {
					this.returning = true;
				} else {
					this.distanceTraveled += nextStep.magnitude;
				}
				transform.position += nextStep;
			}

			transform.position -= this.playerData.GetLastPlayerMovement() / Time.fixedDeltaTime * Time.deltaTime;
		}
	}

	public void swing(Vector3 dir){
		if (this.thrown) {
			return;
		}

		if (this.positionOffset == null) {
			this.positionOffset = transform.position - transform.parent.position;
		}

		if (this.anim.GetBool("slashing")) {
			return;
		}
		this.swinging = true;
		this.direction = dir;
		float angle = ( Mathf.Atan2(dir.y, dir.x) - Mathf.PI ) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		this.anim.SetBool("slashing", true);
		transform.position = transform.parent.position + this.positionOffset + dir * 0.2f;
		if (this.soundEffect != null) {
			Instantiate(this.soundEffect, transform.position, Quaternion.identity);
		}
	}

	public void throwSword(Vector3 dir) {
		if (this.thrown) {
			return;
		}
		Debug.Log("thrown again?");
		dir.Normalize();
		this.anim.SetBool("thrown", true);
		this.throwVelocity = dir * this.throwingSpeed;
		this.thrown = true;
	}


	private Vector3[] points = new Vector3[3];
	private void returnSword(){
		// Implement a way to send the sword back to the player
		// In a nice arc.

	}

	public bool isSwinging(){
		return this.swinging;
	}

	// Is called on last frame of animation
	void slashSwingDone(){
		this.anim.SetBool("slashing", false);
	}

	// Is called from the animation
	void strikeTarget(){
		this.swinging = false;
		Vector2 origin = new Vector2(transform.position.x, transform.position.y);
		Vector2 dir = new Vector2(this.direction.x, this.direction.y);
		List<Collider2D> hits = new List<Collider2D>();
		int nrOfHits = Physics2D.OverlapCircle(origin, this.range, this.filter.NoFilter(), hits);

		for (int i =0; i < nrOfHits; i++) {
			Collider2D hit = hits[i];
			if (hit.GetComponent<EntityController>() != null && hit.gameObject != this.owner) {

				hit.GetComponent<EntityController>().takeDamage(this.damage);

				Vector3 bloodDirection = hit.gameObject.transform.position - this.owner.transform.position;

				float angle = ( Mathf.Atan2( bloodDirection.y,  bloodDirection.x) + Mathf.PI/2 ) * Mathf.Rad2Deg;
				Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				GameObject blood = Instantiate(hitEffect, hit.transform.position, rotation);
			} else if (hit.GetComponent<TriggerActionController>() != null) {
				hit.GetComponent<TriggerActionController>().triggerAction(transform.parent.gameObject, gameObject);
			}
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, this.range);
	}
}
