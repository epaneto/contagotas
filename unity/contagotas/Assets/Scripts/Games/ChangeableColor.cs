using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeableColor : MonoBehaviour {

    public Color rightColor;
    public Color wrongColor;
    Color initColor;

	// Use this for initialization
	void Start () {
        Image img = this.gameObject.GetComponent<Image>();
        initColor = img.color;
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

    public void ResetColor()
    {
        Image img = this.gameObject.GetComponent<Image>();
        img.color = initColor;
    }
}
