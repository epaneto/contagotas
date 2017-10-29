using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class AvatarDecorator : MonoBehaviour {

	[SpineSkin] public string templateSkinName = "default";
	public Material sourceMaterial;

	[Header("Default Body")]
	public Sprite bodySprite;
	[SpineSlot] public string bodySlot;
	[SpineAttachment(slotField:"body_slot", skinField:"default")] public string bodyKey = "body";

	IEnumerator Start () {
		yield return new WaitForSeconds(1f); // Delay for one second before applying. For testing.
		Apply();
	}

	void Apply () {
		SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>();
		Skeleton skeleton = skeletonAnimation.skeleton;
		SkeletonData skeletonData = skeleton.Data;

		// Get the template skin.
		Skin templateSkin = skeletonData.FindSkin(templateSkinName);
		// Prepare the custom skin.
		Skin currentEquipsSkin = new Skin("my custom skin");

		// Get the body
		int bodySlotIndex = skeleton.FindSlotIndex(bodySlot);
		Attachment templateBody = templateSkin.GetAttachment(bodySlotIndex, bodyKey);

		// Clone the template gun Attachment, and map the sprite onto it.
		// This sample uses the sprite and material set in the inspector.
		Attachment newBody = templateBody.GetRemappedClone(bodySprite, sourceMaterial); // This has some optional parameters. See below.
		//Attachment newGun = templateGun.GetRemappedClone(gunSprite, sourceMaterial, premultiplyAlpha: true, cloneMeshAsLinked: true, useOriginalRegionSize: false); // (Full signature.)

		// Add the gun to your new custom skin.
		if (newBody != null) currentEquipsSkin.SetAttachment(bodySlotIndex, bodyKey, newBody);

		// Set and apply the Skin to the skeleton.
		skeleton.SetSkin(currentEquipsSkin);
		skeleton.SetSlotsToSetupPose();
		skeletonAnimation.Update(0);

		Resources.UnloadUnusedAssets();
	}

}
