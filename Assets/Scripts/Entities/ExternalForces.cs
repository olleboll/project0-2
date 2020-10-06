using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalForces : MonoBehaviour
{
	private List<MoveTowardsTarget> forces;
	private Rigidbody2D body;
	void Start()
	{
		this.forces = new List<MoveTowardsTarget>();
		this.body = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		if (this.forces.Count == 0) {
			return;
		}
		Vector3 newPosition = this.body.position;
		foreach (MoveTowardsTarget force in this.forces) {
			newPosition += force.nextStep(Time.fixedDeltaTime);
		}

		// Filter out "finished" forces
		this.forces = this.forces.FindAll(f => !f.reachedTarget);

		this.body.MovePosition(newPosition);
	}

	public void addForce(Vector3 source, float distance, float speed){
		Debug.Log("Adding force");
		Vector3 direction = new Vector3(this.body.position.x, this.body.position.y,0) - source;
		Debug.Log(direction);
		this.forces.Add(new MoveTowardsTarget(this.body.position, direction, distance, speed));
		Debug.DrawRay(new Vector3(this.body.position.x, this.body.position.y,0), direction * 10, Color.red, 5);
	}

	public bool canMove(){
		return this.forces.Count == 0;
	}
}
