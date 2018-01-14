using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class PlayerRankingScoreScreenManager : PlayerRankingBaseAssetsGroupManager {

	[SerializeField]
	Text playerName;

	[SerializeField]
	Text playerScore;

	[SerializeField]
	Text[] PlayersName;
	[SerializeField]
	Text[] PlayersScore;


	public void GoToMapScreen()
	{
        GameSound.gameSound.PlaySFX("button");
        SceneController.sceneController.FadeAndLoadScene("Map", true);
		//UnityEngine.SceneManagement.SceneManager.LoadScene ("Map");
	}
	#region RANKING


	public void StartLoadRanking()
	{
		StartCoroutine (LoadUserRanking ());
	}

	IEnumerator LoadUserRanking()
	{
		WWW result;
		yield return result = WWWUtils.DoWebRequestWithSpecificURL("http://localhost/contagotas/user/score/check/" + PlayerPrefs.GetInt ("user_id").ToString() + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper ().Contains ("ERROR")) {
			screenManager.ShowErrorScreen ("error loading user score:" + result.text);
			yield break;
		} else {
			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

			List<PlayerData> player = JsonConvert.DeserializeObject<List<PlayerData>>(json);

			playerName.text = player[0].playerName;
			playerScore.text = player[0].playerPoints.ToString();

			UserData.userData.playerData.playerPoints = player[0].playerPoints;
			UserData.userData.Save();

			StartCoroutine (LoadRanking ());
		}
	}

	IEnumerator LoadRanking()
	{
		WWW result;
		yield return result = WWWUtils.DoWebRequestWithSpecificURL("http://localhost/contagotas/user/score/top10/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper ().Contains ("ERROR")) {
			screenManager.ShowErrorScreen ("error loading user ranking:" + result.text);
			yield break;
		} else {

			//clearing all texts
			for (int i = 0; i < 10; i++) {
				PlayersName [i].text = "";
				PlayersScore [i].text = "";
			}

			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

			List<PlayerData> rankedPlayers = JsonConvert.DeserializeObject<List<PlayerData>>(json);

			int index = 0;
			foreach (var playerData in rankedPlayers) {
				PlayersName [index].text = playerData.playerName;
				PlayersScore [index].text = playerData.playerPoints.ToString();
				index++;
			}
			screenManager.ShowScreen (PlayerRankingScreenType.PLAYER_RANKING_SCREEN);
		}
	}

	#endregion

}
