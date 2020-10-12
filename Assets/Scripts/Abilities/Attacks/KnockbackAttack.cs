using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Normal attack will deal damage to all entities within an AOE defined by the properties
[System.Serializable]
public class KnockbackAttack : IAttack
{
	public int damage;
	public float radius;
	public float knockBackDistance;
	public float knockBackSpeed;
	public GameObject hitbox;
	public GameObject source;
	// Should:
	// 1. deal damage to entities
	// 2. play sound

	public void attack(Vector3 target){
		if (this.hitbox != null) {
			target = this.hitbox.transform.position;
			this.radius = this.hitbox.GetComponent<Hitbox>().radius;
		}

		List<Collider2D> hits = this.getCurrentHits(target);
		foreach (Collider2D hit in hits) {
			if (hit.gameObject.name == "Player") {
				continue;
			}
			Vector3 hitPoint = this.hitbox.transform.parent.position;//hit.ClosestPoint(target);
			HealthPoints life = hit.gameObject.GetComponent<HealthPoints>();
			if (life != null) {
				life.takeDamage(this.damage, hitPoint);
			}
			ExternalForces forces = hit.gameObject.GetComponent<ExternalForces>();
			if (forces != null) {
				forces.addForce(hitPoint, this.knockBackDistance, this.knockBackSpeed);
			}
		}
	}

	public List<Collider2D> getCurrentHits(Vector3 target){
		Vector2 origin = new Vector2(target.x, target.y);
		List<Collider2D> hits = new List<Collider2D>();
		Physics2D.OverlapCircle(origin, this.radius, new ContactFilter2D(), hits);
		return hits;
	}
	public void update(float deltaTime){
	}
}
