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
	readonly float barLetterBoxOffset = 2.75f;
	public GameObject myTopBar, myBottomBar;
	Vector3 myTopBarDefaultPosition, myBottomBarDefaultPosition;

	// Start is called before the first frame update
	void Start()
	{
		//get my bars' default position
		myTopBarDefaultPosition = myTopBar.transform.position;
		myBottomBarDefaultPosition = myBottomBar.transform.position;

		//get the Hero's sprite renderer
		herosSpriteRenderer = References.theHero.GetComponent<SpriteRenderer>();

		//set my position
		transform.position = new Vector3(References.theHero.transform.position.x + cameraHorizontalOffset, References.theHero.transform.position.y + cameraVerticalOffset, transform.position.z);
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
		Vector3 lerpedCameraPos = Vector2.Lerp(transform.position, 
												References.theHero.transform.position, 
												cameraSpeed * Time.deltaTime * (herosSpriteRenderer.isVisible ? 1 : cameraSpeedBoost));

		//apply our lerped position and apply camera offset
		transform.position = new Vector3(lerpedCameraPos.x + cameraHorizontalOffset * Time.deltaTime, 
											lerpedCameraPos.y + cameraVerticalOffset * Time.deltaTime, 
											transform.position.z);
	}

	void MoveLetterbox()
	{
		if (References.isInCutscene)
		{
			//get the position we should move the letterboxes to
			Vector3 moveTopBarTo = ((myTopBarDefaultPosition + new Vector3(0, -barLetterBoxOffset, 0)) - myTopBar.transform.position) * Time.deltaTime * 2 + myTopBar.transform.position;
			Vector3 moveBottomBarTo = ((myBottomBarDefaultPosition + new Vector3(0, +barLetterBoxOffset, 0)) - myBottomBar.transform.position) * Time.deltaTime * 2 + myBottomBar.transform.position;

			//move our letterboxes
			myTopBar.transform.position = moveTopBarTo;
			myBottomBar.transform.position = moveBottomBarTo;

			Debug.Log(myBottomBarDefaultPosition);

		}
		else
		{
			//get the position we should move the letterboxes to
			Vector3 moveTopBarTo = (myTopBarDefaultPosition - myTopBar.transform.position) * Time.deltaTime * 2 + myTopBar.transform.position;
			Vector3 moveBottomBarTo = (myBottomBarDefaultPosition - myBottomBar.transform.position) * Time.deltaTime  * 2 + myBottomBar.transform.position;

			//move our letterboxes
			myTopBar.transform.position = moveTopBarTo;
			myBottomBar.transform.position = moveBottomBarTo;
		}
	}

}
