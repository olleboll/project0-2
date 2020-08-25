using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantController : MonoBehaviour
{
	public float speed = 0f;
	public float comfortRadius = 3f;
	public string state = "follow";

	private Animator anim;
	public GameObject assistantTo;
	private ContactFilter2D filter;
	private PlayerData playerData;

	private bool isIdling = false;
	private Vector3 idlingDestination = Vector3.zero;
	private Vector3 movementDirection = Vector3.zero;

	// Start is called before the first frame update
	void Start()
	{
		this.anim = GetComponent<Animator>();
		this.filter = new ContactFilter2D();
		this.playerData = Object.FindObjectOfType<PlayerData>();

		Vector3 startPosition = this.playerData.GetLastUniversePosition();
		if (startPosition.x > -999f) {
			transform.position = startPosition;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (this.state == "follow") {
			this.followTarget();
		}

		// negate playermovements
		transform.position -= this.playerData.GetLastPlayerMovement();
	}

	private void followTarget(){

		Vector3 vectorToTarget = this.assistantTo.transform.position - transform.position;

		if (vectorToTarget.magnitude > this.comfortRadius) {
			// LERP setup
			Vector3 currentPosition = transform.position;
			Vector3 target = this.assistantTo.transform.position;
			float interpolation = this.speed * Time.deltaTime;
			transform.position = StaticHelperFunctions.getLerpedPosition(currentPosition, target, interpolation);
			vectorToTarget.Normalize();
			this.anim.SetFloat("dirX", vectorToTarget.x);
			this.anim.SetFloat("dirY", vectorToTarget.y);
			this.anim.SetFloat("speed", this.speed);
		} else {
			this.movementDirection = Vector3.zero;
		}

	}

}
