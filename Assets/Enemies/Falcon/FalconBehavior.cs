using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalconBehavior : MonoBehaviour
{

	readonly float minHorizontalDistanceToSwoop = 5;
	float myStartingYPosition;
	Vector3 swoopPoint = new Vector3(0, 0, 1);					//as long as swoopPoint.z != 0, it isn't initialized with useful data
	Vector3 movementVector = new Vector3(-5, 0, 0);
	bool isAsleep = true;
	bool isSwooping = false;

	// Start is called before the first frame update
	void Start()
	{
		myStartingYPosition = transform.position.y;
	}

	// Update is called once per frame
	void Update()
	{
		CheckIfOnCamera();
		if (!isAsleep)
		{
			Move();
			CheckIfOffCamera();
			Swoop();
		}
	}

	void CheckIfOnCamera()
	{
		if (gameObject.GetComponent<SpriteRenderer>().isVisible)
			isAsleep = false;
	}

	void Move()
	{
		transform.position += movementVector * Time.deltaTime;
	}

	void CheckIfOffCamera()
	{
		//if we've been on camera and we're not anymore, destroy our game object
		if (!gameObject.GetComponent<SpriteRenderer>().isVisible && !isAsleep)
			Destroy(transform.parent.gameObject);
	}

	void Swoop()
	{
		//check if we should swoop
		if (!isSwooping)
			isSwooping = Mathf.Abs(transform.position.x - References.theHero.transform.position.x) < minHorizontalDistanceToSwoop;

		//
		Debug.Log(Mathf.Abs(transform.position.x - References.theHero.transform.position.x));

		//if we should be swooping...
		if (isSwooping)
		{
			//if our swoopPoint isn't initialized, initialize it before we use it
			if (swoopPoint.z != 0)
				swoopPoint = References.theHero.transform.position;

			//figure out what our 'y' position should be
			float newY = ((myStartingYPosition - swoopPoint.y / minHorizontalDistanceToSwoop) / minHorizontalDistanceToSwoop) * Mathf.Pow(transform.position.x - swoopPoint.x, 2) + swoopPoint.y;

			//apply our position
			transform.position = new Vector3(transform.position.x, newY, 0);

			//
			isSwooping = Mathf.Abs(transform.position.x - swoopPoint.x) < minHorizontalDistanceToSwoop;
		} else
		{
			//if not swooping, make sure we stay level
			transform.position = new Vector3(transform.position.x, myStartingYPosition, 0);
		}
	}

}
