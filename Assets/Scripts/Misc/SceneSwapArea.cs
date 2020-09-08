using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapArea : MonoBehaviour
{
	public string toScene;
	public bool shouldUseDefaultPlayerSpawn = true;
	public Vector3 positionInToScene;

	private PlayerData playerData;
	private SceneController sceneController;

	void Start(){
		this.playerData = Object.FindObjectOfType<PlayerData>();
		this.sceneController = Object.FindObjectOfType<SceneController>();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			if (shouldUseDefaultPlayerSpawn) {
				this.playerData.SetUniversePosition(this.playerData.defaultPlayerSpawn);
				this.sceneController.SwapScene(this.toScene);
			} else {
				this.playerData.SetUniversePosition(positionInToScene);
				this.sceneController.SwapScene(this.toScene);
			}
		}
	}
}
