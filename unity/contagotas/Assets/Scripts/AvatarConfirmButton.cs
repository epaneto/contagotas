using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarConfirmButton : MonoBehaviour {

	public void SaveAvatar()
	{
		GameObject go = GameObject.Find ("avatar");
		AvatarDecorator ad = go.GetComponent<AvatarDecorator> ();
		ad.SaveUserDefaultAvatar ();

		Debug.Log ("avatar saved");
	}
}
