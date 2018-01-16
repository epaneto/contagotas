using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTrash : MonoBehaviour {

	FruitDragGame controller;
	float speed;

	// Use this for initialization
	void Start () {
		controller = FindObjectOfType<FruitDragGame>();
		speed = -(Screen.height * 0.7f);
	}

	// Update is called once per frame
	void Update () {
		this.gameObject.transform.Translate(0, speed * Time.deltaTime, 0);

		if (this.gameObject.transform.position.y < 0)
		{
			Debug.Log("LIXO saiu fora");
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("DragDestination"))
		{
			Debug.Log("LIXO TOCOU");
            GameSound.gameSound.PlaySFX("error");
			controller.missedDrop();
			Destroy(this.gameObject);
		}
	}
}
