using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehavior : MonoBehaviour
{
	//Player Movement
	Vector2 wishDir;
	Rigidbody2D myRB;
	readonly float shortTimerStop = 120;
	Vector2 pushDir;

	//Grounding
	readonly float maxGroundAngle = 46;
	float currentGroundAngle = 0;
	Vector2 currentGround = Vector2.zero;
	bool isGrounded = false;
	float timeSinceGrounded = 0;

	//Running
	readonly float movementForce = 1500;
	readonly float playerTopSpeed = 6;
	readonly float playerFriction = 20;
	
	//Jump
	readonly float jumpHeight = 11;
	readonly float hardJumpCooldown = 0.045f;
	readonly float coyoteTime = 0.2f;
	bool jumpReady = true;
	float timeSinceLastJump = 0;

	//RocketBoots
	bool RocketBootsReady = true;


	private void Awake()
	{
		myRB = gameObject.GetComponent<Rigidbody2D>();

		References.theHero = gameObject;
	}

	// Start is called before the first frame update
	void Start()
	{
		pushDir = Vector2.zero;
	}

	// Update is called once per frame
	void Update()
	{
		Timers();
		Jump();
		RocketBoots();
		CheckIfGrounded();
	}

	private void FixedUpdate()
	{
		CounterSlope();
		Movement();
		ResetJump();
		MyLateUpdate();             //HAS TO BE LAST IN FIXED UPDATE
	}

	void Timers()
	{
		if (timeSinceLastJump < shortTimerStop)
			timeSinceLastJump += Time.deltaTime;
		if (timeSinceGrounded < shortTimerStop)
			timeSinceGrounded += Time.deltaTime;
	}

	void MyLateUpdate()
	{
		//reset every collision before we calculate collisions to accurately keep track of what we're touching
		currentGroundAngle = 0;
		currentGround = Vector2.zero;
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

	void Movement()
	{
		//get keyboard input
		wishDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
		
		//reset pushdir
		pushDir = Vector2.zero;

		//set push direction to be our keyboard input
		if (Vector2.Dot(wishDir, myRB.velocity) < 0 || Mathf.Abs(isGrounded ? myRB.velocity.magnitude : myRB.velocity.x) < playerTopSpeed)
			pushDir = new Vector2(wishDir.x, 0) * movementForce * Time.deltaTime;
		
		//give us friction
		if (Vector2.Dot(wishDir, myRB.velocity) <= 0 && myRB.velocity.magnitude != 0 && isGrounded)
			myRB.velocity = myRB.velocity.normalized * 
							Mathf.Clamp((myRB.velocity.magnitude - playerFriction * Time.deltaTime), 0f, Mathf.Infinity);

		//apply our force
		myRB.AddForce(pushDir);
	}

	void Jump()
	{
		//reset jump *before* we jump, if we're going to
		if (isGrounded && timeSinceLastJump >= hardJumpCooldown)
			jumpReady = true;

		//can't jump if we're in the air
		if (timeSinceGrounded >= coyoteTime && !isGrounded)
			jumpReady = false;

		//I love you Brianna :]
		//do a jump
		if (Input.GetKeyDown(References.jumpButton) && jumpReady)
		{
			myRB.velocity = new Vector2(myRB.velocity.x, jumpHeight);
			jumpReady = false;
			timeSinceLastJump = 0;
		}

	}

	void ResetJump()
	{
		//reset jump *before* we jump, if we're going to
		if (isGrounded && timeSinceLastJump >= hardJumpCooldown)
			jumpReady = true;
	}

	void RocketBoots()
	{
		//reset jump *before* we jump, if we're going to
		if (isGrounded && timeSinceLastJump >= hardJumpCooldown)
			RocketBootsReady = true;

		//I love you Brianna :]
		//do a jump
		if (Input.GetKeyDown(References.rocketBootsButton) && RocketBootsReady)
		{
			myRB.velocity = new Vector2(myRB.velocity.x, jumpHeight);
			RocketBootsReady = false;
			timeSinceLastJump = 0;
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
	
	//returns whether the player is on the ground or not, with the assistance of "GroundAndWallNormal()"
	void CheckIfGrounded()
	{
		bool grounded;

		//new way of checking grounded: when we touch the ground, save the collision,
		//and always check to see if *that* collision ever exits in "OnCollisionExit"
		grounded = currentGround != Vector2.zero;
		grounded &= timeSinceLastJump >= hardJumpCooldown;

		isGrounded = grounded;

		if (isGrounded)
			timeSinceGrounded = 0;
	}

}
