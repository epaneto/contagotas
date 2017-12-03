using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {

    CollectRainGame controller;
	// Use this for initialization
	void Start () {
        controller = FindObjectOfType<CollectRainGame>();
	}
	
	// Update is called once per frame
	void Update () {

        this.gameObject.transform.Translate(Vector3.down * 23.0f);

        if (this.gameObject.transform.position.y < -1500)
        {
            Debug.Log("gota saiu fora");
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
