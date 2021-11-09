using UnityEngine;
using System;

public class ThrowableObjectBehavior : MonoBehaviour
{

	[NonSerialized] public bool isGrabbed = false;
	bool isPhysicalWithPlayer = true;
	readonly float maxSpeedToBecomePhysical = 6;
	Rigidbody2D myRB;

	private void Awake()
	{
		myRB = gameObject.GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{
		myRB.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	}

	// Update is called once per frame
	void Update()
	{
		CheckToBecomePhysicalWithPlayer();
	}
	
	public void BecomeGrabbed()
	{
		isGrabbed = true;
		isPhysicalWithPlayer = false;
		//set proper collider layer
		Physics2D.IgnoreCollision(References.theHero.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
	}

	public void BecomeReleased()
	{
		isGrabbed = false;
	}

	void CheckToBecomePhysicalWithPlayer()
	{
		if (!isGrabbed && !isPhysicalWithPlayer && myRB.velocity.magnitude <= maxSpeedToBecomePhysical)
		{
			//go back to colliding with player
			Physics2D.IgnoreCollision(References.theHero.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), false);
			isPhysicalWithPlayer = true;
		}
	}
}
