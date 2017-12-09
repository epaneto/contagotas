using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {

    CollectRainGame controller;
    float speed = -900.0f;

	// Use this for initialization
	void Start () {
        controller = FindObjectOfType<CollectRainGame>();
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.Translate(0, speed * Time.deltaTime, 0);

        if (this.gameObject.transform.position.y < 0)
        {
            Debug.Log("gota saiu fora");
            controller.missedDrop();
            Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DragDestination"))
        {
            Debug.Log("GOTA TOCOU");
            controller.CollectedDrop();
            Destroy(this.gameObject);
        }
    }
}
