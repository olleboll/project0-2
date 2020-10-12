using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{

	List<Collider2D> getCurrentHits(Vector3 target);
	void attack(Vector3 target);
	void update(float deltaTime);
}
