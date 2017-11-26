using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigamesController : MonoBehaviour {

	List<string> minigames;
	public int minigameIndex;
	GameObject endGame;
	GameObject scoreTxt;
	GameObject continueButton;

	// Use this for initialization
	void Start () {
		endGame = GameObject.Find ("EndGame");
		scoreTxt = GameObject.Find ("score_txt");
		continueButton = GameObject.Find ("bt_continuar");

		endGame.SetActive (false);

		minigames = new List<string> ();

		for (int i = 1; i <= MissionController.missionController.activeMission; i++) {
			minigames.Add ("game" + i);
		}

		minigameIndex = 0;

		SceneManager.LoadScene(minigames[minigameIndex], LoadSceneMode.Additive);
	}

	public void ShowResults(int score)
	{
		///REMOVE ACTIVE MINIGAME FROM SCREEN
		SceneManager.UnloadSceneAsync (minigames [minigameIndex]);

		///SHOW END GAME
		Text txt = scoreTxt.GetComponent<Text> ();
		txt.text = score.ToString ();
		endGame.SetActive(true);
		continueButton.GetComponent<Button> ().onClick.AddListener (PlayNextMiniGame);
	}

	public void PlayNextMiniGame()
	{
		continueButton.GetComponent<Button> ().onClick.RemoveAllListeners ();

		if (minigameIndex + 1  < minigames.Count) {
			//Debug.Log ("play next minigame " + minigameIndex);
			minigameIndex++;
			SceneManager.LoadScene (minigames [minigameIndex], LoadSceneMode.Additive);

			endGame.SetActive (false);
		} else {
			//Debug.Log ("that was the last minigame, show map");
			SceneController.sceneController.FadeAndLoadScene ("Map", true);
		}
	}
}