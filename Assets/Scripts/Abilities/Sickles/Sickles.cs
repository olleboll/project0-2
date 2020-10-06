using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickles : MonoBehaviour
{
	public int damage = 0;
	public float range = 0f;
	public float thrustSpeed = 10f;
	public float thrustDistance = 3f;

	public GameObject soundEffect;

	private PlayerState playerState;
	private PlayerInputController input;
	private PlayerMovement playerMove;
	private PlayerAnimationHandler playerAnimation;
	private GameObject player;
	private SickleAnimationHandler sickle1;
	private SickleAnimationHandler sickle2;
	private Vector3 positionOffset;
	private Rigidbody2D body;
	private static PlayerState.State _state = PlayerState.State.attacking;

	private enum AttackType {
		idle,
		right,
		left,
		both,
		spin
	}
	public NormalAttack rightAttack;
	public NormalAttack leftAttack;
	public KnockbackAttack bothAttack;
	public KnockbackAttack spinAttack;

	private Vector3 attackDirection = Vector3.zero;
	private AttackType[] history;
	private AttackType currentAttack = AttackType.idle;
	private AttackType queuedAttack = AttackType.idle;
	private MoveTowardsTarget moveTowardsTarget;

	void Start()
	{
		this.player = transform.parent.gameObject;
		this.body = this.player.GetComponent<Rigidbody2D>();
		this.playerState = Object.FindObjectOfType<PlayerState>();
		this.input = Object.FindObjectOfType<PlayerInputController>();
		this.playerMove = this.player.GetComponent<PlayerMovement>();
		this.playerAnimation = this.player.GetComponent<PlayerAnimationHandler>();
		this.sickle1 = transform.Find("Sickle1").gameObject.GetComponent<SickleAnimationHandler>();
		this.sickle2 = transform.Find("Sickle2").gameObject.GetComponent<SickleAnimationHandler>();
		this.sickle1.onDealDamage += this.onDealDamage;
		this.sickle2.onDealDamage += this.onDealDamage;
		this.positionOffset = transform.position - transform.parent.position;
		this.history = new AttackType[3];
	}

	void OnEnable(){
		this.sickle1 = transform.Find("Sickle1").gameObject.GetComponent<SickleAnimationHandler>();
		this.sickle2 = transform.Find("Sickle2").gameObject.GetComponent<SickleAnimationHandler>();
	}

	void OnDisable(){
		this.sickle1.onDealDamage -= this.onDealDamage;
		this.sickle2.onDealDamage -= this.onDealDamage;
	}

	void Update()
	{
		bool nonePlaying = !this.sickle1.getIsPlaying() &&
		                   !this.sickle2.getIsPlaying();

		if (this.playerState.state == Sickles._state) {
			// Player is attacking..
			// Queue whatever attack they pushed last?
			if (this.input.attack1) {
				if (this.history[0] == AttackType.right && this.history[1] == AttackType.right) {
					this.queuedAttack = AttackType.spin;
				} else if (this.history[0] == AttackType.right && this.history[1] == AttackType.left) {
					this.queuedAttack = AttackType.both;
				} else {
					this.queuedAttack = AttackType.left;
				}

			}
			if (this.input.attack2) {
				if (this.history[0] == AttackType.left && this.history[1] == AttackType.right) {
					this.queuedAttack = AttackType.both;
				} else {
					this.queuedAttack = AttackType.right;
				}
			}

			if (nonePlaying && this.queuedAttack != AttackType.idle) {
				this.startNormalAttack(this.queuedAttack);
			} else if (nonePlaying) {
				this.currentAttack = AttackType.idle;
				this.queuedAttack = AttackType.idle;
				this.history = new AttackType[3];
				this.playerState.resetState(Sickles._state);
			}
			return;
		} else if (!nonePlaying) {
			this.sickle1.stopAnimation();
			this.sickle2.stopAnimation();
		}

		if ((this.input.attack1 || this.input.attack2)
		    && this.playerState.setState(Sickles._state)) {
			if (this.input.attack1) {
				this.startNormalAttack(AttackType.left);
			} else if (this.input.attack2) {
				this.startNormalAttack(AttackType.right);
			}
			return;
		}
	}
	void FixedUpdate(){
		if (this.playerState.state == Sickles._state) {
			if (!this.moveTowardsTarget.reachedTarget) {
				Vector3 newPosition = this.moveTowardsTarget.nextPosition(Time.fixedDeltaTime);
				this.body.MovePosition(newPosition);
			} else {
				this.playerMove.move();
				this.playerAnimation.updateAnimation(this.input.swordDirection, 1);
			}
		}
	}

	private void startNormalAttack(AttackType attack) {
		if (attack == AttackType.idle) {
			return;
		}
		Vector3 dir = this.input.swordDirection;
		float angle = ( Mathf.Atan2(dir.y, dir.x) - 3*Mathf.PI/2 ) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.position = transform.parent.position + this.positionOffset + dir * 0.2f;
		if (attack == AttackType.left) {
			this.sickle1.play(SickleAnimationHandler.Anim.attack_normal);
		}
		if (attack == AttackType.right) {
			this.sickle2.play(SickleAnimationHandler.Anim.attack_normal);
		}
		if (attack == AttackType.both) {
			this.sickle1.play(SickleAnimationHandler.Anim.attack_normal);
			this.sickle2.play(SickleAnimationHandler.Anim.attack_normal);
		}
		if (attack == AttackType.spin) {
			PlayerAnimationHandler ani = this.player.GetComponent<PlayerAnimationHandler>();
			ani.play(PlayerAnimationHandler.Anim.spin);
			this.sickle1.play(SickleAnimationHandler.Anim.attack_spin);
		}
		this.currentAttack = attack;
		this.queuedAttack = AttackType.idle;

		AttackType[] newHistory = new AttackType[3];
		newHistory[0] = attack;
		newHistory[1] = this.history[0];
		newHistory[2] = this.history[1];
		this.history = newHistory;

		this.attackDirection = dir;
		this.moveTowardsTarget = new MoveTowardsTarget(this.body.position, this.attackDirection, this.thrustDistance, this.thrustSpeed);
		if (this.soundEffect != null) {
			Instantiate(this.soundEffect, transform.position, Quaternion.identity);
		}
	}

	private IAttack getAttackType(AttackType attack){
		if (attack == AttackType.left) {
			return this.leftAttack;
		}
		if (attack == AttackType.right) {
			return this.rightAttack;
		}
		if (attack == AttackType.both) {
			return this.bothAttack;
		}
		if (attack == AttackType.spin) {
			return this.spinAttack;
		}
		return null;
	}

	private void onDealDamage(){
		// Dealing damage
		Debug.Log("Dealing damage with");
		AttackType a = this.currentAttack;
		IAttack attack = getAttackType(a);
		// Implement damage dealing depening on attack
		// 02/10-2020

		attack.dealDamage(transform.position);
	}
}
