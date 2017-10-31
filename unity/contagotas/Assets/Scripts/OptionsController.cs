using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour {
	string activeSession = "hair";

	public GameObject bodyOptions;
	public GameObject eyesOptions;
	public GameObject hairOptions;
	public GameObject mouthOptions;

	public GameObject accOptions;
	public GameObject shirtOptions;
	public GameObject pantsOptions;
	public GameObject shoeOptions;

	Component[] optionButtons; 

	string activeSection = "";

	AvatarDecorator avatarController;

	public void Start()
	{
		avatarController = GameObject.FindObjectOfType<AvatarDecorator> ();
	}
	//OPTIONS CONTROLLER
	public void disableAllOptionButtons()
	{
		optionButtons = GetComponentsInChildren<ToogleButton>();

		foreach (ToogleButton toogleButton in optionButtons)
			toogleButton.DisableMenuButton ();
	}

	public void loadItemAtIndex(string itemIndex)
	{
		avatarController.UpdateTextures (activeSection, activeSection + "/" + activeSection + itemIndex);
	}



	//CATEGORY CONTROLLER
	public void updateOptions(string section)
	{
		Debug.Log ("update thumb options to " + section);
		activeSection = section;

		hideAllSections ();

		switch (section) {
			case "body":
				bodyOptions.SetActive (true);
			break;

			case "eye":
			eyesOptions.SetActive (true);
			break;

		case "hair":
			hairOptions.SetActive (true);
			break;

		case "mouth":
			mouthOptions.SetActive (true);
			break;

		case "accessories":
			accOptions.SetActive (true);
			break;

		case "shirt":
			shirtOptions.SetActive (true);
			break;

		case "pants":
			pantsOptions.SetActive (true);
			break;

		case "shoe":
			shoeOptions.SetActive (true);
			break;
		}
	}

	void hideAllSections()
	{
		bodyOptions.SetActive (false);
		eyesOptions.SetActive (false);
		hairOptions.SetActive (false);
		mouthOptions.SetActive (false);

		accOptions.SetActive (false);
		shirtOptions.SetActive (false);
		pantsOptions.SetActive (false);
		shoeOptions.SetActive (false);
	}
}
