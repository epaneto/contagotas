using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HomeController : MonoBehaviour {

	public Button signButton;
	private string destinySceneName;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void HideScene()
	{
		signButton.transform.DOScale(0, 0.6f).SetEase(Ease.OutQuad).OnComplete(GoToScene);
	}

	public void CallForScene(string sceneName)
	{
		destinySceneName = sceneName;
		HideScene ();
	}

	private void GoToScene()
	{
		SceneController.sceneController.FadeAndLoadScene (destinySceneName);
	}
}
