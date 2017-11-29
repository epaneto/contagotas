using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableObject : MonoBehaviour {

	public GameObject destinationObject;
	public bool isTouchingDestiantion = false;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (isTouchingDestiantion)
			return;
		
		if (Input.GetMouseButton (0)) {

			Transform newTransform = this.gameObject.transform;
			newTransform.position = Input.mousePosition;
			this.gameObject.transform.position = newTransform.position;

			Renderer thisRenderer;
			Renderer destinyRenderer;

			thisRenderer = this.gameObject.renderer;
			destinyRenderer = destinationObject.gameObject.renderer;

			if (thisRenderer.bounds.Intersects(destinyRenderer.bounds)) {
				Debug.Log ("TOUCHED");
				isTouchingDestiantion = true;
			}

		}

	}
}
