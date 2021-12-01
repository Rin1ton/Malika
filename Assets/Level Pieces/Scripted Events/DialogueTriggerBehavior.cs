using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerBehavior : MonoBehaviour
{

	AudioSource myAS;
	bool hasBeenTriggered = false;

	// Start is called before the first frame update
	void Start()
	{
		myAS = gameObject.GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == References.theHero && !hasBeenTriggered)
		{
			myAS.Play();
			hasBeenTriggered = true;
		}
	}
}
