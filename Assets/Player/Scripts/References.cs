using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class References
{
	//controls
	readonly public static KeyCode jumpButton = KeyCode.Space;
	readonly public static KeyCode telekinesesButton = KeyCode.Mouse0;
	readonly public static KeyCode rocketBootsButton = KeyCode.W;
	public static bool isInCutscene = false;

	//
	readonly public static float throwableMinSpeedToKill = 22;

	//
	public static GameObject theHero;
	public static Camera theCamera;

	//
	public static List<GameObject> levelCheckpoints = new List<GameObject>();
	public static GameObject activeCheckpoint;
	public static GameObject dollyCameraPosition;

	//animation
	public static Vector3 playerMovement;
}
