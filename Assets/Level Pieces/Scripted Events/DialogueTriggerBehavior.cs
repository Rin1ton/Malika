using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTriggerBehavior : MonoBehaviour
{

	AudioSource myAS;
	bool hasBeenTriggered = false;
	public float clipLength;
	public bool isLevelTransition;
	public string nextScene;

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

			StartCoroutine(TrackClipLength());
		}
	}
	IEnumerator TrackClipLength()
	{
		//wait to end the cutscene
		yield return new WaitForSeconds(clipLength);
		if (isLevelTransition)
		{
			References.activeCheckpoint = null;
			References.levelCheckpoints.Clear();
			SceneManager.LoadScene(nextScene);
		}
	}

}
