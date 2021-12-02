using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusBehavior : MonoBehaviour
{

	readonly Vector3 myVelocity = new Vector3(2, 0, 0);
	bool hasStartedMoving = false;
	Rigidbody2D myRB;

	// Start is called before the first frame update
	void Start()
	{
		myRB = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		Move();
	}

	void Move()
	{
		if (hasStartedMoving)
		{
			myRB.velocity = myVelocity;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject == References.theHero)
			hasStartedMoving = true;
	}

}
