using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthAndDamageBehavior : MonoBehaviour
{

	int heroHealth = 3;
	float timeSinceLastDamage = Mathf.Infinity;
	Rigidbody2D myRB;

	readonly float timeInvincibleAfterDamage = 1.25f;
	readonly float knockbackVerticalOffset = 2;
	readonly float knockbackVelocity = 11;


	private void Awake()
	{
		myRB = gameObject.GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{
		Debug.Log(heroHealth);
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
			//throw us
			myRB.velocity = ((gameObject.transform.position - collision.gameObject.transform.position).normalized + (Vector3.up * knockbackVerticalOffset)).normalized * knockbackVelocity;

			//damage us
			heroHealth--;
			Debug.Log(heroHealth);

			//tell others we've taken damage
			timeSinceLastDamage = 0;
		}
	}
}