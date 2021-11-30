using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeehiveBehavior : MonoBehaviour
{

	readonly float beeForwardOffset = -0.1f;
	public GameObject beePrefab;
	bool isDead =  false;
	Rigidbody2D myRB;

	//Dying
	readonly float horizontalDeathFlingOffset = 7;
	readonly float verticalDeathFlingOffset = 10;
	readonly float rotationalDeathFlingOffset = 100;

	// Start is called before the first frame update
	void Start()
	{
		myRB = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void Anger()
	{
		if (beePrefab != null)
		{
			GameObject thisBee = Instantiate(beePrefab, gameObject.transform, true);
			thisBee.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, beeForwardOffset);
		}
		else
			Debug.LogError("No Bee Prefab!!!");
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
		//tell everyone we're dead
		isDead = true;

		//unfreeze our movement
		gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

		//disable collider
		GetComponent<Collider2D>().enabled = false;

		//give it a velocity and rotation
		myRB.velocity = new Vector2((impact.relativeVelocity.x < 0 ? -1 : 1) * horizontalDeathFlingOffset, verticalDeathFlingOffset);
		myRB.angularVelocity = rotationalDeathFlingOffset;
	}

}
