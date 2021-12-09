using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRigBehavior : MonoBehaviour
{

	bool hasBeenGrabbed = false;
	bool hasToStop = false;
	bool isTouchingPlayer = false;
	Rigidbody2D myRB;
	Vector3 accelerationForce = new Vector3(2, 0, 0);

	// Start is called before the first frame update
	void Start()
	{
		myRB = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void FixedUpdate()
	{
		if (hasBeenGrabbed && !hasToStop)
			myRB.AddForce(accelerationForce);
	}

	public void GetGrabbed()
	{
		if (isTouchingPlayer)
		{
			hasBeenGrabbed = true;
			References.isInCutscene = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Pulled Objects Platforms"))
		{
			hasToStop = true;
			References.isInCutscene = false;
		}

		if (collision.gameObject == References.theHero)
			isTouchingPlayer = true;

	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject == References.theHero)
			isTouchingPlayer = false;
	}

}
