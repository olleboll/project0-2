using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaController : MonoBehaviour
{
	public EventName eventName;
	public enum EventName {
		PortalAreaEntered,
		TriggerEventEntered
	}

	// data for portal area
	public SceneController.WorldScene worldNames;

	// data to trigger game events
	public string triggerEvent;

	// void Start(){
	// 	GameEvents.current.onPortalAreaEntered += TestEvent;
	// }
	//
	// public void TestEvent(string data){
	// 	Debug.Log("I acted on trigered event");
	// 	Debug.Log(data);
	// 	Debug.Log(worldNames);
	//
	// 	List<string> selectedWorlds = new List<string>();
	//
	// 	for (int i = 0; i < System.Enum.GetValues(typeof(SceneController.WorldScene)).Length; i++) {
	// 		int layer = 1 << i;
	// 		string value = System.Enum.GetName(typeof(SceneController.WorldScene), layer);
	// 		if (((int) worldNames & layer) != 0 && value != null) {
	// 			selectedWorlds.Add(value);
	// 			Debug.Log(value);
	// 		}
	// 	}
	// }
	//
	// void OnTriggerEnter2D(Collider2D other){
	// 	Debug.Log("Collided!");
	// 	Debug.Log(other.name);
	// 	Debug.Log(eventName);
	// 	GameEvents.current.portalAreaEntered("some string of data depending on what kind of trigger area this is?");
	// }

}
