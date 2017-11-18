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

		if (File.Exists (filePath)) 
		{
			string DataAsJSON = File.ReadAllText (filePath);

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
		}

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

[System.Serializable]
public class Estado
{
	public string sigla { get; set; }
	public string nome { get; set; }
	public List<string> cidades { get; set; }
}

[System.Serializable]
public class StateList
{
	public List<Estado> estados { get; set; }
}