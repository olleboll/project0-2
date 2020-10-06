using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerMovementController : EntityController
{
	public float speed;
	public float attackDashSpeed;
	public float cameraSpeed = 0f;
	private AudioSource walkingSound;
	private PlayerInputController input;
	private Animator anim;

	private Vector3 lastMoveDirection;
	private Vector3 moveDirection;
	private bool isDashing = false;
	private bool isMegaDashing = false;
	private float defaultSpeed;
	private DashController dashController;
	private MegaDashController megaDashController;
	private SwordController swordController;
	private Rigidbody2D body;
	private UnityEngine.Tilemaps.Tilemap groundTilemap;
	private Light2D light;
	private float lightStartRadius;

	private Light2D medallionLight;
	private Color defaultMedallionLightColor;
	private float defaultMedallionLightIntensity;

	private PlayerData playerData;
	private SceneController sceneController;

	private bool performMegaDashOnNextUpdate = false;
	private bool isAiming = false;

	private float cameraHalfWidth;
	private float cameraHalfHeight;
	private Vector2 maxTilemapBounds;
	private Vector2 minTilemapBounds;

	private PrologueScene sceneData;

	void Start()
	{
		this.defaultSpeed = this.speed;
		this.input = Object.FindObjectOfType<PlayerInputController>();
		this.body = GetComponent<Rigidbody2D>();
		this.anim = GetComponent<Animator>();
		this.lastMoveDirection = new Vector3(0,0,0);
		this.dashController = GetComponent<DashController>();
		this.megaDashController = GetComponent<MegaDashController>();
		this.swordController = GetComponentInChildren<SwordController>();
		this.walkingSound = GetComponent<AudioSource>();
		this.groundTilemap = GameObject.Find("ground").GetComponent<UnityEngine.Tilemaps.Tilemap>();
		this.light = transform.Find("light").GetComponent<Light2D>();
		this.lightStartRadius = this.light.pointLightOuterRadius;

		if (GameObject.Find("Prologue") != null) {
			this.sceneData = GameObject.Find("Prologue").GetComponent<PrologueScene>();

			if (this.sceneData.nightOrDay == "night") {
				this.light.intensity = 0.6f;
			} else {
				this.light.intensity = 0f;
			}

		} else {
			this.light.intensity = 0f;
		}

		this.medallionLight = transform.Find("MedallionLight").GetComponent<Light2D>();
		this.defaultMedallionLightColor = this.medallionLight.color;
		this.defaultMedallionLightIntensity = this.medallionLight.intensity;

		this.playerData = Object.FindObjectOfType<PlayerData>();
		this.sceneController = Object.FindObjectOfType<SceneController>();

		GameEvents.current.onPortalAreaEntered += onPortalAreaEntered;
		GameEvents.current.onPortalAreaExited += onPortalAreaExited;

		Vector3 startPosition = this.playerData.GetLastUniversePosition();
		if (startPosition != this.playerData.defaultPlayerSpawn) {
			transform.position = startPosition;
			Camera.main.transform.position = new Vector3(startPosition.x, startPosition.y, -10);
		}

		if (this.playerData.getMegaDashDirection().magnitude > 0) {
			this.performMegaDashOnNextUpdate = true;
		}

		this.maxTilemapBounds = this.groundTilemap.localBounds.max;
		this.minTilemapBounds = this.groundTilemap.localBounds.min;

		this.cameraHalfHeight = Camera.main.orthographicSize;
		this.cameraHalfWidth = this.cameraHalfHeight * Screen.width / Screen.height;
	}

	// Update is called once per frame
	void Update()
	{
		if (this.playerData.getTeleporting()) {
			return;
		}
		// This si basically the die function.. TODO
		// if (this.currentHealth <= 0) {
		// 	this.currentHealth = 1000;
		// 	SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		// 	return;
		// }

		if (!this.dashController.isDashing && !this.megaDashController.isDashing && !this.isAiming) {
			this.handleMovement();
		}

		if (this.playerData.hasDash) {
			this.handleDash();
		}
		if (this.playerData.hasMegaDash) {
			this.handleMegaDash();
		}
		if (this.playerData.hasSickle) {
			this.handleSword();
		}

	}

	void FixedUpdate(){

		if (this.playerData.getTeleporting() ) {
			return;
		}

		// Let's only care about normal movements if we are not doing some other movement
		if (!this.dashController.isDashing && !this.megaDashController.isDashing && !this.isAiming) {

			float currentSpeed = this.moveDirection.magnitude;

			float newXMovement = moveDirection.x * speed * Time.fixedDeltaTime;
			float newYMovement = moveDirection.y * speed * Time.fixedDeltaTime;
			float newX = this.body.position.x + newXMovement;
			float newY = this.body.position.y + newYMovement;

			var grid = this.groundTilemap.layoutGrid;
			var tilePositionX = grid.WorldToCell(new Vector3(newX, this.body.position.y, 0));
			var tileX = this.groundTilemap.GetTile(tilePositionX);

			var tilePositionY = grid.WorldToCell(new Vector3(this.body.position.x, newY, 0));
			var tileY = this.groundTilemap.GetTile(tilePositionY);
			// Make an outOfBounds function in statis helper functions
			if (tileX == null ||
			    StaticHelperFunctions.isNonGroundTile(tileX.name) ||
			    newX < (this.minTilemapBounds.x + 1.2) ||
			    newX > (this.maxTilemapBounds.x - 2)) {
				newX = this.body.position.x;
				newXMovement = 0;
			}

			if (tileY == null || StaticHelperFunctions.isNonGroundTile(tileY.name) ||
			    newY < (this.minTilemapBounds.y + 1) ||
			    newY > (this.maxTilemapBounds.y - 3.5)) {
				newY = this.body.position.y;
				newYMovement = 0;
			}

			Vector3 newPosition = new Vector3(newX, newY, 0);
			this.body.MovePosition(newPosition);

			this.playerData.SetLastPlayerMovement(new Vector3(newXMovement, newYMovement));
		} else {
			this.anim.SetFloat("speed", 0);
		}


		this.handleCamera();
	}

	private void handleMovement(){
		this.moveDirection = this.input.moveDirection;
		float currentSpeed = moveDirection.magnitude;

		Vector3 currentDirection;

		if (currentSpeed > 0) {
			currentDirection = moveDirection;
			this.lastMoveDirection = moveDirection;

			if (!walkingSound.isPlaying) {
				walkingSound.Play();
			}
		}else {
			currentDirection = lastMoveDirection;
			walkingSound.Stop();
		}

		this.anim.SetFloat("dirX", currentDirection.x);
		this.anim.SetFloat("dirY", currentDirection.y);
		this.anim.SetFloat("speed", currentSpeed);
	}

	private float chargeDashTimer = 0f;
	private void handleMegaDash(){

		// Reimplement this

		// if (this.performMegaDashOnNextUpdate) {
		// 	this.megaDashController.dash(this.playerData.getMegaDashDirection());
		// 	this.performMegaDashOnNextUpdate = false;
		// }
		//
		// if (chargeDashTimer >= 1f && !this.input.chargeDash) {
		// 	this.chargeDashTimer = 0f;
		// 	this.megaDashController.dash(this.input.aimDirection);
		// } else if (chargeDashTimer < 1f && this.input.chargeDash) {
		// 	this.chargeDashTimer += 1 * Time.deltaTime;
		// } else if (!this.input.chargeDash) {
		// 	this.chargeDashTimer = 0f;
		// }
		//
		// if (this.megaDashController.isDashing && this.input.dash) {
		// 	this.megaDashController.stopDash();
		// }
	}

	private void handleDash(){
		if (
			!this.isDashing &&
			this.input.dash &&
			this.dashController != null &&
			!this.megaDashController.isDashing &&
			this.input.moveDirection.magnitude > 0) {
			this.dashController.dash(this.input.moveDirection);
		}
	}

	private void handleSword(){
		bool swing = this.input.attack;
		this.isAiming = false;
		if (swing && this.swordController != null && !this.swordController.isSwinging()) {
			this.swordController.swing(this.input.swordDirection);
			this.speed += this.attackDashSpeed;
		} else if (this.swordController.isSwinging()) {
			// Do nothing here?
		} else if (this.input.aimThrow) {
			this.isAiming = true;
		} else if (this.input.rangedAttack) {
			this.swordController.throwSword(this.input.swordDirection);
		} else {
			this.speed = this.defaultSpeed;
		}
	}

	private void handleCamera(){
		Vector3 currentPosition = Camera.main.transform.position;
		float interpolation = this.cameraSpeed * Time.deltaTime;
		Vector3 target = this.transform.position;

		Vector3 newCameraPosition = StaticHelperFunctions.getLerpedPosition(currentPosition, target, interpolation);

		newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, this.minTilemapBounds.x + this.cameraHalfWidth + 1, this.maxTilemapBounds.x - this.cameraHalfWidth - 2);
		newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, this.minTilemapBounds.y + this.cameraHalfHeight + 1, this.maxTilemapBounds.y - this.cameraHalfHeight - 2);
		newCameraPosition.z = -10;

		Camera.main.transform.position = newCameraPosition;
	}

	public void onPortalAreaEntered(PortalArea.PortalData data) {
		if (!this.playerData.hasMedallion) {
			return;
		}
		if (data.entityName == this.name) {
			this.medallionLight.color = data.color;
			this.medallionLight.intensity = 0.5f;
		}
	}

	public void onPortalAreaExited(string entityName) {
		if (!this.playerData.hasMedallion) {
			return;
		}

		if (entityName == this.name) {
			this.medallionLight.color = this.defaultMedallionLightColor;
			this.medallionLight.intensity = this.defaultMedallionLightIntensity;
		}
	}

	void OnDestroy(){
		GameEvents.current.onPortalAreaEntered -= onPortalAreaEntered;
		GameEvents.current.onPortalAreaExited -= onPortalAreaExited;
	}

	public Vector3 getMoveDirection(){
		return this.moveDirection;
	}
	public float getSpeed(){
		return this.speed;
	}
}
