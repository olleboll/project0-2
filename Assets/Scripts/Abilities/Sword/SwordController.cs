using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
	public int damage = 0;
	public float range = 0f;
	public GameObject owner;
	public GameObject hitEffect;
	public GameObject soundEffect;

	private Animator anim;
	private bool isSlashing = false;
	private Vector3 direction;
	private Vector3 positionOffset;
	private AudioSource audio;
	private ContactFilter2D filter;
	private bool swinging = false;
	void Start()
	{
		this.anim = GetComponent<Animator>();
		this.filter = new ContactFilter2D();
		if (transform.parent != null) {
			this.positionOffset = transform.position - transform.parent.position;
		}
	}

	public void swing(Vector3 dir){

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
				Debug.Log("*** SWROD HIT TRIGGER****");
				hit.GetComponent<TriggerActionController>().triggerAction(transform.parent.gameObject, gameObject);
			}
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, this.range);
	}
}
