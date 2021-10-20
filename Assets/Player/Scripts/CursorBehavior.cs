using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{

	//dumb
	public GameObject MyChest;

	readonly float aimAssistMagnitude = 0.2f;
	Vector2 cursorPositionInWorld;
	Collider2D[] collidersNearCursor;
	Rigidbody2D heldObjectRigidBody;
	Collider2D closestCollider;
	float timeSinceStartedHoldingTelekinesesButton = 0;

	//Jab
	readonly float jabForce = 10;
	readonly float maxTimeToHoldButtonForJab = 0.3f;

	//Grab
	readonly float maxGrabFollowSpeed = 450;

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
		//ObjectJab();
		ObjectGrabAndThrow();
	}

	private void FixedUpdate()
	{
		MoveGrabbedObjectToCursor();
	}

	void Timers()
	{
		if (Input.GetKey(References.telekinesesButton))
			timeSinceStartedHoldingTelekinesesButton += Time.deltaTime;
		if (Input.GetKeyDown(References.telekinesesButton))
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
			//as well as check to see if it has a throwableobject script
			if (distanceToClosestObject > Vector2.Distance(cursorPositionInWorld, collidersNearCursor[colliderBeingChecked].transform.position) && 
				collidersNearCursor[colliderBeingChecked].gameObject.GetComponent<ThrowableObjectBehavior>() != null)
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
		if (Input.GetKeyDown(References.telekinesesButton) && closestCollider != null)
			heldObjectRigidBody = closestCollider.gameObject.GetComponent<Rigidbody2D>();

		//check if we can we're clicking on a collider
		if (Input.GetKeyUp(References.telekinesesButton) && heldObjectRigidBody != null && timeSinceStartedHoldingTelekinesesButton < maxTimeToHoldButtonForJab)
		{
			//if our object has an RB, jab it
			if (heldObjectRigidBody != null)
				heldObjectRigidBody.velocity += new Vector2(0, jabForce);;

			//set our object back to null
			heldObjectRigidBody = null;
		}
	}

	void ObjectGrabAndThrow()
	{
		//do the grab (plays once at start of grab)
		if (Input.GetKeyDown(References.telekinesesButton) && closestCollider != null && heldObjectRigidBody == null)
		{
			//get our object's rigidbody
			heldObjectRigidBody = closestCollider.gameObject.GetComponent<Rigidbody2D>();
		}

		//let go if not holding the button anymore (plays once at the end of grab)
		if (Input.GetKeyUp(References.telekinesesButton) && heldObjectRigidBody != null)
		{
			heldObjectRigidBody = null;
		}
	}

	void MoveGrabbedObjectToCursor()
	{
		//move the object to the cursor constantly if we have an object
		if (heldObjectRigidBody != null && Input.GetKey(References.telekinesesButton))
		{
			//move the object to the cursor
			heldObjectRigidBody.velocity = (cursorPositionInWorld - (new Vector2(heldObjectRigidBody.transform.position.x, heldObjectRigidBody.transform.position.y)
			/*get rid of this bit when we have better objects*/ + heldObjectRigidBody.gameObject.GetComponent<Collider2D>().offset / 2)/**/) * maxGrabFollowSpeed * Time.fixedDeltaTime;
		}
	}
}