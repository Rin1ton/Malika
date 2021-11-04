using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthAndDamageBehavior : MonoBehaviour
{


	int heroHealth;
	float timeSinceLastDamage = Mathf.Infinity;
	Rigidbody2D myRB;

	readonly int startingHeroHealth = 3;
	readonly float timeInvincibleAfterDamage = 0.8f;
	readonly float knockbackVerticalOffset = 2;
	readonly float knockbackVelocity = 11;


	private void Awake()
	{
		myRB = gameObject.GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{
		heroHealth = startingHeroHealth;
	}

	// Update is called once per frame
	void Update()
	{
		Timers();
	}

	void Timers()
	{
		if (timeSinceLastDamage < timeInvincibleAfterDamage)
			timeSinceLastDamage += Time.deltaTime;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<TortoiseBehavior>() != null ||
			collision.gameObject.GetComponent<GoombaBehavior>() != null)
			TakeDamage(collision);
	}

	void TakeDamage(Collision2D collision)
	{
		if (timeSinceLastDamage >= timeInvincibleAfterDamage)
		{
			//damage us
			heroHealth--;

			Debug.Log(heroHealth);

			if (heroHealth == 0)
			{
				Respawn();
				return;
			}

			//throw us
			myRB.velocity = ((gameObject.transform.position - collision.gameObject.transform.position).normalized + (Vector3.up * knockbackVerticalOffset)).normalized * knockbackVelocity;

			//tell others we've taken damage
			timeSinceLastDamage = 0;
		}
	}

	void Respawn()
	{
		heroHealth = startingHeroHealth;
		myRB.velocity = Vector2.zero;
		gameObject.transform.position = References.activeCheckpoint.transform.position;
	}

}