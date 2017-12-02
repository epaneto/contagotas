using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class RankingScreenManager : BaseAssetsGroupManager {

	[SerializeField]
	Transform RankingGroupParent;

	public void StartLoadRanking()
	{
		StartCoroutine (LoadRanking ());
	}

	IEnumerator LoadRanking()
	{
		screenManager.ShowLoadingScreen ();
		WWW result;
		yield return result = WWWUtils.DoWebRequest("score/top/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper ().Contains ("ERROR")) {
			screenManager.ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		} else {

			foreach (var item in screenManager.temporaryObjsList) {
				Destroy (item);
			}
			screenManager.temporaryObjsList.Clear ();

			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

			List<GroupData> rankedGroups = JsonConvert.DeserializeObject<List<GroupData>>(json);

			foreach (var group in rankedGroups) {
				GameObject item = Instantiate (screenManager.GroupObjectPrefabs, RankingGroupParent);
				screenManager.temporaryObjsList.Add (item);
				item.GetComponent<GroupInfo> ().SetupGroupInfo (group.Name, group.Score, group.Id);
			}

			screenManager.ShowGroup (ScreenType.RANKING_GROUP);	
		}
	}

	public void Retry()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Group");
	}
}
