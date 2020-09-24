using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PrologueScene : MonoBehaviour
{

	public GameObject waitForDarkQuest;
	public GameObject globalLight;
	private QuestController.QuestData waitForDarkQuestData;
	private PrologueData sceneData;

	public string nightOrDay = "day";
	void Start()
	{
		this.sceneData = Object.FindObjectOfType<PrologueData>();
		this.waitForDarkQuestData =  this.sceneData.GetQuestData(this.waitForDarkQuest.GetComponent<QuestController>().GetType().Name);
		Debug.Log("STARTING PROLOGUE SCENE");

		if (this.waitForDarkQuestData.complete) {
			this.nightOrDay = "night";
		}
	}

	void Update(){
		if (this.nightOrDay == "night") {
			this.globalLight.GetComponent<Light2D>().intensity = 0.2f;
		} else {
			this.globalLight.GetComponent<Light2D>().intensity = 1f;
		}
	}

}
