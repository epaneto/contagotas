using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToogleButton : MonoBehaviour {
	
	public GameObject MyIcon;

	public Sprite pressedSprite;
	public Sprite normalSprite;

	public void EnableMenuButton()
	{
		gameObject.GetComponent<Image>().sprite = pressedSprite;

		if(MyIcon)
			MyIcon.GetComponent<Image>().color = Color.red;	
	}

	public void DisableMenuButton() {
		
		gameObject.GetComponent<Image>().sprite = normalSprite;	

		if(MyIcon)
			MyIcon.GetComponent<Image>().color = Color.white;	
	}
}