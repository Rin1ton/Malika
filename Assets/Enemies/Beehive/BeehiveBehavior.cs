using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeehiveBehavior : MonoBehaviour
{

	readonly float beeForwardOffset = -0.1f;
	readonly float beeSpawnCoolDown = 1;
	readonly float waitToSpawnFirstBee = 0.75f;
	public GameObject beePrefab;
	Rigidbody2D myRB;
	SpriteRenderer mySR;
	bool seenYet = false;

	//Dying
	readonly float horizontalDeathFlingOffset = 7;
	readonly float verticalDeathFlingOffset = 10;
	readonly float rotationalDeathFlingOffset = 100;

	//timers
	float timeSinceSpawnedBee = Mathf.Infinity;

	// Start is called before the first frame update
	void Start()
	{
		//get myRB
		myRB = gameObject.GetComponent<Rigidbody2D>();

		//get mySR
		mySR = gameObject.GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		Timers();
		CheckIfSeenForFirstTime();
	}

	void Timers()
	{
		if (timeSinceSpawnedBee <= beeSpawnCoolDown)
			timeSinceSpawnedBee += Time.deltaTime;
	}

	void CheckIfSeenForFirstTime()
	{
		if (mySR.isVisible && !seenYet)
		{
			seenYet = true;
			StartCoroutine(DelayedAnger());
		}
	}

	public void Anger()
	{
		//spawn bee if we haven't just done it
		if (beePrefab != null && timeSinceSpawnedBee >= beeSpawnCoolDown)
		{
			//tell others we've spawned a bee
			timeSinceSpawnedBee = 0;

			//spawn bee
			GameObject thisBee = Instantiate(beePrefab, gameObject.transform, true);
			thisBee.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, beeForwardOffset);
		}
	}

	IEnumerator DelayedAnger()
	{
		yield return new WaitForSeconds(waitToSpawnFirstBee);
		Anger();
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

		if (collision.gameObject.GetComponent<HeroBehavior>() != null)
			Anger();
	}

	void Die(Collision2D impact)
	{
		//unfreeze our movement
		gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

		//disable collider
		GetComponent<Collider2D>().enabled = false;

		//give it a velocity and rotation
		myRB.velocity = new Vector2((impact.relativeVelocity.x < 0 ? -1 : 1) * horizontalDeathFlingOffset, verticalDeathFlingOffset);
		myRB.angularVelocity = rotationalDeathFlingOffset;
	}

}
