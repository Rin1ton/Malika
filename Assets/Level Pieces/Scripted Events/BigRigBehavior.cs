using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRigBehavior : MonoBehaviour
{

	bool hasBeenGrabbed = false;
	Rigidbody2D myRB;
	Vector3 accelerationForce = new Vector3(4, 0, 0);

	// Start is called before the first frame update
	void Start()
	{
		myRB = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void FixedUpdate()
	{
		if (hasBeenGrabbed)
			myRB.AddForce(accelerationForce);
	}

	public void GetGrabbed()
	{
		hasBeenGrabbed = true;
	}

}
