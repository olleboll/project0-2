using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EntityController
{
	public float speed = 8f;
	public int missileDamage = 0;
	public GameObject targetEntity;
	public MagicMissileController missileController;
	public ThunderStrikeController thunderController;

	private SwordController swordController;
	public float castTime = 2f;
	private float elapsedCastTime = 0f;
	private bool onCooldown = false;

	public enum State {
		Idle = 0,
		Chase = 1,
		CastMagicMissile = 2,
		CastThunderStrike = 3,
		Cooldown = 4,
		Attack = 5,
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
		this.swordController = GetComponentInChildren<SwordController>();
		this.thunderController = GetComponent<ThunderStrikeController>();
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
		case ( State.CastMagicMissile ):
			CastMagicMissileUpdate();
			break;
		case ( State.CastThunderStrike ):
			CastThunderStrikeUpdate();
			break;
		case ( State.Attack ):
			AttackUpdate();
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
		if (distanceToTarget < 15) {
			alterState(State.Chase);
		}
	}

	void ChaseUpdate(){
		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);

		if (distanceToTarget < 10 && distanceToTarget > 4) {
			if (!onCooldown) {
				alterState(State.CastMagicMissile);
				return;
			}
		} else if (distanceToTarget < 15 && distanceToTarget >10) {
			if (!onCooldown) {
				alterState(State.CastThunderStrike);
			}
		}else if (distanceToTarget > 15) {
			alterState(State.Idle);
			return;
		} else if (distanceToTarget < 2) {
			alterState(State.Attack);
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

	void AttackUpdate(){
		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);

		if (distanceToTarget > 2) {
			alterState(State.Chase);
			return;
		}
		Vector3 dir = targetEntity.transform.position - transform.position;
		if (!this.onCooldown) {
			dir.Normalize();
			this.swordController.swing(dir);
			onCooldown = true;
			StartCoroutine(offCooldown(0.5f));
		}
		anim.SetFloat("speed", 0f);
		anim.SetFloat("dirX", dir.x);
		anim.SetFloat("dirY", dir.y);
	}

	void CastMagicMissileUpdate()
	{

		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);

		if (distanceToTarget > 10 && !anim.GetBool("casting")) {
			anim.SetBool("casting", false);
			alterState(State.Idle);
			return;
		} else if (distanceToTarget < 15 && distanceToTarget > 8 && !anim.GetBool("casting") && !thunderController.isCasting()) {
			if (!onCooldown) {
				alterState(State.CastThunderStrike);
			}
		}else if (distanceToTarget > 8 && !anim.GetBool("casting")) {
			anim.SetBool("casting", false);
			alterState(State.Chase);
			return;
		}

		elapsedCastTime = elapsedCastTime + Time.deltaTime;
		if (anim.GetBool("casting") && elapsedCastTime > castTime) {
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

	void CastThunderStrikeUpdate(){
		if (!thunderController.isCasting()) {
			float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);

			if (distanceToTarget > 15 && !anim.GetBool("casting")) {
				anim.SetBool("casting", false);
				alterState(State.Idle);
				return;
			}

			thunderController.startOnGameObjectCast();
			StartCoroutine(executeThunderStrike());
		}
	}

	IEnumerator executeThunderStrike(){
		yield return new WaitForSeconds(1);
		thunderController.startIndicatorAnimations(targetEntity.transform.position, 1f);
		StartCoroutine(stopThunderStrikeCast());
	}

	IEnumerator stopThunderStrikeCast(){
		yield return new WaitForSeconds(1);
		alterState(State.Idle);
		onCooldown = false;
	}

	IEnumerator offCooldown(float cd = 2f)
	{
		yield return new WaitForSeconds(cd);
		this.missileController.destroyProjectile();
		onCooldown = false;
	}
}
