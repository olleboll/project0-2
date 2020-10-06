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

	public enum Ability {
		dash,
		megaDash,
		medallion,
		sickle,
		assistant,
	}
	public bool hasSickle = false;
	public bool hasMedallion = false;
	public bool hasDash = false;
	public bool hasMegaDash = false;
	public bool hasAssistant = false;
	public int playerHealth;
	public int playerMaxHealth;

	// This is bad.
	// This cannot be hardcoded.
	// I guess some scenes may change this?
	// It needs to be under pretty strict control though.
	public string worldLocationAsPrefix = "prologue_";

	public Vector3 defaultPlayerSpawn = new Vector3(-1000, -1000, -1000);
	private Vector3 lastUniversePosition;
	private List<string> worldOptions = new List<string>();

	private Vector3 megaDashDirection;
	private bool isTeleporting;
	private Vector3 lastPlayerMovement = Vector3.zero;

	void Start(){
		lastUniversePosition = this.defaultPlayerSpawn;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void EnableAbility(Ability ability){
		// It's not elegant bt it works
		if (ability == Ability.dash) {
			this.hasDash = true;
		}
		if (ability == Ability.megaDash) {
			this.hasMegaDash = true;
		}
		if (ability == Ability.medallion) {
			this.hasMedallion = true;
		}
		if (ability == Ability.sickle) {
			this.hasSickle = true;
		}
		if (ability == Ability.assistant) {
			this.hasAssistant = true;
		}
	}

	public bool hasAbility(Ability ability){
		if (ability == Ability.dash) {
			return this.hasDash;
		}
		if (ability == Ability.megaDash) {
			return this.hasMegaDash;
		}
		if (ability == Ability.sickle) {
			return this.hasSickle;
		}
		if (ability == Ability.assistant) {
			return this.hasAssistant;
		}
		return false;
	}

	public string GetWorldLocationPrefix(){
		return this.worldLocationAsPrefix;
	}
	public void SetWorldLocationPrefix(string prefix){
		this.worldLocationAsPrefix = prefix;
	}

	public Vector3 GetLastPlayerMovement(){
		return this.lastPlayerMovement;
	}
	public void SetLastPlayerMovement(Vector3 lastPlayerMovement){
		this.lastPlayerMovement = lastPlayerMovement;
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

	public void setTeleporting(bool isTeleporting){
		this.isTeleporting = isTeleporting;
	}

	public bool getTeleporting(){
		return this.isTeleporting;
	}

	// This could be messed up if two area overlap?
	// Even if they are in different scenes maybe?
	public void SetPlayerTeleportOptions(List<string> worlds){
		this.worldOptions = worlds;
	}

	public List<string> GetTeleportOptions(){
		return this.worldOptions;
	}

	public void OnSceneLoaded(UnityEngine.SceneManagement.Scene _scene, LoadSceneMode _mode){
		// Clear the teleport options.
		this.worldOptions = new List<string>();
	}

}
