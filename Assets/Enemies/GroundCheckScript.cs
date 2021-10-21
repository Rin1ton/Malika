using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
	[System.NonSerialized] public float maxGroundAngle = 46;
	[System.NonSerialized] public float currentGroundAngle = 0;
	[System.NonSerialized] public Vector2 currentGround = Vector2.zero;
	[System.NonSerialized] public bool isGrounded = false;

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

		isGrounded = grounded;
	}
}
