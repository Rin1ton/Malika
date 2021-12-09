using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBehavior : MonoBehaviour
{

	public Animator myAnimator;
	Rigidbody2D myRB;
	float minSpeedToRun = 0.1f;
	SpriteRenderer mySR;

	// Start is called before the first frame update
	void Start()
	{
		myRB = GetComponent<Rigidbody2D>();
		mySR = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Mathf.Abs(myRB.velocity.x) >= minSpeedToRun)
		{
			if (myRB.velocity.x < 0)
			{
				myAnimator.SetBool("isRunningLeft", true);
				myAnimator.SetBool("isRunningRight", false);
				mySR.flipX = true;
			}
			if (myRB.velocity.x > 0)
			{
				myAnimator.SetBool("isRunningLeft", false);
				myAnimator.SetBool("isRunningRight", true);
				mySR.flipX = false;
			}
		}
		else
		{
				myAnimator.SetBool("isRunningLeft", false);
				myAnimator.SetBool("isRunningRight", false);
		}
	}
}
