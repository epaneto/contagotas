using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMenuController : MonoBehaviour {

	Component[] menuButtons;
	GameObject options;
	AvatarOptionsController optionsController;

	void Start()
	{
		options = GameObject.Find ("options");
		optionsController = options.GetComponent<AvatarOptionsController> ();

		GameObject go = GameObject.Find ("body");
		AvatarCategoryButton cb = go.GetComponent<AvatarCategoryButton> ();
		cb.updateMenu ();
	}

	public void disableAllButtons(string nextSection)
	{
		menuButtons = GetComponentsInChildren<ToogleButton>();

		foreach (ToogleButton toogleButton in menuButtons)
			toogleButton.DisableMenuButton ();

		optionsController.updateOptions (nextSection);
	}
}
