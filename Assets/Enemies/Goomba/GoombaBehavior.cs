using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaBehavior : MonoBehaviour
{

	//moving
	readonly float defaultStartDirection = -1;
	readonly float goombaTopSpeed = 3f;
	readonly float movementForce = 1500;
	readonly float changeDirectionStopThreshold = 0.2f;
	readonly float goombaFriction = 20;
	public float wishXMovement;
	Rigidbody2D myRB;
	Vector2 pushDir;
	Vector2 wishDir;

	float maxGroundAngle = 46;
	float currentGroundAngle = 0;
	Vector2 currentGround = Vector2.zero;

	//Timers
	float timeSinceDirectionChange = 0;
	float timeBeforeStopCheck = 0.8f;

	//Dying
	readonly float horizontalDeathFlingOffset = 10;
	readonly float verticalDeathFlingOffset = 10;
	readonly float rotationalDeathFlingOffset = 100;

	// Start is called before the first frame update
	void Start()
	{
		//get my rigidbody
		myRB = gameObject.GetComponent<Rigidbody2D>();

		//initialize wishDir
		wishDir = wishXMovement != 0 ? new Vector2(wishXMovement, 0) : new Vector2(defaultStartDirection, 0);
	}

	// Update is called once per frame
	void Update()
	{
		Timers();
		ChangeDirection();
	}

	private void FixedUpdate()
	{
		Move();
		CounterSlope();
	}

	void Timers()
	{
		if (timeSinceDirectionChange < timeBeforeStopCheck)
			timeSinceDirectionChange += Time.deltaTime;
	}

	void CounterSlope()
	{
		if (currentGroundAngle != 0)
		{
			//declare a vector for our groun force
			Vector2 counterSlopeForce;

			//give our vector its upward component
			counterSlopeForce.y = Mathf.Abs(currentGround.x);

			//give our vector its lateral componen
			counterSlopeForce.x = currentGround.x < 0 ? currentGround.y : -currentGround.y;

			//give it the proper magnitude
			counterSlopeForce *= Mathf.Sin(Mathf.Deg2Rad * currentGroundAngle) * Physics2D.gravity.magnitude * myRB.mass;

			//apply the force
			myRB.AddForce(counterSlopeForce);
		}
	}

	void Move()
	{
		//reset pushDir
		pushDir = Vector2.zero;

		//set push direction to be our keyboard input
		if (Vector2.Dot(wishDir, myRB.velocity) < 0 || Mathf.Abs(myRB.velocity.magnitude) < goombaTopSpeed)
			pushDir = new Vector2(wishDir.x, 0) * movementForce * Time.deltaTime;

		//give us friction
		if (Vector2.Dot(wishDir, myRB.velocity) <= 0 && myRB.velocity.magnitude != 0)
			myRB.velocity = myRB.velocity.normalized *
							Mathf.Clamp((myRB.velocity.magnitude - goombaFriction * Time.deltaTime), 0f, Mathf.Infinity);

		//apply push force
		myRB.AddForce(pushDir);
	}

	void ChangeDirection()
	{
		//check that we're stopped and we haven't changed direction recently before we do it
		if (myRB.velocity.magnitude < changeDirectionStopThreshold && timeSinceDirectionChange >= timeBeforeStopCheck)
		{
			//say that we're changing direction
			timeSinceDirectionChange = 0;

			//change direction
			wishDir = new Vector2(-wishDir.x, 0);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		GroundNormal(collision);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		GroundNormal(collision);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		TakeDamage(collision);
	}

	void GroundNormal(Collision2D other)
	{
		//see if any of the contacts of this collision are shallow enough to be the ground
		float lowestNormalAngle = 180;
		Vector2 lowestNormal = Vector2.zero;
		for (int k = 0; k < other.contactCount; k++)
		{
			Vector2 normal = other.contacts[k].normal;
			if (Vector2.Angle(Vector2.up, normal) < lowestNormalAngle)
			{
				lowestNormalAngle = Vector2.Angle(Vector2.up, normal);
				lowestNormal = normal;
			}
		}

		//if any part of this collision is shallow enough to be the ground, then this is the collision with the ground
		if (lowestNormalAngle <= maxGroundAngle)
		{
			currentGroundAngle = lowestNormalAngle;
			currentGround = lowestNormal;
		}

	}

	void TakeDamage(Collision2D collision)
	{
		//check if we're hit by something fast enough to take damage
		if(collision.relativeVelocity.magnitude >= References.throwableMinSpeedToKill)
		{
			//get a reference to the object that hit us
			ThrowableObjectBehavior throwableObjectThatHitMe = collision.collider.gameObject.GetComponent<ThrowableObjectBehavior>();

			//if that object is, in fact, throwable...
			if (throwableObjectThatHitMe != null)
			{
				Die(collision);
			}
		}
	}

	void Die(Collision2D impact)
	{
		//disable collider
		GetComponent<Collider2D>().enabled = false;

		//disable gravity on RB and give it a velocity and rotation
		myRB.velocity = impact.relativeVelocity * horizontalDeathFlingOffset + Vector2.up * verticalDeathFlingOffset;
		myRB.angularVelocity = rotationalDeathFlingOffset;
	}
}
