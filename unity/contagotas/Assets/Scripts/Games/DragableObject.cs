using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragableObject : MonoBehaviour {

	public GameObject destinationObject;
	public bool isTouchingDestiantion = false;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	bool isDraging = false;
	void Update () {
		
		if (isTouchingDestiantion)
			return;

		if (isDraging) {
			Debug.Log ("DRAG ME!");
			Transform newSpongeTransform = this.gameObject.transform;
			newSpongeTransform.position = Input.mousePosition;
		}

		RaycastHit2D hitInfo = Physics2D.Raycast (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, 0);

		if (Input.GetMouseButtonDown (0)){
			Debug.Log (hitInfo);

			if (hitInfo) {
				Debug.Log ("its a hit");
				isDraging = true;
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			isDraging = false;
		}

	}

	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.tag == "dragable")
		{
			Physics2D.IgnoreCollision(coll.gameObject.GetComponent<BoxCollider2D>(), this.gameObject.GetComponent<BoxCollider2D>());
			return;
		}


		Debug.Log ("tocou em algum role");
		isTouchingDestiantion = true;
	}
}
