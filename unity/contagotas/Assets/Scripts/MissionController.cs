﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissionController : MonoBehaviour {

	int maxDays = 21;

	void Start () {

		UserData user = UserData.userData;
		user.Load ();

		//First, check if the player has made any progress based on returning days.
		DateTime lastDate;

		Debug.Log ("Player Last access: " + user.playerData.lastAccess);

		if(user.playerData.lastAccess != null)
			lastDate = user.playerData.lastAccess;
		
		DateTime today = DateTime.Now;
		int activeMission = UserData.userData.playerData.activeMission;

		if (lastDate.Year != 1)  {
			
			if (today.Day != lastDate.Day && today.Month >= lastDate.Month && today.Year >= lastDate.Year) {
				
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

		MapController mapController = GameObject.FindObjectOfType<MapController> ();
		mapController.UpdateMapBasedInPlayerProgress (activeMission, maxDays);

	}

}