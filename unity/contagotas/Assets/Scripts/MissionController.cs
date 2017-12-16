using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class MissionController : MonoBehaviour {

	public int maxDays = 17;
	public int activeMission;

	//public static MissionController missionController;

	// Use this for initialization
	void Awake () 
	{
		//if (missionController == null) {
		//	DontDestroyOnLoad (gameObject);
		//	missionController = this;
		//} else if (missionController != this) {
		//	missionController.UpdateMap ();
		//	Destroy (gameObject);
		//}
	}

    void Start () {

		UserData user = UserData.userData;
		user.Load ();

		//First, check if the player has made any progress based on returning days.
        DateTime today = DateTime.Now;
        DateTime lastDate = today;

		Debug.Log ("Player Last access: " + user.playerData.lastAccess);

		if(user.playerData.lastAccess != null)
			lastDate = user.playerData.lastAccess;

		bool isNextDay = false;
        
		if (today.Year > lastDate.Year) {
			isNextDay = true;
		} else if (today.Year < lastDate.Year) {
			isNextDay = false;
		}else if(today.Year == lastDate.Year && today.DayOfYear > lastDate.DayOfYear) {
			isNextDay = true;
		}

		activeMission = UserData.userData.playerData.activeMission;

		if (lastDate.Year != 1)  {
			
			if (isNextDay) {
				
				Debug.Log ("One day has passed.");
				lastDate = today;
				if(activeMission < maxDays)
					activeMission++;

				user.playerData.lastAccess = lastDate;
				user.playerData.activeMission = activeMission;
				user.Save ();

			} else {
				
				Debug.Log ("Not passing or cheating time.");

			}
		} else {
			Debug.Log ("Its a new player, declare his first values.");
			//player never done any progress, place initial values
			lastDate = today;
			activeMission = 1;

			UserData.userData.playerData.lastAccess = lastDate;
			UserData.userData.playerData.activeMission = activeMission;
			UserData.userData.Save ();
		}

		UpdateMap ();

	}

	public void UpdateMap()
	{
		MapController mapController = GameObject.FindObjectOfType<MapController> ();
		mapController.UpdateMapBasedInPlayerProgress (activeMission, maxDays);
	}

	public void OpenMiniGame()
	{
        GameSound.gameSound.PlaySFX("button");
		Debug.Log ("Mission Controller: Open Minigame " + activeMission);
        GameSound.gameSound.StopMusic();
		SceneController.sceneController.FadeAndLoadScene ("Minigames", true);
	}


}
