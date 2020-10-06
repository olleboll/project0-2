using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will calculate the next position towards target
// Will not know about obstacles.
public class MoveTowardsTarget
{
	public bool reachedTarget = false;
	private Vector3 position;
	public Vector3 direction;
	private Vector3 target;
	private float distance;
	private float speed;
	private float distanceTraveled;

	public MoveTowardsTarget(Vector3 start, Vector3 direction, float distance, float speed){
		this.position = start;
		this.direction = direction;
		this.direction.Normalize();
		this.distance = distance;
		this.target = this.position + this.direction * this.distance;
		this.speed = speed;
		this.distanceTraveled = 0;
	}

	public Vector3 nextPosition(float delta){
		Vector3 nextStep = this.nextStep(delta);
		if (this.reachedTarget) {
			this.distanceTraveled = this.distance;
			this.position = this.target;
		} else {
			this.position += nextStep;
		}
		return this.position;
	}
	public Vector3 nextStep(float delta){
		Vector3 nextStep = this.direction * this.speed * delta;
		this.distanceTraveled += nextStep.magnitude;
		if (this.distanceTraveled >= this.distance) {
			this.reachedTarget = true;
		}
		return nextStep;
	}
}
