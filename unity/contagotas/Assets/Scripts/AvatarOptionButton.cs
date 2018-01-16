using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarOptionButton : MonoBehaviour {

	public string itemIndex;

	public void updateOption()
	{
		GameObject options = GameObject.Find("options");
		AvatarOptionsController optionsController = options.GetComponent (typeof (AvatarOptionsController)) as AvatarOptionsController;
		optionsController.disableAllOptionButtons ();
		optionsController.loadItemAtIndex (itemIndex);

		//enable this button
        GameSound.gameSound.PlaySFX("button");
		ToogleButton button = gameObject.GetComponent<ToogleButton>();
		button.EnableMenuButton ();
	}
}
