using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Normal attack will deal damage to all entities within an AOE defined by the properties
[System.Serializable]
public class BoomerangAttack : IAttack
{
	public int damage;
	public float radius;
	public float range;
	public float speed;
	public GameObject boomerang;
	public GameObject owner;
	// Should:
	// 1. deal damage to entities
	// 2. play sound
	private float deltaTime = 0;
	private Vector3 start;
	private Vector3 target;
	private MoveTowardsTarget trajectory;
	private bool returning = false;
	public void attack(Vector3 target){
		this.target = target;
		this.start = this.boomerang.transform.position;
		Vector3 dir = this.target - this.start;
		this.trajectory = new MoveTowardsTarget(this.start, dir, this.range, this.speed);
	}

	public List<Collider2D> getCurrentHits(Vector3 target){
		Vector2 origin = new Vector2(target.x, target.y);
		List<Collider2D> hits = new List<Collider2D>();
		Physics2D.OverlapCircle(origin, this.radius, new ContactFilter2D(), hits);
		return hits;
	}

	public void update(float deltaTime){
		this.deltaTime = deltaTime;

		if (this.returning) {
			Vector3 dir = this.boomerang.transform.position - this.owner.transform.position;
			if (dir.magnitude < 0.2) {
				// this could probably be made into something better?
				// more general so I can reuse?
				// For now so what.. this is the only usecase
				this.boomerang.GetComponent<SickleAnimationHandler>().stopAnimation();
			} else {
				dir.Normalize();
				this.boomerang.transform.position += dir * this.speed * deltaTime;
			}
		} else {
			Vector3 newPos = this.trajectory.nextPosition(deltaTime);
			if (this.trajectory.reachedTarget) {
				this.returning = true;
			}
			this.boomerang.transform.position = newPos;
			List<Collider2D> hits = this.getCurrentHits(this.boomerang.transform.position);
			foreach (Collider2D hit in hits) {
				if (hit.gameObject.name == "Player") {
					continue;
				}
				Vector3 hitPoint = this.boomerang.transform.position;
				HealthPoints life = hit.gameObject.GetComponent<HealthPoints>();
				if (life != null) {
					// damage on each update.. so multiply damage with deltaTime?
					// omg... int vs floats!?
					life.takeDamage(this.damage, hitPoint);
				}
			}
		}


	}
}
