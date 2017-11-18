using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Linq;

public class SignUpController : MonoBehaviour {

	private string destinySceneName;
	private JObject jsonData;
	void Start()
	{
		Show ();

	}

	private void Show()
	{
		GameObject title = GameObject.Find ("title");
		GameObject inputname = GameObject.Find ("input_name");
		GameObject inputmail = GameObject.Find ("input_email");
		GameObject dropstate = GameObject.Find ("drop_state");
		GameObject dropcity = GameObject.Find ("drop_city");
		GameObject requiredfield = GameObject.Find ("required");
		GameObject btConfirm = GameObject.Find ("bt_confirmar");


		title.transform.DOMoveY(2000, 1.2f).SetEase(Ease.OutQuad).From();
		inputname.transform.DOMoveY(2000, 1.1f).SetEase(Ease.OutQuad).From();
		inputmail.transform.DOMoveY(2000, 1.0f).SetEase(Ease.OutQuad).From();
		dropstate.transform.DOMoveY(2000, 0.9f).SetEase(Ease.OutQuad).From();
		dropcity.transform.DOMoveY(2000, 0.8f).SetEase(Ease.OutQuad).From();
		requiredfield.transform.DOMoveY(2000, 0.7f).SetEase(Ease.OutQuad).From();
		btConfirm.transform.DOMoveY(2000, 0.6f).SetEase(Ease.OutQuad).From();
	}

	public void SendData()
	{
		CallForScene ("Avatar");
	}

	public void CallForScene(string sceneName)
	{
		destinySceneName = sceneName;
		HideScene ();
	}

	private void HideScene()
	{
		GoToScene ();
	}

	private void GoToScene()
	{
		SceneController.sceneController.FadeAndLoadScene (destinySceneName, true);
	}
}
