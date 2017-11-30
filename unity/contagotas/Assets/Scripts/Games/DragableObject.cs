using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableObject : MonoBehaviour {

	public GameObject destinationObject;
	//public bool isTouchingDestination = false;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	bool isDraging = false;

	void Update () {

	}

	public void onDragMe()
	{
		Vector3 mousePosition = Input.mousePosition;
		transform.position = Vector2.Lerp(transform.position, mousePosition, 1.0f);
	}

	public void checkCollision()
	{
		if (destinationObject.GetComponent<Image> ().sprite.bounds.Intersects (GetComponent<Image> ().sprite.bounds)) {
			Debug.Log ("ta colando em mim mano");
		}
	}
		
}
