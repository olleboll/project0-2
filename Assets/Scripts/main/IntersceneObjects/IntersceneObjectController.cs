using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersceneObjectController : MonoBehaviour
{

	public struct ObjectData {
		public string id;
		public Vector3 position;
		public ObjectData(string id, Vector3 pos) {
			this.id = id;
			this.position = pos;
		}
	}

	private Dictionary<string, ObjectData> intersceneObjects;
	void Start()
	{
		this.intersceneObjects = new Dictionary<string, ObjectData>();
	}

	public void SetObjectData(ObjectData data) {
		if (this.intersceneObjects.ContainsKey(data.id)) {
			this.intersceneObjects[data.id] = data;
			return;
		}
		this.intersceneObjects.Add(data.id, data);
	}

	public ObjectData GetObjectData(string id) {
		Debug.Log("Getting data");
		if (this.intersceneObjects.ContainsKey(id)) {
			Debug.Log(this.intersceneObjects[id]);
			return this.intersceneObjects[id];
		}
		return default;
	}

}
