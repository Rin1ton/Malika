using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBehavior : MonoBehaviour
{

	Vector2 startPosition;
	float startZ;
	float startY;

	Vector2 travel => (Vector2)References.theCamera.transform.position - startPosition;


	float distanceFromSubject => transform.position.z - References.theHero.transform.position.z;
	float clippingPlane => (References.theCamera.transform.position.z + (distanceFromSubject > 0 ? References.theCamera.farClipPlane : References.theCamera.nearClipPlane));

	float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;

	Vector2 parallaxVector;

	// Start is called before the first frame update
	void Start()
	{
		startPosition = transform.position;
		startZ = transform.position.z;
		startY = transform.position.y;
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 newPos = startPosition + travel * parallaxFactor;
		transform.position = new Vector3(newPos.x, newPos.y, startZ);
	}
}
