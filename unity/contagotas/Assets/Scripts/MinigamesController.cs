using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class MinigamesController : MonoBehaviour {

	private string missionJSONFile = "missoes.json";
	List<string> minigames;
	public int minigameIndex;

	JArray Missions;
	GameObject endGame;
	GameObject loseGame;
	GameObject loseGameSadChar;
	GameObject scoreTxt;
	GameObject continueButton;
	GameObject continueLoseButton;
	GameObject hintTxt;

	// Use this for initialization
	void Start () {
		endGame = GameObject.Find ("EndGame");
		scoreTxt = GameObject.Find ("score_txt");
		continueButton = GameObject.Find ("bt_continuar_score");
		continueLoseButton = GameObject.Find ("bt_continuar_lose");
		hintTxt = GameObject.Find ("hint_txt");
		loseGame = GameObject.Find ("GameLose");
		loseGameSadChar = GameObject.Find ("character_sad");

		///load mission json
		TextAsset t = (TextAsset) Resources.Load("missoes", typeof(TextAsset));
		string DataAsJSON = t.text;

		JObject o = JObject.Parse(DataAsJSON);
		Missions = (JArray) o["missoes"];


		endGame.SetActive (false);
		loseGame.SetActive (false);

		minigameIndex = 0;

		JToken sceneName = Missions[minigameIndex]["sceneid"];
		SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);
	}

	public void ShowLose()
	{
		///REMOVE ACTIVE MINIGAME FROM SCREEN
		JToken sceneName = Missions[minigameIndex]["sceneid"];
		SceneManager.UnloadSceneAsync (sceneName.ToString());

		loseGame.SetActive (true);

		SkeletonGraphic graphic = loseGameSadChar.GetComponent<SkeletonGraphic> ();
		graphic.AnimationState.SetAnimation(0,"sad",false);

		continueLoseButton.GetComponent<Button> ().onClick.AddListener (PlayNextMiniGame);
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
		continueLoseButton.GetComponent<Button> ().onClick.RemoveAllListeners ();

		endGame.SetActive (false);
		loseGame.SetActive (false);

		if (minigameIndex + 1  < Missions.Count) {
			//Debug.Log ("play next minigame " + minigameIndex);

			minigameIndex++;


			JToken sceneName = Missions[minigameIndex]["sceneid"];
			SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);

		} else {
			//Debug.Log ("that was the last minigame, show map");
			SceneController.sceneController.FadeAndLoadScene ("Map", true);
		}
	}
}