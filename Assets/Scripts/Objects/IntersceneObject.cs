using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersceneObject : MonoBehaviour
{
	public string id;
	private IntersceneObjectController.ObjectData data;
	private IntersceneObjectController dataController;
	void Start()
	{
		if (this.id == null) {
			Debug.Log("No id supplied.. wont work");
			return;
		}
		this.dataController = Object.FindObjectOfType<IntersceneObjectController>();

		IntersceneObjectController.ObjectData _data = this.dataController.GetObjectData(this.id);
		if (_data.id == null) {
			this.data = new IntersceneObjectController.ObjectData(this.id, transform.position);
		} else {
			this.data = _data;
			transform.position = this.data.position;
		}
	}

	void Update()
	{
		if (id == null) {
			Debug.Log("No id supplied.. wont work");
			return;
		}


		this.data.position = transform.position;
		this.dataController.SetObjectData(this.data);
	}
}
