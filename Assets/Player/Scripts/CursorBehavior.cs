using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{

    Vector2 cursorPositionInWorld;
    readonly float aimAssistMagnitude = 0.25f;
    Collider2D[] collidersNearCursor;

    // Start is called before the first frame update
    void Start()
    {
        cursorPositionInWorld = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        SetTelekinesesCursorPosition();
    }

    void SetTelekinesesCursorPosition()
	{
        //get our cursor's position, as well as everything near it in an array
        cursorPositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        collidersNearCursor = Physics2D.OverlapCircleAll(cursorPositionInWorld, aimAssistMagnitude);

        //prepare to sift through that array
        float distanceToClosestObject = Mathf.Infinity;
        Collider2D closestCollider = null;

        //sift through that array

        if (collidersNearCursor.Length != 0)
            Debug.Log(collidersNearCursor.Length);
        else
            Debug.Log("nothing");
    }
}
