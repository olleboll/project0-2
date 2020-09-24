using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
	public int maxHealth = 0;
	public int currentHealth = 0;

	public virtual void takeDamage(int damage){
		Debug.Log(this.name + " Took damage!!");
		this.currentHealth -= damage;
		if (this.currentHealth <= 0) {
			Destroy(gameObject);
			return;
		}
	}

	void OnCollision2D(Collider2D col){
		if (GetComponent<Rigidbody2D>() != null) {
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}
	}
}
