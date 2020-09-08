using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	public int damage = 0;
	public bool isAoe = false;
	public float radius = 0f;
	public Vector3 direction;
	public Vector3 animatedMoveTo;
	public float projectileSpeed;
	public GameObject hitEffect;

	private GameObject owner;
	private bool reachedTarget = false;
	private bool hasFired = false;

	void Update(){
		if (reachedTarget) {
			return;
		}

		if (this.animatedMoveTo.magnitude != 0 ) {
			Vector3 dir = transform.position - this.animatedMoveTo;
			float distance = dir.magnitude;
			dir.Normalize();
			transform.position = transform.position + dir * 2f * Time.deltaTime * distance / 5;
		} else if (this.hasFired) {
			transform.position += this.projectileSpeed * this.direction * Time.deltaTime;
		}
	}

	public void shoot(GameObject owner, Vector3 direction, float speed, int damage){
		this.owner = owner;
		this.direction = direction;
		this.projectileSpeed = speed;
		this.damage = damage;
		this.hasFired = true;
		this.animatedMoveTo = new Vector3(0,0,0);
	}

	void OnTriggerEnter2D(Collider2D col) {
		GameObject entity = col.gameObject;
		if (entity == this.owner || !this.hasFired || entity.name == this.name) {
			return;
		}
		if (entity.GetComponent<EntityController>() != null) {
			entity.GetComponent<EntityController>().takeDamage(this.damage);
		}
		StartCoroutine(destroyAfterDelay(0.1f));
		this.reachedTarget = true;
		if (hitEffect != null) {
			Instantiate(hitEffect, transform.position, Quaternion.identity);
		}
	}

	public void destroy(float delay){
		if (delay > 0) {
			StartCoroutine(destroyAfterDelay(delay));
		} else {
			Destroy(gameObject);
		}
	}

	IEnumerator destroyAfterDelay(float delay){
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}
}
