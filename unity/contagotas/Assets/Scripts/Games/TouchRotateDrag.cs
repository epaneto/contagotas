using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotateDrag : MonoBehaviour {

	private bool canDrag = true;
    public bool isRight = false;
	public string dragDestinatonName;
    public TouchRotate TouchRotateScript;
	// Use this for initialization

	public void onDragMe()
	{
		if (canDrag) {
			Vector3 mousePosition = Input.mousePosition;
			transform.position = Vector2.Lerp (transform.position, mousePosition, 1.0f);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("DragDestination")) {
			if (dragDestinatonName == other.gameObject.name) {
				
				this.transform.position = other.gameObject.transform.position;
                canDrag = false;
                isRight = true;
                TouchRotateScript.CheckPipe();
			}
		}
	}

}
