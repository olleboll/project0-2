using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
	public float cameraSpeed = 1f;

	private Camera camera;
	private GameObject followTarget;
	private GameObject player;
	private UnityEngine.Tilemaps.Tilemap groundTilemap;
	private float cameraHalfWidth;
	private float cameraHalfHeight;
	private Vector2 maxTilemapBounds;
	private Vector2 minTilemapBounds;

	void Start()
	{
		this.camera = GetComponent<Camera>();
		this.player = GameObject.Find("Player");
		this.followTarget = this.player;
		this.groundTilemap = GameObject.Find("ground").GetComponent<UnityEngine.Tilemaps.Tilemap>();
		this.maxTilemapBounds = this.groundTilemap.localBounds.max;
		this.minTilemapBounds = this.groundTilemap.localBounds.min;
		this.cameraHalfHeight = Camera.main.orthographicSize;
		this.cameraHalfWidth = this.cameraHalfHeight * Screen.width / Screen.height;
	}

	void FixedUpdate(){
		Vector3 currentPosition = this.camera.transform.position;
		float interpolation = this.cameraSpeed * Time.fixedDeltaTime;
		Vector3 target = this.followTarget.GetComponent<Rigidbody2D>().position;

		Vector3 newCameraPosition = StaticHelperFunctions.getLerpedPosition(currentPosition, target, interpolation);

		newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, this.minTilemapBounds.x + this.cameraHalfWidth + 1, this.maxTilemapBounds.x - this.cameraHalfWidth - 2);
		newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, this.minTilemapBounds.y + this.cameraHalfHeight + 1, this.maxTilemapBounds.y - this.cameraHalfHeight - 2);
		newCameraPosition.z = -10;

		this.camera.transform.position = newCameraPosition;
	}
}
