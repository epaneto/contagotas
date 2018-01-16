using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCategoryButton : MonoBehaviour {

	public void updateMenu()
	{
        GameSound.gameSound.PlaySFX("button");
		////disable all butons and load category
		GameObject menu = GameObject.Find("menu");
		AvatarMenuController menuController = menu.GetComponent (typeof (AvatarMenuController)) as AvatarMenuController;
		menuController.disableAllButtons (gameObject.name);

		//enable this button
		ToogleButton button = gameObject.GetComponent<ToogleButton>();
		button.EnableMenuButton ();
	}
}