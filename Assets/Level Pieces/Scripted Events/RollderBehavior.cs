using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollderBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        //if the object entering our space is throwable
		if (collision.GetComponent<ThrowableObjectBehavior>() != null)
            Physics2D.IgnoreCollision(collision, gameObject.GetComponent<Collider2D>());
    }

}
