using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	Component[] menuButtons;
	GameObject options;
	OptionsController optionsController;

	void Start()
	{
		options = GameObject.Find ("options");
		optionsController = options.GetComponent<OptionsController> ();

		GameObject go = GameObject.Find ("body");
		CategoryButton cb = go.GetComponent<CategoryButton> ();
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
