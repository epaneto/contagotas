using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesController : MonoBehaviour {

	List<string> minigames;
	int minigameIndex;
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
		if (minigameIndex < minigames.Count) {
			SceneManager.UnloadSceneAsync (minigames [minigameIndex]);

			minigameIndex++;
			SceneManager.LoadScene (minigames [minigameIndex], LoadSceneMode.Additive);
		}
	}
}