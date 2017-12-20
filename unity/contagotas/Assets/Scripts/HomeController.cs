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
		GameObject leafs1 = GameObject.Find ("leafs1");
		GameObject leafs2 = GameObject.Find ("leafs2");

		signButton.transform.DOMoveY (-Screen.height - signButton.transform.localScale.y, 0.6f).SetEase (Ease.InQuad);
		facebookButton.transform.DOMoveY (-Screen.height - facebookButton.transform.localScale.y, 0.6f).SetEase (Ease.InQuad);

		logo.transform.DOMoveY (-Screen.height - logo.transform.localScale.y, 1f).SetEase (Ease.InQuad);
		caixalogo.transform.DOMoveY (-Screen.height - caixalogo.transform.localScale.y, 1f).SetEase (Ease.InQuad);

		background.transform.DOMoveY(-Screen.height / 2 , 1.5f).SetEase(Ease.InQuad).OnComplete(GoToScene);

		leafs1.transform.DOMoveY (-Screen.height / 2, 1.7f).SetEase (Ease.InQuad);
		leafs2.transform.DOMoveY (-Screen.height / 2, 1.7f).SetEase (Ease.InQuad);
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

    public void PlayButtonSound()
    {
        GameSound.gameSound.PlaySFX("button");
    }
}
