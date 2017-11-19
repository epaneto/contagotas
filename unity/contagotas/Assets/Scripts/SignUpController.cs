using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class SignUpController : MonoBehaviour {

	private string destinySceneName;
	private string stateJSONFile = "geo.json";
	private JArray States;

	void Start()
	{
		string filePath = Path.Combine(Application.streamingAssetsPath, stateJSONFile);

		TextAsset t = (TextAsset) Resources.Load("geo", typeof(TextAsset));

//		if (File.Exists (filePath)) 
//		{
		string DataAsJSON = t.text;

			JObject o = JObject.Parse(DataAsJSON);
			States = (JArray) o["estados"];

			Dropdown statesDrop = GameObject.Find ("drop_state").GetComponent<Dropdown> ();
			statesDrop.ClearOptions ();

			List <string> statesNames = new List<string>();

			for (int i = 0; i < States.Count; i++) 
			{
				JToken name = States [i] ["nome"];
				statesNames.Add (name.ToString());
			}

			statesDrop.AddOptions (statesNames);
//		}

		LoadCities (0);

		Show ();

	}

	public void LoadCities(int stateIndex)
	{
		JArray cities = (JArray) States[stateIndex]["cidades"];

		Dropdown cityDrop = GameObject.Find ("drop_city").GetComponent<Dropdown> ();
		cityDrop.ClearOptions ();

		List <string> cityNames = new List<string>();

		for (int i = 0; i < cities.Count; i++) 
		{
			JToken name = cities [i];
			cityNames.Add (name.ToString());
		}

		cityDrop.AddOptions (cityNames);

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
		GameObject btBack = GameObject.Find ("bt_voltar");


		title.transform.DOMoveY(1500, 1.2f).SetEase(Ease.OutQuad).From();
		inputname.transform.DOMoveY(1500, 1.1f).SetEase(Ease.OutQuad).From();
		inputmail.transform.DOMoveY(1500, 1.0f).SetEase(Ease.OutQuad).From();
		dropstate.transform.DOMoveY(1500, 0.9f).SetEase(Ease.OutQuad).From();
		dropcity.transform.DOMoveY(1500, 0.8f).SetEase(Ease.OutQuad).From();
		requiredfield.transform.DOMoveY(1500, 0.7f).SetEase(Ease.OutQuad).From();
		btConfirm.transform.DOMoveY(1500, 0.6f).SetEase(Ease.OutQuad).From();
		btBack.transform.DOMoveY(1500, 0.6f).SetEase(Ease.OutQuad).From();
	}

	public void SendData()
	{
		string username = GameObject.Find ("input_name").GetComponent<InputField> ().text;
		string email = GameObject.Find ("input_email").GetComponent<InputField> ().text;

		if (username == "Nome" || email == "E-mail")
			return;
		
		CallForScene ("Avatar");
	}

	public void GoBack()
	{
		CallForScene ("Home");
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