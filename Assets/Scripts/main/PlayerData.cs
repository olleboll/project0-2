using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
	// Keep track of what abilities and powerups the player has picked up
	// Has access to Nomal Arrows? Dash? Double dash!?
	// Mostly a bunch of getters and setters
	// This should probably be set when we load our save

	public bool hasDash = false;
	public bool hasMegaDash = false;

	private Vector3 lastUniversePosition = new Vector3(-1000, 0, 0);
	private List<string> worldOptions = new List<string>();

	private Vector3 megaDashDirection;

	void Start(){
		// For now we'll just do this to enable dev mode
		hasDash = true;
		hasMegaDash = true;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public Vector3 GetLastUniversePosition()
	{
		return this.lastUniversePosition;
	}

	public void SetUniversePosition(Vector3 position)
	{
		this.lastUniversePosition = position;
	}

	public void setMegaDashDirection(Vector3 dir){
		this.megaDashDirection = dir;
	}

	public Vector3 getMegaDashDirection(){
		return this.megaDashDirection;
	}

	// This could be messed up if two area overlap?
	// Even if they are in different scenes maybe?
	public void SetPlayerTeleportOptions(List<string> worlds){
		Debug.Log("possible worlds");
		Debug.Log(worlds.Count);
		if (worlds.Count > 0) {
			if (worlds[0] != null) {
				Debug.Log(worlds[0]);
			}
		}
		this.worldOptions = worlds;
	}

	public List<string> GetTeleportOptions(){
		return this.worldOptions;
	}

	public void OnSceneLoaded(UnityEngine.SceneManagement.Scene _scene, LoadSceneMode _mode){
		this.worldOptions = new List<string>();
	}

}
