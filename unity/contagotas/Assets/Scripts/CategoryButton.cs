using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryButton : MonoBehaviour {

	public void updateMenu()
	{
		////disable all butons and load category
		GameObject menu = GameObject.Find("menu");
		MenuController menuController = menu.GetComponent (typeof (MenuController)) as MenuController;
		menuController.disableAllButtons (gameObject.name);

		//enable this button
		ToogleButton button = gameObject.GetComponent<ToogleButton>();
		button.EnableMenuButton ();
	}
}