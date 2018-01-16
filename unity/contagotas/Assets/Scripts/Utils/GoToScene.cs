using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToScene : MonoBehaviour {

	[SerializeField]
	string sceneName;

	public void ChangeScene()
	{
		SceneController.sceneController.FadeAndLoadScene(sceneName, true);
		//UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}
}
