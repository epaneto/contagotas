using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenSceneButton : MonoBehaviour
{
	public string sceneName = "";
	public Button yourButton;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(CallForScene);
	}

	void CallForScene()
	{
		GameObject sceneController = GameObject.Find ("AppSceneController");
		SceneController controllerInstance = sceneController.GetComponent<SceneController>();
		controllerInstance.FadeAndLoadScene (sceneName);
	}
}