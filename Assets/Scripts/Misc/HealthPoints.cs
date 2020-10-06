using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoints : MonoBehaviour
{
	public int maxHealth = 0;
	public int currentHealth = 0;

	public GameObject damageTakenEffect;
	public GameObject deathEffect;

	void Start()
	{
		this.currentHealth = maxHealth;
	}

	public virtual void takeDamage(int damage, Vector3 source){
		this.currentHealth -= damage;
		Vector3 bloodDirection = transform.position - source;
		float angle = ( Mathf.Atan2( bloodDirection.y,  bloodDirection.x) + Mathf.PI/2 ) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		GameObject blood = Instantiate(damageTakenEffect, transform.position, rotation);
		this.takeDamage(damage);
	}

	public virtual void takeDamage(int damage){
		Debug.Log(this.name + " Took damage!!");
		this.currentHealth -= damage;

		if (this.currentHealth <= 0) {
			if (deathEffect) {
				Instantiate(deathEffect, transform.position, Quaternion.identity);
			}
			Destroy(gameObject);
			return;
		}
	}

}
