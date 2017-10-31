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

	public string[] bodyOptionsArray;
	public string[] eyeOptionsArray;
	public string[] hairOptionsArray;
	public string[] mouthOptionsArray;

	public string[] accOptionsArray;
	public string[] shirtOptionsArray;
	public string[] pantsOptionsArray;
	public string[] shoeOptionsArray;

	//OPTIONS CONTROLLER
	public void disableAllOptionButtons()
	{
		optionButtons = GetComponentsInChildren<ToogleButton>();

		foreach (ToogleButton toogleButton in optionButtons)
			toogleButton.DisableMenuButton ();
	}

	public void loadItemAtIndex(int itemIndex)
	{

	}



	//CATEGORY CONTROLLER
	public void updateOptions(string section)
	{
		Debug.Log ("update options to " + section);
		activeSection = section;

		hideAllSections ();

		switch (section) {
			case "bt_body":
				bodyOptions.SetActive (true);
			break;

			case "bt_eye":
			eyesOptions.SetActive (true);
			break;

		case "bt_hair":
			hairOptions.SetActive (true);
			break;

		case "bt_mouth":
			mouthOptions.SetActive (true);
			break;

		case "bt_acessory":
			accOptions.SetActive (true);
			break;

		case "bt_shirt":
			shirtOptions.SetActive (true);
			break;

		case "bt_pants":
			pantsOptions.SetActive (true);
			break;

		case "bt_shoes":
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
