using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : EntityController
{
	public float speed = 8f;
	public int missileDamage = 0;
	public GameObject targetEntity;
	public MagicMissileController missileController;


	public float castTime = 2f;
	private float elapsedCastTime = 0f;
	private bool onCooldown = false;

	public enum State {
		Idle = 0,
		Chase = 1,
		Cast = 2,
		Cooldown = 3,
	}
	State state;
	Animator anim;
	Rigidbody2D rigidBody;

	void Start()
	{
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		speed += 2 * Random.value;
		if (targetEntity == null) {
			targetEntity = GameObject.Find("Player");
		}
	}

	void Update()
	{
		if (this.currentHealth <= 0) {
			Destroy(gameObject);
			return;
		}
		// Let's use the physics collsion but not make this object care about forces
		rigidBody.velocity = new Vector3(0, 0, 0);
		switch (state)
		{
		case ( State.Idle ):
			IdleUpdate();
			break;
		case ( State.Chase ):
			ChaseUpdate();
			break;
		case ( State.Cast ):
			CastUpdate();
			break;
		default:
			Debug.Log("this should not happen :O");
			break;
		}
	}

	void alterState(State newState)
	{
		state = newState;
	}

	void IdleUpdate()
	{
		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);
		anim.SetFloat("speed", 0f);
		if (distanceToTarget < 10) {
			alterState(State.Chase);
		}
	}

	void ChaseUpdate(){
		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);

		if (distanceToTarget < 8) {
			if (onCooldown) {
				alterState(State.Idle);
			}else {
				alterState(State.Cast);
			}

			return;
		} else if (distanceToTarget > 10) {
			alterState(State.Idle);
			return;
		}

		Vector3 targetDirection = targetEntity.transform.position - transform.position;
		targetDirection.Normalize();
		anim.SetFloat("speed", targetDirection.magnitude);
		anim.SetFloat("dirX", targetDirection.x);
		anim.SetFloat("dirY", targetDirection.y);
		Vector3 newPosition = transform.position + targetDirection * speed * Time.deltaTime;
		transform.position = newPosition;
	}


	void CastUpdate()
	{

		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);

		if (distanceToTarget > 10 && !anim.GetBool("casting")) {
			anim.SetBool("casting", false);
			alterState(State.Idle);
			return;
		}else if (distanceToTarget > 8 && !anim.GetBool("casting")) {
			anim.SetBool("casting", false);
			alterState(State.Chase);
			return;
		}

		elapsedCastTime = elapsedCastTime + Time.deltaTime;
		if (anim.GetBool("casting") && elapsedCastTime > castTime) {
			Debug.Log("pewpew");
			// Vector3 direction = targetEntity.transform.position - transform.position;
			// direction.Normalize();
			this.missileController.shoot(targetEntity.transform.position, this.missileDamage);
			anim.SetBool("casting", false);
			onCooldown = true;
			StartCoroutine(offCooldown());
			alterState(State.Idle);
			return;
		}

		if (!anim.GetBool("casting") && !onCooldown) {
			anim.SetBool("casting", true);
			elapsedCastTime = 0;
			this.missileController.spawn();
		}
	}

	IEnumerator offCooldown()
	{
		yield return new WaitForSeconds(3);
		this.missileController.destroyProjectile();
		onCooldown = false;
	}
}
