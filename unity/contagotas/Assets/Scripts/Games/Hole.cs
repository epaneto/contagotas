using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour {

	TapHoleGame controller;

	// Use this for initialization
	void Start () {
		controller = FindObjectOfType<TapHoleGame>();
	}
	
	public void TappedHole()
	{
		controller.updateHoles ();
		this.gameObject.SetActive (false);
	}
}
