using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{

	public PlayerData.Ability ability;

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		if (hitInfo.name != "Player") {
			return;
		}
		Object.FindObjectOfType<PlayerData>().EnableAbility(this.ability);
		Destroy(gameObject);
	}
}
