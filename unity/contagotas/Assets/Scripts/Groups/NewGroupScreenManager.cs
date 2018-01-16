using System.Collections;
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
        GameSound.gameSound.PlaySFX("button");
		SceneController.sceneController.FadeAndLoadScene("Map", true);
		//UnityEngine.SceneManagement.SceneManager.LoadScene ("Map");
	}
}
