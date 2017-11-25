using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesController : MonoBehaviour {

	List<string> minigames;
	public int minigameIndex;

	// Use this for initialization
	void Start () {

		minigames = new List<string> ();

		for (int i = 1; i <= MissionController.missionController.activeMission; i++) {
			minigames.Add ("game" + i);
		}

		minigameIndex = 0;

		SceneManager.LoadScene(minigames[minigameIndex], LoadSceneMode.Additive);
	}

	public void PlayNextMinigame()
	{
		Debug.Log ("play next minigame! active is " + minigameIndex);

		if (minigameIndex + 1  < minigames.Count) {
			SceneManager.UnloadSceneAsync (minigames [minigameIndex]);
			Debug.Log ("increase mini game index!");

			minigameIndex++;
			SceneManager.LoadScene (minigames [minigameIndex], LoadSceneMode.Additive);
		} else {
			Debug.Log ("last minigame, unload everything!");
			SceneManager.UnloadSceneAsync (minigames [minigameIndex]);
		}
	}
}