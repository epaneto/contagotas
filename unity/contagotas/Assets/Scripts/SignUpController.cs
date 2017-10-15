using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignUpController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void show()
	{

	}

	private void hide()
	{

	}

	public void CallForScene(string sceneName)
	{
		//SceneController.sceneController.BeforeSceneUnload += hide;
		//SceneController.sceneController.AfterSceneLoad += show;

		SceneController.sceneController.FadeAndLoadScene (sceneName);
	}
}
