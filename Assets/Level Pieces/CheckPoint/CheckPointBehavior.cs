using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBehavior : MonoBehaviour
{

    public int myCheckpointOrder = 0;

    // Awake is called when the script instance is loaded
	private void Awake()
	{
        References.levelCheckpoints.Add(gameObject);
	}

	// Start is called before the first frame update
	void Start()
    {
        for (int checkpointBeingChecked = 0; checkpointBeingChecked < References.levelCheckpoints.Count; checkpointBeingChecked++)
		{

            GameObject thatOne = References.levelCheckpoints[checkpointBeingChecked];

            //if the one being checked is further left than us
            if (thatOne.gameObject.transform.position.x < gameObject.transform.position.x)
                myCheckpointOrder++;
		}

        //if, after checking the order of every checkpoint, we're still 0, then we must be the active checkpoint
        if (myCheckpointOrder == 0)
            References.activeCheckpoint = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        //if the hero touches us
		if (collision.gameObject.GetComponent<HeroBehavior>() != null)
		{
            //if the active checkpoint is further left than us
            if (References.activeCheckpoint.GetComponent<CheckPointBehavior>().myCheckpointOrder < myCheckpointOrder)
			{
                //make us the active checkpoint
                References.activeCheckpoint = gameObject;

                //MAKE A CHECKPOINT EFFECT AND TRIGGER IT HERE
                //MAKE A CHECKPOINT EFFECT AND TRIGGER IT HERE
                Debug.Log("new Checkpoint order: " + myCheckpointOrder);
			}
		}
	}

}
