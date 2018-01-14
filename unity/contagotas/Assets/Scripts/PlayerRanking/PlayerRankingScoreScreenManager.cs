using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class PlayerRankingScoreScreenManager : PlayerRankingBaseAssetsGroupManager {

	[SerializeField]
	Text groupName;

	[SerializeField]
	Text groupScore;

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
		StartCoroutine (LoadRanking ());
	}

	IEnumerator LoadRanking()
	{
		WWW result;
		yield return result = WWWUtils.DoWebRequest("score/top/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper ().Contains ("ERROR")) {
			screenManager.ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		} else {

			//clearing all texts
			for (int i = 0; i < 8; i++) {
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

		}
	}

	#endregion

}
