using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortoiseBehavior : MonoBehaviour
{
	//housekeeping
	GroundCheckScript myGroundChecker;

	//moving
	readonly float defaultStartDirection = -1;
	readonly float tortoiseTopSpeed = 2f;
	readonly float movementForce = 3000;
	readonly float changeDirectionStopThreshold = 0.8f;
	readonly float tortoiseFriction = 20;
	public float wishXMovement;
	Rigidbody2D myRB;
	Vector2 pushDir;
	Vector2 wishDir;

	//attacking
	readonly float horizontalSearchRange = 4;
	readonly float verticalSearchRange = 0.5f;
	readonly float chargeWindupTime = 0.8f;
	readonly float chargeSpeed = 15;
	readonly float bouncinessAfterCharge = 10;
	float timeSinceStoppedPatrolling = 0;
	bool isPatrolling = true;
	bool isCharging = false;

	//Timers
	float timeSinceDirectionChange = 0;
	float timeBeforeStopCheck = 0.3f;

	//Dying
	readonly float horizontalDeathFlingOffset = 10;
	readonly float verticalDeathFlingOffset = 10;
	readonly float rotationalDeathFlingOffset = 100;
	bool isDead = false;

	// Start is called before the first frame update
	void Start()
	{
		//get my rigidbody
		myRB = gameObject.GetComponent<Rigidbody2D>();

		//initialize wishDir
		wishDir = wishXMovement != 0 ? new Vector2(wishXMovement, 0) : new Vector2(defaultStartDirection, 0);

		//get my ground checker
		myGroundChecker = GetComponent<GroundCheckScript>();
	}

	// Update is called once per frame
	void Update()
	{
		Timers();
		Charge();
		if (isPatrolling)
		{
			ChangeDirection();
			ScanForPlayer();
		}
	}

	private void FixedUpdate()
	{
		if (isPatrolling)
		{
			Patrol();
			CounterSlope();
		}
	}

	void Timers()
	{
		if (timeSinceDirectionChange < timeBeforeStopCheck)
			timeSinceDirectionChange += Time.deltaTime;
		if (timeSinceStoppedPatrolling <= chargeWindupTime &&
			!isPatrolling &&
			!isCharging)
		{
			timeSinceStoppedPatrolling += Time.deltaTime;
			Debug.Log("nice");
		}
	}

	void CounterSlope()
	{
		if (myGroundChecker.currentGroundAngle != 0)
		{
			//declare a vector for our groun force
			Vector2 counterSlopeForce;

			//give our vector its upward component
			counterSlopeForce.y = Mathf.Abs(myGroundChecker.currentGround.x);

			//give our vector its lateral componen
			counterSlopeForce.x = myGroundChecker.currentGround.x < 0 ? myGroundChecker.currentGround.y : -myGroundChecker.currentGround.y;

			//give it the proper magnitude
			counterSlopeForce *= Mathf.Sin(Mathf.Deg2Rad * myGroundChecker.currentGroundAngle) * Physics2D.gravity.magnitude * myRB.mass;

			//apply the force
			myRB.AddForce(counterSlopeForce);
		}
	}

	void Patrol()
	{
		//reset pushDir
		pushDir = Vector2.zero;

		//set push direction to be our keyboard input
		if (Vector2.Dot(wishDir, myRB.velocity) < 0 || Mathf.Abs(myRB.velocity.magnitude) < tortoiseTopSpeed)
			pushDir = new Vector2(wishDir.x, 0) * movementForce * Time.deltaTime;

		//give us friction
		if (Vector2.Dot(wishDir, myRB.velocity) <= 0 && myRB.velocity.magnitude != 0)
			myRB.velocity = myRB.velocity.normalized *
							Mathf.Clamp((myRB.velocity.magnitude - tortoiseFriction * Time.deltaTime), 0f, Mathf.Infinity);

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

	void ScanForPlayer()
	{
		//save the distances to the player
		float verticalDistanceToPlayer = Mathf.Abs(References.theHero.transform.position.y - transform.position.y);
		float horizontalDistanceToPlayer = Mathf.Abs(References.theHero.transform.position.x - transform.position.x);

		//if we're in range, charge
		if (verticalDistanceToPlayer <= verticalSearchRange && horizontalDistanceToPlayer <= horizontalSearchRange && !isDead)
		{
			isPatrolling = false;
		}
	}
	
	void Charge()
	{

		//if still winding up
		if (!isPatrolling && !isCharging)		
		{
			//keep us still
			myRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
		}

		//charge if we've stopped patrolling for at least the windup time
		if (timeSinceStoppedPatrolling >= chargeWindupTime)
		{

			//(plays once at beginning of charge)
			if (!isCharging)
			{
				//tell everyone we're charging
				isCharging = true;

				//make us throwable
				gameObject.AddComponent<ThrowableObjectBehavior>();

				//unfreeze us
				myRB.constraints = RigidbodyConstraints2D.None;

				//launch us at player
				myRB.velocity = (References.theHero.transform.position - transform.position).normalized * chargeSpeed;
			}

		}
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//check if we're touching a stop point and change direction if we are (if we haven't too recently)
		if (collision.gameObject.layer == LayerMask.NameToLayer("TortoiseStopPoint") && isPatrolling)
		{
			//say we're changing direction
			timeSinceDirectionChange = 0;

			//stop dead so we don't go over the ledge
			myRB.velocity = new Vector2(0, myRB.velocity.y);

			//change direction
			wishDir = new Vector2(-wishDir.x, 0);
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
		//unfreeze in case we're frozen
		gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		
		//if we're throwable
		if (gameObject.GetComponent<ThrowableObjectBehavior>() != null)
		{
			//get dropped
			References.theHero.GetComponent<CursorBehavior>().DropIt();

			//destroy our Throwability
			Destroy(gameObject.GetComponent<ThrowableObjectBehavior>());
		}

		//disable collider
		GetComponent<Collider2D>().enabled = false;

		//give it a velocity and rotation
		myRB.velocity = new Vector2((impact.relativeVelocity.x < 0 ? -1 : 1) * horizontalDeathFlingOffset, verticalDeathFlingOffset);
		myRB.angularVelocity = rotationalDeathFlingOffset;

		//tell everyone we're dead
		isDead = true;
	}
}
