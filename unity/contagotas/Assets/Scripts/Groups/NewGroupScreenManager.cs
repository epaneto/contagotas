﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGroupScreenManager : BaseAssetsGroupManager {

	public void ShowCreateScreen(){
		screenManager.ShowCreateScreen ();
	}

	public void ShowJoinScreen(){
		screenManager.ShowJoinScreen ();
	}

	public void ShowMapScreen(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Map");
	}
}
