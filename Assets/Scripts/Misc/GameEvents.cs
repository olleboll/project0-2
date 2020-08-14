using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
	public static GameEvents current;

	void Awake(){
		current = this;
	}

	public event Action<PortalArea.PortalData> onPortalAreaEntered;
	public void portalAreaEntered(PortalArea.PortalData data){
		if (onPortalAreaEntered != null) {
			onPortalAreaEntered(data);
		}
	}

	public event Action<string> onPortalAreaExited;
	public void portalAreaExited(string data) {
		if (onPortalAreaExited != null) {
			onPortalAreaExited(data);
		}
	}
}
