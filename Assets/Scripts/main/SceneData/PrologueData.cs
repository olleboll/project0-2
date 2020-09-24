using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Save all data about the scenes.
// Quests mostly.
// For now let's only keep track if quests are completed or not.
public class PrologueData : MonoBehaviour
{
	private Dictionary<string, QuestController.QuestData> questData = new Dictionary<string, QuestController.QuestData>();

	public void SetQuestData(string key, QuestController.QuestData data){
		this.questData[key] = data;
	}

	public QuestController.QuestData GetQuestData(string key){
		QuestController.QuestData data;
		if (!this.questData.TryGetValue(key, out data)) {
			return new QuestController.QuestData();
		}
		return data;
	}
}
