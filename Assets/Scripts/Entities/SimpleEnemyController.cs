using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyController : EntityController
{
	public float speed = 8f;
	public GameObject targetEntity;
	public float attackCooldown;

	private SwordController swordController;
	private bool onCooldown = false;

	public enum State {
		Idle = 0,
		Chase = 1,
		Attack = 2,
	}
	State state = State.Idle;
	Animator anim;
	Rigidbody2D rigidBody;
	private ExternalForces forces;

	void Start()
	{
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		this.speed += 2 * Random.value;
		if (targetEntity == null) {
			targetEntity = GameObject.Find("Player");
		}
		this.swordController = GetComponentInChildren<SwordController>();
		this.forces = GetComponent<ExternalForces>();
	}

	void Update()
	{
		switch (this.state) {
		case State.Idle:
			idleUpdate();
			break;
		case State.Chase:
			chaseUpdate();
			break;
		case State.Attack:
			attackUpdate();
			break;
		}
	}

	private void idleUpdate()
	{
		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);
		anim.SetFloat("speed", 0f);
		if (distanceToTarget < 10) {
			alterState(State.Chase);
		}
	}

	private void chaseUpdate(){
		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);

		if (distanceToTarget < 2) {
			alterState(State.Attack);
			return;
		}else if (distanceToTarget > 10) {
			alterState(State.Idle);
			return;
		}

		Vector3 targetDirection = targetEntity.transform.position - transform.position;
		targetDirection.Normalize();
		anim.SetFloat("speed", targetDirection.magnitude);
		anim.SetFloat("dirX", targetDirection.x);
		anim.SetFloat("dirY", targetDirection.y);
		if (this.forces.canMove()) {
			Vector3 newPosition = transform.position + targetDirection * speed * Time.deltaTime;
			transform.position = newPosition;
		}
	}

	private void attackUpdate()
	{
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
			StartCoroutine(offCooldown(this.attackCooldown));
		}
		anim.SetFloat("speed", 0f);
		anim.SetFloat("dirX", dir.x);
		anim.SetFloat("dirY", dir.y);
	}

	IEnumerator offCooldown(float time){
		yield return new WaitForSeconds(time);
		this.onCooldown = false;
	}

	private void alterState(State newState)
	{
		state = newState;
	}

}
