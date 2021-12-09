using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingCarBehavior : MonoBehaviour
{

	readonly float carSpeed = -12;
	bool hasStarted = false;
	SpriteRenderer mySR;
	Rigidbody2D myRB;
	

	// Start is called before the first frame update
	void Start()
	{
		mySR = GetComponent<SpriteRenderer>();
		myRB = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (mySR.isVisible && !hasStarted)
		{
			hasStarted = true;
			myRB.velocity = new Vector3(carSpeed, 0, 0);
		}

		if (hasStarted && !mySR.isVisible)
		{
			Destroy(gameObject);
		}
	}
}
