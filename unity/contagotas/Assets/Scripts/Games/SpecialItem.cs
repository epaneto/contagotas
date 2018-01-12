using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItem : MonoBehaviour {

	CollectRainGame controller;
	float speed;

	// Use this for initialization
	void Start () {
		controller = FindObjectOfType<CollectRainGame>();
		speed = -(Screen.height * 0.7f);
	}

	// Update is called once per frame
	void Update () {
		this.gameObject.transform.Translate(0, speed * Time.deltaTime, 0);

		if (this.gameObject.transform.position.y < 0)
		{
			Debug.Log("Special saiu fora");
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("DragDestination"))
		{
			Debug.Log("SPECIAL TOCOU");
			controller.SpecialCollected();
			Destroy(this.gameObject);
		}
	}
}
