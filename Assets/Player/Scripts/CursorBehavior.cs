using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{

	readonly float aimAssistMagnitude = 0.20f;
	Vector2 cursorPositionInWorld;
	Collider2D[] collidersNearCursor;
	Rigidbody2D heldObjectRigidBody;
	Collider2D closestCollider;
	float timeSinceStartedHoldingTelekinesesButton = 0;

	//Jab
	readonly float jabForce = 10;
	readonly float maxTimeToHoldButtonForJab = 0.3f;

	//

	// Start is called before the first frame update
	void Start()
	{
		cursorPositionInWorld = Vector2.zero;
	}

	// Update is called once per frame
	void Update()
	{
		Timers();
		SetTelekinesesCursorPosition();			//has to happen before any telekineses moves
		ObjectJab();
		ObjectGrabAndThrow();
	}

	void Timers()
	{
		if (Input.GetKey(Controls.telekinesesButton))
			timeSinceStartedHoldingTelekinesesButton += Time.deltaTime;
		if (Input.GetKeyDown(Controls.telekinesesButton))
			timeSinceStartedHoldingTelekinesesButton = 0;
	}


	void SetTelekinesesCursorPosition()
	{
		//get our cursor's position, as well as everything near it in an array
		cursorPositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		collidersNearCursor = Physics2D.OverlapCircleAll(cursorPositionInWorld, aimAssistMagnitude);

		//prepare to sift through that array
		float distanceToClosestObject = Mathf.Infinity;
		closestCollider = null;

		//sift through that array
		for (int colliderBeingChecked = 0; colliderBeingChecked < collidersNearCursor.Length; colliderBeingChecked++)
		{
			//check if the object we're looking as is closer to the cursor than any other we've checked so far
			//as well as the collider is on the throwable object layer
			if (distanceToClosestObject > Vector2.Distance(cursorPositionInWorld, collidersNearCursor[colliderBeingChecked].transform.position) && 
				collidersNearCursor[colliderBeingChecked].gameObject.layer == LayerMask.NameToLayer("Throwable"))
			{
				//set our new closest object
				distanceToClosestObject = Vector2.Distance(cursorPositionInWorld, collidersNearCursor[colliderBeingChecked].transform.position);
				closestCollider = collidersNearCursor[colliderBeingChecked];
			}
		}
	}

	void ObjectJab()
	{
		//set our heldObject if we're clicking on something
		if (Input.GetKeyDown(Controls.telekinesesButton) && closestCollider != null)
			heldObjectRigidBody = closestCollider.gameObject.GetComponent<Rigidbody2D>();

		//check if we can we're clicking on a collider
		if (Input.GetKeyUp(Controls.telekinesesButton) && heldObjectRigidBody != null && timeSinceStartedHoldingTelekinesesButton < maxTimeToHoldButtonForJab)
		{
			//if our object has an RB, jab it
			if (heldObjectRigidBody != null)
				heldObjectRigidBody.velocity += new Vector2(0, jabForce);;

			//set our object back to null
			heldObjectRigidBody = null;
		}

		Debug.Log(timeSinceStartedHoldingTelekinesesButton < maxTimeToHoldButtonForJab);

	}

	void ObjectGrabAndThrow()
	{
		//start the grab
		if(Input.GetKey(Controls.telekinesesButton) && closestCollider != null)
		{
			heldObjectRigidBody = closestCollider.gameObject.GetComponent<Rigidbody2D>();
			heldObjectRigidBody.transform.position = cursorPositionInWorld;
		}
	}

}
