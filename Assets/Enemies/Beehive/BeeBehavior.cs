using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBehavior : MonoBehaviour
{

	readonly float beeSpeed = 3;
	Rigidbody2D myRB;
	SpriteRenderer mySR;

	//Dying
	readonly float horizontalDeathFlingOffset = 5;
	readonly float verticalDeathFlingOffset = 7;
	readonly float rotationalDeathFlingOffset = 100;
	bool isDead = false;

	//sounds
	public AudioSource myIdleSound;
	public AudioSource myDeathSound;

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
		if (!isDead)
		{
			Move();
		}
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

	public void Die()
	{
		//disable collider
		GetComponent<Collider2D>().enabled = false;

		//play death and mute idel
		myDeathSound.Play();
		myIdleSound.mute = true;

		//enable gravity and give it a velocity and rotation
		myRB.gravityScale = 1;
		myRB.velocity = new Vector2((References.theHero.transform.position.x > transform.position.x ? -1 : 1) * horizontalDeathFlingOffset, verticalDeathFlingOffset);
		myRB.angularVelocity = rotationalDeathFlingOffset;

		//tell everyone we're dead
		isDead = true;
	}

	public void Die(Collision2D impact)
	{
		//disable collider
		GetComponent<Collider2D>().enabled = false;

		//play death and mute idel
		myDeathSound.Play();
		myIdleSound.mute = true;

		//enable gravity and give it a velocity and rotation
		myRB.gravityScale = 1;
		myRB.velocity = new Vector2((impact.relativeVelocity.x < 0 ? -1 : 1) * horizontalDeathFlingOffset, verticalDeathFlingOffset);
		myRB.angularVelocity = rotationalDeathFlingOffset;

		//tell everyone we're dead
		isDead = true;
	}

}
