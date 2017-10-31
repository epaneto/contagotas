using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class AvatarDecorator : MonoBehaviour {

	string avatarResourcesPath = "Art/Avatar/Characters/";

	[SpineSkin] public string templateSkinName = "default";
	public Material sourceMaterial;

	Sprite bodySprite;
	[SpineSlot] public string bodySlot;
	[SpineAttachment(slotField:"body_slot", skinField:"default")] public string bodyKey = "body";

	Sprite hairSprite;
	[SpineSlot] public string hairSlot;
	[SpineAttachment(slotField:"hair_slot", skinField:"default")] public string hairKey = "hair";

	Sprite eyeSprite;
	[SpineSlot] public string eyeSlot;
	[SpineAttachment(slotField:"eye_slot", skinField:"default")] public string eyeKey = "eye";

	Sprite mouthSprite;
	[SpineSlot] public string mouthSlot;
	[SpineAttachment(slotField:"mouth_slot", skinField:"default")] public string mouthKey = "mouth";

	Sprite accSprite;
	[SpineSlot] public string accSlot;
	[SpineAttachment(slotField:"acessorie_slot", skinField:"default")] public string accKey = "accessorie";

	Sprite shirtSprite;
	[SpineSlot] public string shirtSlot;
	[SpineAttachment(slotField:"shirt_slot", skinField:"default")] public string shirtKey = "shirt";

	Sprite pantsSprite;
	[SpineSlot] public string pantsSlot;
	[SpineAttachment(slotField:"pants_slot", skinField:"default")] public string pantsKey = "pants";

	Sprite shoeSprite;
	[SpineSlot] public string shoeSlot;
	[SpineAttachment(slotField:"show_slot", skinField:"default")] public string showKey = "shoe";

	IEnumerator Start () {
		yield return new WaitForSeconds(1f); // Delay for one second before applying. For testing.
//		UpdateTextures("","");
	}

	public void UpdateTextures(string itemCategory, string itemName) {
		Debug.Log ("change avatar item at category " + itemCategory + " with item named " + itemName);

		if(itemCategory == "body"){
			bodySprite = Resources.Load(avatarResourcesPath + itemName, typeof(Sprite)) as Sprite;
		}else if(itemCategory == "eye"){
			eyeSprite = Resources.Load(avatarResourcesPath + itemName, typeof(Sprite)) as Sprite;
		}else if(itemCategory == "hair"){
			hairSprite = Resources.Load(avatarResourcesPath + itemName, typeof(Sprite)) as Sprite;
		}else if(itemCategory == "mouth"){
			mouthSprite = Resources.Load(avatarResourcesPath + itemName, typeof(Sprite)) as Sprite;
		}else if(itemCategory == "accessories"){
			accSprite = Resources.Load(avatarResourcesPath + itemName, typeof(Sprite)) as Sprite;
		}else if(itemCategory == "shirt"){
			shirtSprite = Resources.Load(avatarResourcesPath + itemName, typeof(Sprite)) as Sprite;
		}else if(itemCategory == "pants"){
			pantsSprite = Resources.Load(avatarResourcesPath + itemName, typeof(Sprite)) as Sprite;
		}else if(itemCategory == "shoe"){
			shoeSprite = Resources.Load(avatarResourcesPath + itemName, typeof(Sprite)) as Sprite;
		}

		Apply();
	}

	void Apply () {
		SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>();
		Skeleton skeleton = skeletonAnimation.skeleton;
		SkeletonData skeletonData = skeleton.Data;

		// Get the template skin. Prepare the custom skin.
		Skin templateSkin = skeletonData.FindSkin(templateSkinName);
		Skin currentEquipsSkin = new Skin("my custom skin");

//		BODY SETUP
		if (bodySprite) {
			int bodySlotIndex = skeleton.FindSlotIndex (bodySlot);
			Attachment templateBody = templateSkin.GetAttachment (bodySlotIndex, bodyKey);
			Attachment newBody = templateBody.GetRemappedClone (bodySprite, sourceMaterial);
			if (newBody != null)
				currentEquipsSkin.SetAttachment (bodySlotIndex, bodyKey, newBody);
		}

//		HAIR SETUP
		if (hairSprite) {
			int hairSlotIndex = skeleton.FindSlotIndex (hairSlot);
			Attachment templateHair = templateSkin.GetAttachment (hairSlotIndex, hairKey);
			Attachment newHair = templateHair.GetRemappedClone (hairSprite, sourceMaterial);
			if (newHair != null)
				currentEquipsSkin.SetAttachment (hairSlotIndex, hairKey, newHair);
		}

//		EYE SETUP
		if (eyeSprite) {
			int eyeSlotIndex = skeleton.FindSlotIndex (eyeSlot);
			Attachment templateEye = templateSkin.GetAttachment (eyeSlotIndex, eyeKey);
			Attachment newEye = templateEye.GetRemappedClone (eyeSprite, sourceMaterial);
			if (newEye != null)
				currentEquipsSkin.SetAttachment (eyeSlotIndex, eyeKey, newEye);
		}

//		MOUTH SETUP
		if (mouthSprite) {
			int mouthSlotsIndex = skeleton.FindSlotIndex (mouthSlot);
			Attachment templateMouth = templateSkin.GetAttachment (mouthSlotsIndex, mouthKey);
			Attachment newMouth = templateMouth.GetRemappedClone (mouthSprite, sourceMaterial);
			if (newMouth != null)
				currentEquipsSkin.SetAttachment (mouthSlotsIndex, mouthKey, newMouth);
		}


//		ACC SETUP
		if (accSprite) {
			int accSlotsIndex = skeleton.FindSlotIndex (accSlot);
			Attachment templateAcc = templateSkin.GetAttachment (accSlotsIndex, accKey);
			Attachment newAcc = templateAcc.GetRemappedClone (accSprite, sourceMaterial);
			if (newAcc != null)
				currentEquipsSkin.SetAttachment (accSlotsIndex, accKey, newAcc);
		}

//		SHIRT SETUP
		if (shirtSprite) {
			int shirtSlotsIndex = skeleton.FindSlotIndex (shirtSlot);
			Attachment templateShirt = templateSkin.GetAttachment (shirtSlotsIndex, shirtKey);
			Attachment newShirt = templateShirt.GetRemappedClone (shirtSprite, sourceMaterial);
			if (newShirt != null)
				currentEquipsSkin.SetAttachment (shirtSlotsIndex, shirtKey, newShirt);
		}

//		//PANTS SETUP
		if (pantsSprite) {
			int pantsSlotIndex = skeleton.FindSlotIndex (pantsSlot);
			Attachment templatePants = templateSkin.GetAttachment (pantsSlotIndex, pantsKey);
			Attachment newPants = templatePants.GetRemappedClone (pantsSprite, sourceMaterial);
			if (newPants != null)
				currentEquipsSkin.SetAttachment (pantsSlotIndex, pantsKey, newPants);
		}

//		SHOE SETUP
		if (shoeSprite) {
			int shoeSlotsIndex = skeleton.FindSlotIndex (shoeSlot);
			Attachment templateShoe = templateSkin.GetAttachment (shoeSlotsIndex, showKey);
			Attachment newShoe = templateShoe.GetRemappedClone (shoeSprite, sourceMaterial);
			if (newShoe != null)
				currentEquipsSkin.SetAttachment (shoeSlotsIndex, showKey, newShoe);
		}



		// Set and apply the Skin to the skeleton.
		skeleton.SetSkin(currentEquipsSkin);
		skeleton.SetSlotsToSetupPose();
		skeletonAnimation.Update(0);

		Resources.UnloadUnusedAssets();
	}


	//remove attachments
	//currentEquipsSkin.RemoveAttachment (bodySlotIndex, bodyKey);

}
