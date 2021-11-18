using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

	readonly float cameraSpeed = 1;
	readonly float cameraSpeedBoost = 5;
	readonly float cameraVerticalOffset = 2f;
	SpriteRenderer herosSpriteRenderer;

	// Start is called before the first frame update
	void Start()
	{
		herosSpriteRenderer = References.theHero.GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		//
		MoveCamera();
	}

	void MoveCamera()
	{
		//save our lerped position
		Vector3 lerpedCameraPos = Vector2.Lerp(transform.position, References.theHero.transform.position, cameraSpeed * Time.deltaTime * (herosSpriteRenderer.isVisible ? 1 : cameraSpeedBoost));

		//apply our lerped position and apply camera offset
		transform.position = new Vector3(lerpedCameraPos.x, lerpedCameraPos.y + cameraVerticalOffset * Time.deltaTime, transform.position.z);
	}
}
