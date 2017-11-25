using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class MinigamesController : MonoBehaviour {

	private string missionJSONFile = "missoes.json";
	List<string> minigames;
	public int minigameIndex;

	JArray Missions;
	GameObject endGame;
	GameObject scoreTxt;
	GameObject continueButton;
	GameObject hintTxt;

	// Use this for initialization
	void Start () {
		endGame = GameObject.Find ("EndGame");
		scoreTxt = GameObject.Find ("score_txt");
		continueButton = GameObject.Find ("bt_continuar");
		hintTxt = GameObject.Find ("hint_txt");

		///load mission json
		TextAsset t = (TextAsset) Resources.Load("missoes", typeof(TextAsset));
		string DataAsJSON = t.text;

		JObject o = JObject.Parse(DataAsJSON);
		Missions = (JArray) o["missoes"];


		endGame.SetActive (false);

		minigameIndex = 0;

		JToken sceneName = Missions[minigameIndex]["sceneid"];
		SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);
	}

	public void ShowResults(int score)
	{
		///REMOVE ACTIVE MINIGAME FROM SCREEN
		JToken sceneName = Missions[minigameIndex]["sceneid"];
		SceneManager.UnloadSceneAsync (sceneName.ToString());


		///SHOW END GAME
		Text txt = scoreTxt.GetComponent<Text> ();
		txt.text = score.ToString ();

		JToken hintString = Missions[minigameIndex]["hint"];
		Text hintField = hintTxt.GetComponent<Text> ();
		hintField.text = hintString.ToString ();

		endGame.SetActive(true);
		continueButton.GetComponent<Button> ().onClick.AddListener (PlayNextMiniGame);
	}

	public void PlayNextMiniGame()
	{
		continueButton.GetComponent<Button> ().onClick.RemoveAllListeners ();

		if (minigameIndex + 1  < Missions.Count) {
			//Debug.Log ("play next minigame " + minigameIndex);
			minigameIndex++;

			JToken sceneName = Missions[minigameIndex]["sceneid"];
			SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);

			endGame.SetActive (false);
		} else {
			//Debug.Log ("that was the last minigame, show map");
			SceneController.sceneController.FadeAndLoadScene ("Map", true);
		}
	}
}