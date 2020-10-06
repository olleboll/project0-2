using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
	public enum State {
		idle,
		moving,
		attacking,
		dashing,
		teleporting,
		dialogue,
		cinematic
	}

	public State state;

	private PlayerInputController input;

	void Start(){
		this.input = Object.FindObjectOfType<PlayerInputController>();
		// read the state stored in playerData and determine what to do?
		// If the player was dashing before teleporting it should go back to dashing etc.
	}

	public bool setState(State toState) {
		if (toState == State.moving) {
			if (this.state == State.idle) {
				this.state = State.moving;
				return true;
			} else {
				return false;
			}
		}

		if (toState == State.dashing) {
			if (this.state == State.moving || this.state == State.attacking) {
				this.state = State.dashing;
				return true;
			} else {
				return false;
			}
		}


		if (toState == State.attacking) {
			if (this.state == State.idle ||
			    this.state == State.moving) {
				this.state = State.attacking;
				return true;
			} else {
				return false;
			}
		}

		return false;
	}

	public void resetState(State fromState) {
		if (fromState == State.dashing && this.state == State.dashing) {
			this.state = State.idle;
		}

		if (fromState == State.moving && this.state == State.moving) {
			this.state = State.idle;
		}

		if (fromState == State.attacking && this.state == State.attacking) {
			this.state = State.idle;
		}
	}

}
