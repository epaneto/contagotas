using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarOptionsController : MonoBehaviour {
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
		avatarController.UpdateTextures (activeSection, itemIndex);
		Debug.Log (itemIndex);
	}



	//CATEGORY CONTROLLER
	public void updateOptions(string section)
	{
		Debug.Log ("update thumb options to " + section);
		activeSection = section;

		hideAllSections ();
		disableAllOptionButtons ();

		ToogleButton optionButton = null;

		if (section == AvatarCategoryStrings.BODY) {
			bodyOptions.SetActive (true);
			optionButton = GameObject.Find ("option" + avatarController.bodyItem).GetComponent<ToogleButton> ();
		}else if(section == AvatarCategoryStrings.EYE){
			eyesOptions.SetActive (true);
			optionButton = GameObject.Find ("option" + avatarController.eyeItem).GetComponent<ToogleButton> ();
		}else if(section == AvatarCategoryStrings.HAIR){
			hairOptions.SetActive (true);
			optionButton = GameObject.Find ("option" + avatarController.hairItem).GetComponent<ToogleButton> ();
		}else if(section == AvatarCategoryStrings.MOUTH){
			mouthOptions.SetActive (true);
			optionButton = GameObject.Find ("option" + avatarController.mouthItem).GetComponent<ToogleButton> ();
		}else if(section == AvatarCategoryStrings.ACC){
			accOptions.SetActive (true);
			optionButton = GameObject.Find ("option" + avatarController.accItem).GetComponent<ToogleButton> ();
		}else if(section == AvatarCategoryStrings.SHIRT){
			shirtOptions.SetActive (true);
			optionButton = GameObject.Find ("option" + avatarController.shirtItem).GetComponent<ToogleButton> ();
		}else if(section == AvatarCategoryStrings.PANTS){
			pantsOptions.SetActive (true);
			optionButton = GameObject.Find ("option" + avatarController.pantsItem).GetComponent<ToogleButton> ();
		}else if(section == AvatarCategoryStrings.SHOE){
			shoeOptions.SetActive (true);
			optionButton = GameObject.Find ("option" + avatarController.showItem).GetComponent<ToogleButton> ();
		}

		if (optionButton != null)
			optionButton.EnableMenuButton ();

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
