using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Normal attack will deal damage to all entities within an AOE defined by the properties
[System.Serializable]
public class NormalAttack : IAttack
{
	public int damage;
	public float radius;
	public GameObject hitbox;
	// Should:
	// 1. deal damage to entities
	// 2. play sound

	public void dealDamage(Vector3 target){
		if (this.hitbox != null) {
			target = this.hitbox.transform.position;
			this.radius = this.hitbox.GetComponent<Hitbox>().radius;
		}

		List<Collider2D> hits = this.getCurrentHits(target);
		foreach (Collider2D hit in hits) {
			if (hit.gameObject.name == "Player") {
				continue;
			}
			HealthPoints life = hit.gameObject.GetComponent<HealthPoints>();
			if (life != null) {
				life.takeDamage(this.damage, target);
			}
		}
	}

	public List<Collider2D> getCurrentHits(Vector3 target){
		Vector2 origin = new Vector2(target.x, target.y);
		List<Collider2D> hits = new List<Collider2D>();
		Physics2D.OverlapCircle(origin, this.radius, new ContactFilter2D(), hits);
		return hits;
	}
}
