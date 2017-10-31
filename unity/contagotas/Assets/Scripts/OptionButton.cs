using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour {

	public int itemIndex;

	public void updateOption()
	{
		GameObject options = GameObject.Find("options");
		OptionsController optionsController = options.GetComponent (typeof (OptionsController)) as OptionsController;
		optionsController.disableAllOptionButtons ();
		optionsController.loadItemAtIndex (itemIndex);

		//enable this button
		ToogleButton button = gameObject.GetComponent<ToogleButton>();
		button.EnableMenuButton ();
	}
}
