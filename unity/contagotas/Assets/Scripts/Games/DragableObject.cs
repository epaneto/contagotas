using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableObject : MonoBehaviour {

	public GameObject destinationObject;
	public bool isAtDestination = false;
    public bool isFlowerPot = false;

	// Use this for initialization
	void Start () {
		
	}

	public void onDragMe()
	{
		Vector3 mousePosition = Input.mousePosition;
        if (isFlowerPot)
            mousePosition = new Vector3(Input.mousePosition.x + 5.0f, Input.mousePosition.y - 50.0f, Input.mousePosition.z);
        
		transform.position = Vector2.Lerp(transform.position, mousePosition, 1.0f);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("DragDestination")) {
			isAtDestination = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag ("DragDestination")) {
			isAtDestination = false;
		}
	}
		
}
