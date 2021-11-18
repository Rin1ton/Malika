using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalconBehavior : MonoBehaviour
{

	Rigidbody2D myRB;
	float myStartingYPosition;
	bool isAsleep = true;

	//Swooping
	readonly float minHorizontalDistanceToSwoop = 4.25f;
	Vector3 swoopPoint = new Vector3(0, 0, 1);					//as long as swoopPoint.z != 0, it isn't initialized with useful data
	Vector3 movementVector = new Vector3(-5, 0, 0);
	bool isSwooping = false;
	bool hasSwooped = false;

	//Dying
	readonly float horizontalDeathFlingOffset = 7;
	readonly float verticalDeathFlingOffset = 10;
	readonly float rotationalDeathFlingOffset = 100;
	readonly float minTimeOffCameraToDie = 0.2f;
	bool isDead = false;

	//Timers
	float timeSinceOffCamera = 0;

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
			if (!isDead)
			Swoop();
			Timers();
		}
	}

	void Timers()
	{
		if (!gameObject.GetComponent<SpriteRenderer>().isVisible)
			timeSinceOffCamera += Time.deltaTime;
		else
			timeSinceOffCamera = 0;
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
		if (!gameObject.GetComponent<SpriteRenderer>().isVisible && !isAsleep && timeSinceOffCamera > minTimeOffCameraToDie)
			; // Destroy(transform.parent.gameObject);
	}

	void Swoop()
	{
		//check if we should swoop
		if (!hasSwooped && Mathf.Abs(transform.position.x - References.theHero.transform.position.x) < minHorizontalDistanceToSwoop)
		{
			isSwooping = true;
			hasSwooped = true;
		}

		//if we should be swooping...
		if (isSwooping)
		{
			//if our swoopPoint isn't initialized, initialize it before we use it
			if (swoopPoint.z != 0)
				swoopPoint = References.theHero.transform.position;

			//figure out what our 'y' position should be
			//math is a bull, and I'm the matadore
			float newY = (((myStartingYPosition - swoopPoint.y) / minHorizontalDistanceToSwoop) / minHorizontalDistanceToSwoop) * Mathf.Pow(transform.position.x - swoopPoint.x, 2) + swoopPoint.y;

			//apply our position
			transform.position = new Vector3(transform.position.x, newY, 0);

			//
			isSwooping = Mathf.Abs(transform.position.x - swoopPoint.x) <= minHorizontalDistanceToSwoop;

		} else
		{
			//if not swooping, make sure we stay level
			transform.position = new Vector3(transform.position.x, myStartingYPosition, 0);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		TakeDamage(collision);
	}

	void TakeDamage(Collision2D collision)
	{
		//check if we're hit by something fast enough to take damage
		if (collision.relativeVelocity.magnitude >= References.throwableMinSpeedToKill)
			Die(collision);
	}

	void Die(Collision2D impact)
	{
		//tell everyone we're dead
		isDead = true;

		//give us a rigidbody
		Rigidbody2D myRB = gameObject.AddComponent<Rigidbody2D>();
		myRB.gravityScale = 1;

		//unfreeze our rotation
		gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

		//disable collider
		GetComponent<Collider2D>().enabled = false;

		//get dropped
		References.theHero.GetComponent<CursorBehavior>().DropIt();

		//destroy our Throwability
		Destroy(gameObject.GetComponent<ThrowableObjectBehavior>());

		//give it a velocity and rotation
		myRB.velocity = new Vector2((impact.relativeVelocity.x < 0 ? -1 : 1) * horizontalDeathFlingOffset, verticalDeathFlingOffset);
		myRB.angularVelocity = rotationalDeathFlingOffset;
	}

}
