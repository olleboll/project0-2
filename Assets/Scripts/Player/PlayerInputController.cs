﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
	public Vector3 moveDirection;
	public Vector3 aimDirection;
	public Vector3 swordDirection;

	public bool action;
	public bool dash;
	public bool chargeDash;

	private GameObject player;

	void Start(){
		this.player = GameObject.Find("Player");
	}

	void Update()
	{
		// This can be rewritten when I take the time to learn new input system properly.
		var gamepad = Gamepad.current;
		if (gamepad != null) {
			Vector2 move = gamepad.leftStick.ReadValue();
			this.moveDirection = new Vector3(move.x, move.y, 0.0f);
			this.moveDirection.Normalize();

			Vector2 aim = gamepad.rightStick.ReadValue();
			this.aimDirection = new Vector3(aim.x, aim.y, 0.0f);
			this.aimDirection.Normalize();

			this.action = gamepad.buttonWest.wasPressedThisFrame;
			this.dash = gamepad.buttonSouth.wasPressedThisFrame;
			this.chargeDash = gamepad.leftTrigger.isPressed;
			this.swordDirection = this.moveDirection;
		}else {

			Keyboard keyboard = Keyboard.current;

			float x = 0;
			float y = 0;

			if (keyboard.aKey.isPressed) {
				x--;
			}
			if (keyboard.dKey.isPressed) {
				x++;
			}
			if (keyboard.wKey.isPressed) {
				y++;
			}
			if (keyboard.sKey.isPressed) {
				y--;
			}

			Vector3 move = new Vector3(x,y,0);
			this.moveDirection = move;
			this.moveDirection.Normalize();

			// This is bugged :/
			Vector2 mouse = Mouse.current.position.ReadValue();
			Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 0));
			this.aimDirection = worldMousePosition - new Vector3(player.transform.position.x, player.transform.position.y,0);
			this.aimDirection.Normalize();


			this.action = Mouse.current.leftButton.wasPressedThisFrame;
			this.dash = keyboard.spaceKey.isPressed;
			this.swordDirection = this.aimDirection;
		}
	}
}