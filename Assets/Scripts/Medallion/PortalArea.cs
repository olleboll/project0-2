using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalArea : MonoBehaviour
{

	// data for portal area
	public SceneController.WorldScene worldNames;
	public Color color = Color.white;

	private List<string> listOfWorlds = new List<string>();

	void Start(){
		GameEvents.current.onPortalAreaEntered += PlayerEnteredThisPortal;
		GameEvents.current.onPortalAreaExited += PlayerExitedThisPortal;

		for (int i = 0; i < System.Enum.GetValues(typeof(SceneController.WorldScene)).Length; i++) {
			int layer = 1 << i;
			string value = System.Enum.GetName(typeof(SceneController.WorldScene), layer);
			if (((int) worldNames & layer) != 0 && value != null) {
				listOfWorlds.Add(value);
			}
		}
	}

	public struct PortalData {
		public string entityName;
		public List<string> possibleWorlds;
		public Color color;
	}

	public void PlayerEnteredThisPortal(PortalData data){
		if (data.entityName == "Player") {
			Object.FindObjectOfType<PlayerData>().SetPlayerTeleportOptions(listOfWorlds);
		}
	}

	public void PlayerExitedThisPortal(string entityName){
		if (entityName == "Player") {
			Object.FindObjectOfType<PlayerData>().SetPlayerTeleportOptions(new List<string>());
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		PortalData data = new PortalData();
		data.entityName = other.name;
		data.possibleWorlds = this.listOfWorlds;
		data.color = this.color;
		GameEvents.current.portalAreaEntered(data);
	}

	void OnTriggerExit2D(Collider2D other){
		GameEvents.current.portalAreaExited(other.name);
	}
}
