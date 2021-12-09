using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{

	readonly float aimAssistMagnitude = 0.2f;
	[System.NonSerialized] public Rigidbody2D heldObjectRigidBody;
	Vector2 cursorPositionInWorld;
	Collider2D[] collidersNearCursor;
	Collider2D closestCollider;

	//Grab
	readonly float maxGrabFollowSpeed = 525;

	// Start is called before the first frame update
	void Start()
	{
		cursorPositionInWorld = Vector2.zero;
	}

	// Update is called once per frame
	void Update()
	{
		if (!References.isInCutscene)
		{
			SetTelekinesesCursorPosition();			//has to happen before any telekineses moves
			ObjectGrabAndThrow();
		}
	}

	private void FixedUpdate()
	{
		if (!References.isInCutscene)
			MoveGrabbedObjectToCursor();
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
				collidersNearCursor[colliderBeingChecked].gameObject.GetComponent<Rigidbody2D>() != null)
			{
				//set our new closest object
				distanceToClosestObject = Vector2.Distance(cursorPositionInWorld, collidersNearCursor[colliderBeingChecked].transform.position);
				closestCollider = collidersNearCursor[colliderBeingChecked];
			}
		}
	}

	void ObjectGrabAndThrow()
	{
		//do the grab (plays once at start of grab)
		if (Input.GetKeyDown(References.telekinesesButton) && closestCollider != null && heldObjectRigidBody == null)
		{
			//get our object's rigidbody
			heldObjectRigidBody = closestCollider.gameObject.GetComponent<Rigidbody2D>();
			
			//tell object it's been grabbed
			if (heldObjectRigidBody.GetComponent<ThrowableObjectBehavior>() != null)
			{
				heldObjectRigidBody.GetComponent<ThrowableObjectBehavior>().BecomeGrabbed();
			}

			//if it's a beehive, anger it and lose the reference
			if (heldObjectRigidBody.GetComponent<BeehiveBehavior>() != null)
			{
				heldObjectRigidBody.GetComponent<BeehiveBehavior>().Anger();
				heldObjectRigidBody = null;
			}
			//if it's a bee, kill it then lose the reference
			else if (heldObjectRigidBody.GetComponent<BeeBehavior>() != null)
			{
				heldObjectRigidBody.GetComponent<BeeBehavior>().Die();
				heldObjectRigidBody = null;
			}

		}

		//let go if not holding the button anymore (plays once at the end of grab)
		if (Input.GetKeyUp(References.telekinesesButton) && heldObjectRigidBody != null)
		{
			//tell object it's been released
			if (heldObjectRigidBody.GetComponent<ThrowableObjectBehavior>() != null)
				heldObjectRigidBody.GetComponent<ThrowableObjectBehavior>().BecomeReleased();

			//reset held object to null
			heldObjectRigidBody = null;
		}
	}

	void MoveGrabbedObjectToCursor()
	{
		//move the object to the cursor constantly if we have an object
		if (heldObjectRigidBody != null && Input.GetKey(References.telekinesesButton))
		{
			if (heldObjectRigidBody.GetComponent<ThrowableObjectBehavior>() != null)
			{
				//move the object to the cursor
				heldObjectRigidBody.velocity = (cursorPositionInWorld - (new Vector2(heldObjectRigidBody.transform.position.x, heldObjectRigidBody.transform.position.y)
				/*get rid of this bit when we have better objects*/ + heldObjectRigidBody.gameObject.GetComponent<Collider2D>().offset / 2)/**/) * maxGrabFollowSpeed * Time.fixedDeltaTime;
			}
		}
	}

	public void DropIt()
	{
		//drop the thing if it can be dropped
		if (heldObjectRigidBody != null && heldObjectRigidBody.gameObject.GetComponent<ThrowableObjectBehavior>() != null)
			heldObjectRigidBody.gameObject.GetComponent<ThrowableObjectBehavior>().BecomeReleased();
		heldObjectRigidBody = null;
	}
}