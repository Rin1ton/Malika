using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalconBehavior : MonoBehaviour
{

	bool isAsleep = true;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		CheckIfOnCamera();
	}

	void CheckIfOnCamera()
	{
		if (gameObject.GetComponent<SpriteRenderer>().isVisible)
			Debug.Log("myballse");
	}

}
