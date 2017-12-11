using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToScene : MonoBehaviour {

	[SerializeField]
	string sceneName;

	public void ChangeScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}
}
