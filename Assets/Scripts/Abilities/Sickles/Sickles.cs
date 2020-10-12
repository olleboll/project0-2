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
	private SickleAnimationHandler leftSickle;
	private SickleAnimationHandler rightSickle;
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
		this.leftSickle = transform.Find("Sickle1").gameObject.GetComponent<SickleAnimationHandler>();
		this.rightSickle = transform.Find("Sickle2").gameObject.GetComponent<SickleAnimationHandler>();
		this.leftSickle.onDealDamage += this.onDealDamage;
		this.rightSickle.onDealDamage += this.onDealDamage;
		this.leftSickle.onAnimationComplete += this.onAnimationComplete;
		this.rightSickle.onAnimationComplete += this.onAnimationComplete;
		this.positionOffset = transform.position - transform.parent.position;
		this.history = new AttackType[3];
	}

	void OnEnable(){
		this.leftSickle = transform.Find("Sickle1").gameObject.GetComponent<SickleAnimationHandler>();
		this.rightSickle = transform.Find("Sickle2").gameObject.GetComponent<SickleAnimationHandler>();
	}

	void OnDisable(){
		this.leftSickle.onDealDamage -= this.onDealDamage;
		this.rightSickle.onDealDamage -= this.onDealDamage;
	}

	void Update()
	{
		bool nonePlaying = !this.leftSickle.getIsPlaying() &&
		                   !this.rightSickle.getIsPlaying();

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
				this.startAttack(this.queuedAttack);
			} else if (nonePlaying) {
				this.currentAttack = AttackType.idle;
				this.queuedAttack = AttackType.idle;
				this.history = new AttackType[3];
				this.playerState.resetState(Sickles._state);
			}

			return;
		} else if (!nonePlaying) {
			this.leftSickle.stopAnimation();
			this.rightSickle.stopAnimation();
		}

		if ((this.input.attack1 || this.input.attack2)
		    && this.playerState.setState(Sickles._state)) {
			if (this.input.attack1) {
				this.startAttack(AttackType.left);

			} else if (this.input.attack2) {
				this.startAttack(AttackType.right);
			}
			return;
		}
	}
	void FixedUpdate(){
		if (this.playerState.state == Sickles._state) {
			if (this.moveTowardsTarget != null && !this.moveTowardsTarget.reachedTarget && !this.input.focusing) {
				Vector3 newPosition = this.moveTowardsTarget.nextPosition(Time.fixedDeltaTime);
				this.body.MovePosition(newPosition);
			} else if (!this.input.focusing) {
				this.playerMove.move();
				this.playerAnimation.updateAnimation(this.input.moveDirection, 1);
			}
		}
	}

	private void startAttack(AttackType attack) {
		if (attack == AttackType.idle) {
			return;
		}
		Vector3 dir = this.input.swordDirection;
		float angle = ( Mathf.Atan2(dir.y, dir.x) - 3*Mathf.PI/2 ) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.position = transform.parent.position + this.positionOffset + dir * 0.2f;
		if (attack == AttackType.left) {
			this.leftSickle.play(SickleAnimationHandler.Anim.attack_normal, "sickle1");
		}
		if (attack == AttackType.right) {
			this.rightSickle.play(SickleAnimationHandler.Anim.attack_normal, "sickle2");
		}
		if (attack == AttackType.both) {
			this.leftSickle.play(SickleAnimationHandler.Anim.attack_normal, "sickle1");
			this.rightSickle.play(SickleAnimationHandler.Anim.attack_normal, "sickle2");
		}
		if (attack == AttackType.spin) {
			PlayerAnimationHandler ani = this.player.GetComponent<PlayerAnimationHandler>();
			ani.play(PlayerAnimationHandler.Anim.spin);
			this.leftSickle.play(SickleAnimationHandler.Anim.attack_spin, "sickle1");
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
		Debug.Log(a);
		IAttack attack = getAttackType(a);
		if (attack != null) {
			attack.attack(transform.position);
		}
	}

	private void onAnimationComplete(string id){
		Debug.Log("ani complete");
		Debug.Log(id);
	}
}
