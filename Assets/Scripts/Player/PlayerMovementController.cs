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
	private bool isDashing = false;
	private bool isMegaDashing = false;
	private float defaultSpeed;
	private DashController dashController;
	private MegaDashController megaDashController;
	private SwordController swordController;
	private Rigidbody2D body;
	private UnityEngine.Tilemaps.Tilemap map;
	private Light2D light;
	private float lightStartRadius;

	private Light2D medallionLight;
	private Color defaultMedallionLightColor;
	private float defaultMedallionLightIntensity;

	private PlayerData playerData;
	private SceneController sceneController;

	private bool performMegaDashOnNextUpdate = false;

	private float cameraHalfWidth;
	private float cameraHalfHeight;
	private Vector2 maxTilemapBounds;
	private Vector2 minTilemapBounds;

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
		this.map = GameObject.Find("Tilemap").GetComponent<UnityEngine.Tilemaps.Tilemap>();

		this.light = transform.Find("light").GetComponent<Light2D>();
		this.lightStartRadius = this.light.pointLightOuterRadius;

		this.medallionLight = transform.Find("MedallionLight").GetComponent<Light2D>();
		this.defaultMedallionLightColor = this.medallionLight.color;
		this.defaultMedallionLightIntensity = this.medallionLight.intensity;

		this.playerData = Object.FindObjectOfType<PlayerData>();
		this.sceneController = Object.FindObjectOfType<SceneController>();

		GameEvents.current.onPortalAreaEntered += onPortalAreaEntered;
		GameEvents.current.onPortalAreaExited += onPortalAreaExited;

		Vector3 startPosition = this.playerData.GetLastUniversePosition();
		if (startPosition.x > -999f) {
			transform.position = startPosition;
			Camera.main.transform.position = new Vector3(startPosition.x, startPosition.y, -10);
		}

		if (this.playerData.getMegaDashDirection().magnitude > 0) {
			this.performMegaDashOnNextUpdate = true;
		}

		this.maxTilemapBounds = this.map.localBounds.max;
		this.minTilemapBounds = this.map.localBounds.min;

		this.cameraHalfHeight = Camera.main.orthographicSize;
		this.cameraHalfWidth = this.cameraHalfHeight * Screen.width / Screen.height;
	}

	// Update is called once per frame
	void Update()
	{
		if (this.currentHealth <= 0) {
			this.currentHealth = 1000;
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
			return;
		}

		if (this.input.teleport) {
			// No user interaction while fiddling with the medallion
			return;
		}

		if (!this.dashController.isDashing && !this.megaDashController.isDashing && !(this.chargeDashTimer > 0f)) {
			this.handleMovement();
		}

		this.handleDash();
		this.handleMegaDash();
		this.handleSword();
		this.handleCamera();
	}

	private void handleMovement(){
		Vector3 moveDirection = this.input.moveDirection;
		float currentSpeed = moveDirection.magnitude;

		float newX = transform.position.x + moveDirection.x * speed * Time.deltaTime;
		float newY = transform.position.y + moveDirection.y * speed * Time.deltaTime;

		var grid = map.layoutGrid;
		var tilePositionX = grid.WorldToCell(new Vector3(newX, transform.position.y, 0));
		var tileX = map.GetTile(tilePositionX);

		var tilePositionY = grid.WorldToCell(new Vector3(transform.position.x, newY, 0));
		var tileY = map.GetTile(tilePositionY);

		if (tileX == null || tileX.name == "AbyssGrass" || tileX.name == "Water") {
			newX = transform.position.x;
		}

		if (tileY == null || tileY.name == "AbyssGrass" || tileY.name == "Water") {
			newY = transform.position.y;
		}

		Vector3 newPosition = new Vector3(newX, newY, 0);

		transform.position = newPosition;

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

		this.playerData.SetUniversePosition(transform.position);
	}

	private float chargeDashTimer = 0f;
	private void handleMegaDash(){

		if (this.performMegaDashOnNextUpdate) {
			this.megaDashController.dash(this.playerData.getMegaDashDirection());
			this.performMegaDashOnNextUpdate = false;
		}

		if (chargeDashTimer >= 1f && !this.input.chargeDash) {
			this.chargeDashTimer = 0f;
			this.megaDashController.dash(this.input.aimDirection);
		} else if (chargeDashTimer < 1f && this.input.chargeDash) {
			this.chargeDashTimer += 1 * Time.deltaTime;
		} else if (!this.input.chargeDash) {
			this.chargeDashTimer = 0f;
		}

		if (this.megaDashController.isDashing && this.input.dash) {
			this.megaDashController.stopDash();
		}
	}

	private void handleDash(){
		if (!this.isDashing && this.input.dash && this.dashController != null && !this.megaDashController.isDashing) {
			this.dashController.dash(this.input.moveDirection);
		}
	}

	private void handleSword(){
		bool swing = this.input.action;

		if (swing && this.swordController != null && !this.swordController.isSwinging()) {
			this.swordController.swing(this.input.swordDirection);
			this.speed += this.attackDashSpeed;
		} else if (this.swordController.isSwinging()) {
			// Do nothing here?
		} else {
			this.speed = this.defaultSpeed;
		}
	}

	private void handleCamera(){
		Vector3 currentPosition = Camera.main.transform.position;

		float interpolation = this.cameraSpeed * Time.deltaTime;

		float minX = currentPosition.x;
		float maxX = this.transform.position.x;
		float newX = Mathf.Lerp(minX, maxX, interpolation);

		float minY = currentPosition.y;
		float maxY = this.transform.position.y;
		float newY = Mathf.Lerp(minY, maxY, interpolation);

		newX = Mathf.Clamp(newX, this.minTilemapBounds.x + this.cameraHalfWidth, this.maxTilemapBounds.x - this.cameraHalfWidth);
		newY = Mathf.Clamp(newY, this.minTilemapBounds.y + this.cameraHalfHeight, this.maxTilemapBounds.y - this.cameraHalfHeight);

		Camera.main.transform.position = new Vector3(newX, newY, -10);
	}

	public void onPortalAreaEntered(PortalArea.PortalData data) {
		if (data.entityName == this.name) {
			this.medallionLight.color = data.color;
			this.medallionLight.intensity = 0.5f;
		}
	}

	public void onPortalAreaExited(string entityName) {
		if (entityName == this.name) {
			this.medallionLight.color = this.defaultMedallionLightColor;
			this.medallionLight.intensity = this.defaultMedallionLightIntensity;
		}
	}

	void OnDestroy(){
		GameEvents.current.onPortalAreaEntered -= onPortalAreaEntered;
		GameEvents.current.onPortalAreaExited -= onPortalAreaExited;
	}
}
