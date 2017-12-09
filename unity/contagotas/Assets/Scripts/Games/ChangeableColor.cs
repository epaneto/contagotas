using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeableColor : MonoBehaviour {

    public Color rightColor;
    public Color wrongColor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeColor(bool isRight)
    {
        Image img = this.gameObject.GetComponent<Image>();

        if (isRight)
            img.color = rightColor;
        else
            img.color = wrongColor;
    }
}
