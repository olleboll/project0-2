using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoints : MonoBehaviour
{
	public int maxHealth = 0;
	public int currentHealth = 0;

	void Start()
	{
		this.currentHealth = maxHealth;
	}

	public void takeDamage(int damage){
		Debug.Log("taking damage");
		Debug.Log(damage);
		Debug.Log(this.currentHealth);
		this.currentHealth -= damage;
	}
}
