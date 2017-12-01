using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.Text;

public class SignUpController : MonoBehaviour {

	[SerializeField]
	Dropdown cityDropDown;
	[SerializeField]
	Dropdown stateDropDown;

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

			stateDropDown.ClearOptions ();

			List <string> statesNames = new List<string>();

			for (int i = 0; i < States.Count; i++) 
			{
				JToken name = States [i] ["nome"];
				statesNames.Add (name.ToString());
			}

			stateDropDown.AddOptions (statesNames);
//		}

		LoadCities (0);

		LoadFacebookData ();

		Show ();


	}

	public void LoadFacebookData()
	{
		if(PlayerPrefs.HasKey("user_name"))
			GameObject.Find ("input_name").GetComponent<InputField> ().text = PlayerPrefs.GetString("user_name");

		if(PlayerPrefs.HasKey("user_email"))
			GameObject.Find ("input_email").GetComponent<InputField> ().text = PlayerPrefs.GetString("user_email");
		
	}

	public void LoadCities(int stateIndex)
	{
		JArray cities = (JArray) States[stateIndex]["cidades"];

		cityDropDown.ClearOptions ();

		List <string> cityNames = new List<string>();

		for (int i = 0; i < cities.Count; i++) 
		{
			JToken name = cities [i];
			cityNames.Add (name.ToString());
		}

		cityDropDown.AddOptions (cityNames);

	}

	private void Show()
	{
		GameObject title = GameObject.Find ("title");
		GameObject inputname = GameObject.Find ("input_name");
		GameObject inputmail = GameObject.Find ("input_email");
		GameObject requiredfield = GameObject.Find ("required");
		GameObject btConfirm = GameObject.Find ("bt_confirmar");
		GameObject btBack = GameObject.Find ("bt_voltar");


		title.transform.DOMoveY(1500, 1.2f).SetEase(Ease.OutQuad).From();
		inputname.transform.DOMoveY(1500, 1.1f).SetEase(Ease.OutQuad).From();
		inputmail.transform.DOMoveY(1500, 1.0f).SetEase(Ease.OutQuad).From();
		stateDropDown.transform.DOMoveY(1500, 0.9f).SetEase(Ease.OutQuad).From();
		cityDropDown.transform.DOMoveY(1500, 0.8f).SetEase(Ease.OutQuad).From();
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
		
		string state = stateDropDown.options [stateDropDown.value].text;
		string city = cityDropDown.options [cityDropDown.value].text;

		string facebook_id = PlayerPrefs.HasKey("user_facebookid") ? PlayerPrefs.GetString("user_facebookid") : "0";

		StartCoroutine(DoRegister(username,email,city,state,facebook_id));
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

	IEnumerator DoRegister (string playerName, string email, string city, string state, string facebookId)
	{

		Hashtable headers = new Hashtable ();
		headers.Add ("User-Agent", "app-contagotas");
		headers.Add ("charset", "utf-8");

		StringBuilder sb = new StringBuilder ();
		sb.Append ("data={");
		sb.Append ("\"name\"");
		sb.Append (":\"");
		sb.Append (playerName);
		sb.Append ("\"");
		sb.Append (",");
		sb.Append ("\"email\"");
		sb.Append (":\"");
		sb.Append (email);
		sb.Append ("\"");
		sb.Append (",");
		sb.Append ("\"city\"");
		sb.Append (":\"");
		sb.Append (city);
		sb.Append ("\"");
		sb.Append (",");
		sb.Append ("\"state\"");
		sb.Append (":\"");
		sb.Append (state);
		sb.Append ("\"");
		sb.Append (",");
		sb.Append ("\"facebookId\"");
		sb.Append (":\"");
		sb.Append (facebookId);
		sb.Append ("\"");
		sb.Append ("}");

		string finalURL = "http://www.contagotas.online/services/user/create/";

		WWW result = new UnityEngine.WWW(finalURL, Encoding.UTF8.GetBytes(sb.ToString()), headers);
		yield return result;

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			Debug.Log ("Error accepting invite ->" + result.text);
		} else {
			int userId;
			int.TryParse (result.text, out userId);

			PlayerPrefs.SetInt ("user_id", userId);

			Debug.Log ("Criado player com sucesso, com a id = " + userId + " deveria ir para outra cena");

			//TODO: Esse funcao nao ta funcionando O_o, aparentemente dessa cena, ele ta usando uma variavel estatica que não foi inicializada.
			//CallForScene ("Avatar");
			UnityEngine.SceneManagement.SceneManager.LoadScene("Avatar");
		}
	}

}