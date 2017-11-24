using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapController : MonoBehaviour {

	public void UpdateMapBasedInPlayerProgress(int day, int maxDays)
	{
		Debug.Log("Hello player! Today its your " + day + "th day. let's update the map.");


		for (int i = 1; i <= maxDays; i++) {
			if (i < day) {
				GameObject.Find ("bt_level_" + i).GetComponent<MapButton> ().SetOldSPrite ();
			} else if (i == day) {
				GameObject.Find ("bt_level_" + i).GetComponent<MapButton> ().SetActiveSprite ();
			}
		}

	}
}