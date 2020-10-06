using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfController : EntityController
{
	public float speed = 6f;
	public float attackSpeed = 12f;
	public int attackDamage = 5;
	public float attackTime = 2f;
	public float cooldown = 2f;
	public float chaseDistance = 8f;
	public float attackDistance = 3f;


	public enum State {
		Idle = 0,
		Chase = 1,
		Attack = 2,
		Cooldown = 3,
	}

	private GameObject targetEntity;
	private State state = State.Idle;
	private Animator anim;
	private Rigidbody2D body;

	private Vector3 direction;
	private float defaultSpeed;
	private float attackCountdown = 0f;
	private bool onCooldown = false;

	void Start()
	{
		anim = GetComponent<Animator>();
		this.body = GetComponent<Rigidbody2D>();
		targetEntity = GameObject.Find("Player");

		this.defaultSpeed = this.speed;
	}

	// Update is called once per frame
	void Update()
	{
		if (this.currentHealth <= 0) {
			Destroy(gameObject);
			return;
		}
		switch (state)
		{
		case ( State.Idle ):
			IdleUpdate();
			break;
		case ( State.Chase ):
			ChaseUpdate();
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
		this.speed = 0;
		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);
		if (distanceToTarget < this.chaseDistance && !this.onCooldown) {
			alterState(State.Chase);
		}
	}

	void ChaseUpdate(){
		this.speed = this.defaultSpeed;
		this.direction = this.targetEntity.transform.position - transform.position;
		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);

		if (distanceToTarget < this.attackDistance) {
			if (this.onCooldown) {
				alterState(State.Idle);
			}else {
				alterState(State.Attack);
			}
		} else if (distanceToTarget > this.chaseDistance) {
			alterState(State.Idle);
		}
	}

	void AttackUpdate(){
		if (this.attackCountdown > this.attackTime  ) {
			this.direction =  this.targetEntity.transform.position - transform.position;
			this.speed = this.attackSpeed;
			this.attackCountdown = 0f;
			this.onCooldown = true;
			StartCoroutine(offCooldown());
			return;
		} else if (!this.onCooldown) {
			this.speed = 0f;
			this.attackCountdown += Time.deltaTime;
			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			renderer.color = Color.red;
		}

		float distanceToTarget = Vector3.Distance(targetEntity.transform.position, transform.position);

		if (distanceToTarget > this.attackDistance + 2f) {
			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			renderer.color = Color.white;
			alterState(State.Idle);
		}

	}

	void FixedUpdate(){
		this.direction.Normalize();
		Vector3 newPosition = new Vector3(this.body.position.x, this.body.position.y, 0) + this.direction * this.speed * Time.fixedDeltaTime;

		this.body.MovePosition(newPosition);

		anim.SetFloat("speed", this.speed);
		anim.SetFloat("dirX", this.direction.x);
		anim.SetFloat("dirY", this.direction.y);

		Debug.Log(this.state);
		Debug.Log(this.speed);
	}

	IEnumerator offCooldown()
	{
		yield return new WaitForSeconds(this.cooldown);
		this.onCooldown = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject == this.targetEntity) {
			//this.targetEntity.GetComponent<EntityController>().takeDamage(this.attackDamage);
		}
	}

}
