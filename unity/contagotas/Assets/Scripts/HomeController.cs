using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HomeController : MonoBehaviour {

	public Button signButton;
	public Button facebookButton;
	private string destinySceneName;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void HideScene()
	{
		GameObject background = GameObject.Find ("background");
		GameObject logo = GameObject.Find ("logo");
		GameObject caixalogo = GameObject.Find ("caixa_logo");

		signButton.transform.DOMoveY(-Screen.height - signButton.transform.localScale.y , 0.6f).SetEase(Ease.InQuad).OnComplete(GoToScene);
		facebookButton.transform.DOMoveY(-Screen.height - facebookButton.transform.localScale.y , 0.6f).SetEase(Ease.InQuad).OnComplete(GoToScene);

		logo.transform.DOMoveY(-Screen.height - logo.transform.localScale.y , 1f).SetEase(Ease.InQuad).OnComplete(GoToScene);
		caixalogo.transform.DOMoveY(-Screen.height - caixalogo.transform.localScale.y , 1f).SetEase(Ease.InQuad).OnComplete(GoToScene);

		//background.gameObject.GetComponent<Image> ().DOFade (0, 2.0f);
		background.transform.DOMoveY(-Screen.height / 2 , 1.5f).SetEase(Ease.InQuad).OnComplete(GoToScene);
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
