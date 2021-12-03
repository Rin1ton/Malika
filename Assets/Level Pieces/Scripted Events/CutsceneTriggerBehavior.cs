using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTriggerBehavior : MonoBehaviour
{
	//tweakables
	public float audioPlayDelay;
	public float cutsceneLength;
	//my stop sign thing, if it exists
	public GameObject myStopSign;

	AudioSource myAS;
	bool hasBeenTriggered = false;

	// Start is called before the first frame update
	void Start()
	{
		//get our Audio source
		myAS = gameObject.GetComponent<AudioSource>();

		//if our cutscene length is 0, something must be wrong
		if (cutsceneLength == 0)
			Debug.LogError("THIS CUTSCENE HAS NO LENGTH");
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == References.theHero && !hasBeenTriggered)
		{
			//remember we've been triggered
			hasBeenTriggered = true;

			//lock the player
			References.isInCutscene = true;

			//start our length and audio play delay coroutines
			StartCoroutine(TrackAudioPlayDelay());
			StartCoroutine(TrackCutsceneLength());

			//give our stopsign throwability if we have one
			if (myStopSign != null)
				myStopSign.AddComponent<ThrowableObjectBehavior>();
		}
	}

	IEnumerator TrackCutsceneLength()
	{
		//wait to end the cutscene
		yield return new WaitForSeconds(cutsceneLength);
		EndCutscene();
	}

	IEnumerator TrackAudioPlayDelay()
	{
		//begin the audio when it's been long enough
		yield return new WaitForSeconds(audioPlayDelay);
		myAS.Play();
	}

	void EndCutscene()
	{
		//unlock the player
		References.isInCutscene = false;
	}

}
