﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour {

	public Sprite lockedSprite;
	public Sprite activeSprite;
	public Sprite oldSprite;


	public void SetActiveSprite()
	{
		gameObject.GetComponent<Button> ().onClick.AddListener(OpenMinigame);
		gameObject.GetComponent<Image>().sprite = activeSprite;
	}

	public void SetOldSPrite()
	{
		gameObject.GetComponent<Image>().sprite = oldSprite;
	}

	void OpenMinigame()
	{
		MissionController.missionController.OpenMiniGame ();
	}
}