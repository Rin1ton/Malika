using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBehavior : MonoBehaviour
{

	readonly float minSpeedToRun = 0.1f;
	readonly float minTimeToNotBeGrounded = 0.085f;
	public Animator myAnimator;
	Rigidbody2D myRB;

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
		if (Mathf.Abs(References.playerMovement.x) >= minSpeedToRun)
		{
			if (myRB.velocity.x < 0)
			{
				myAnimator.SetBool("isRunning", true);
				mySR.flipX = true;
			}
			if (myRB.velocity.x > 0)
			{
				myAnimator.SetBool("isRunning", true);
				mySR.flipX = false;
			}
		}
		else
		{
			myAnimator.SetBool("isRunning", false);
		}
		if (References.playerTimeSinceGrounded > minTimeToNotBeGrounded)
		{
			if (References.playerMovement.y > 0)
			{
				myAnimator.SetBool("isRising", true);
				myAnimator.SetBool("isFalling", false);
			}
			if (References.playerMovement.y < 0)
			{
				myAnimator.SetBool("isRising", false);
				myAnimator.SetBool("isFalling", true);
			}
		}
		else
		{
			myAnimator.SetBool("isRising", false);
			myAnimator.SetBool("isFalling", false);
		}
	}

	public void TakeDamage()
	{
		myAnimator.Play("Take Damage");
	}

}
