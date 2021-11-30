using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBehavior : MonoBehaviour
{

	readonly float beeSpeed = 2;
	Rigidbody2D myRB;
	SpriteRenderer mySR;

	//Dying
	readonly float horizontalDeathFlingOffset = 7;
	readonly float verticalDeathFlingOffset = 10;
	readonly float rotationalDeathFlingOffset = 100;

	// Start is called before the first frame update
	void Start()
	{
		//get my RB
		myRB = gameObject.GetComponent<Rigidbody2D>();

		//get my SR
		mySR = gameObject.GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		Move();
		CheckIfOffCamera();
	}

	void Move()
	{
		myRB.velocity = (References.theHero.transform.position - transform.position).normalized * beeSpeed;
	}

	void CheckIfOffCamera()
	{
		//if off camera, die
		if (!mySR.isVisible)
			Destroy(gameObject);
	}

	public void Die()
	{
		//disable collider
		GetComponent<Collider2D>().enabled = false;

		//give it a velocity and rotation
		myRB.velocity = new Vector2((References.theHero.transform.position.x > transform.position.x ? -1 : 1) * horizontalDeathFlingOffset, verticalDeathFlingOffset);
		myRB.angularVelocity = rotationalDeathFlingOffset;
	}

}
