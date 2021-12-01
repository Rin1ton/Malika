using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

	readonly float cameraSpeed = 1;
	readonly float cameraSpeedBoost = 5;
	readonly float cameraVerticalOffset = 1.6f;
	readonly float cameraHorizontalOffset = 1.8f;
	SpriteRenderer herosSpriteRenderer;

	//my bars
	public GameObject myTopBar, myBottomBar;
	float myTopBarDefaultYPosition;
	float myBottomBarDefaultYPosition;

	// Start is called before the first frame update
	void Start()
	{
		//get my bars' default position
		myTopBarDefaultYPosition = myTopBar.transform.position.y;
		myBottomBarDefaultYPosition = myBottomBar.transform.position.x;

		//get the Hero's sprite renderer
		herosSpriteRenderer = References.theHero.GetComponent<SpriteRenderer>();

		//set my position
		transform.position = new Vector2(References.theHero.transform.position.x + cameraHorizontalOffset, References.theHero.transform.position.y + cameraVerticalOffset);
	}

	// Update is called once per frame
	void Update()
	{
		MoveCamera();
		MoveLetterbox();
	}

	void MoveCamera()
	{
		//save our lerped position
		Vector3 lerpedCameraPos = Vector2.Lerp(transform.position, References.theHero.transform.position, cameraSpeed * Time.deltaTime * (herosSpriteRenderer.isVisible ? 1 : cameraSpeedBoost));

		//apply our lerped position and apply camera offset
		transform.position = new Vector3(lerpedCameraPos.x + cameraHorizontalOffset * Time.deltaTime, lerpedCameraPos.y + cameraVerticalOffset * Time.deltaTime, transform.position.z);
	}

	public void EngageLetterbox()
	{
		
	}

	public void DisengageLetterbox()
	{

	}

	void MoveLetterbox()
	{

	}

}
