using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;

public class JoinScreenManager : BaseAssetsGroupManager {

	[SerializeField]
	Transform searchResultParent;
	[SerializeField]
	InputField groupToSearchInput;
	[SerializeField]
	Transform suggestedParent;
	private GameObject suggestedResultObj;

	public void SearchGroup()
	{
		StartCoroutine(StartSearchGroup());
	}

	IEnumerator StartSearchGroup()
	{
		if (groupToSearchInput.text == "")
			yield break;

		if (suggestedResultObj != null)
			Destroy (suggestedResultObj);

		screenManager.ShowLoadingScreen ();
		WWW result;
		yield return result = WWWUtils.DoWebRequest("list_by_name/" + groupToSearchInput.text + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text == "[]") {
			screenManager.ShowErrorScreen ("Grupo nao encontrado");
			yield break;
		}

		if (result.text.ToUpper ().Contains ("ERROR") || result.text.ToUpper ().Contains ("TIMEOUT")) {
			screenManager.ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		} 

		screenManager.ShowGroup (ScreenType.JOIN_GROUP);

		string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

		List<GroupData> foundGroups = JsonConvert.DeserializeObject<List<GroupData>>(json);

		suggestedResultObj = Instantiate (screenManager.GroupObjectPrefabs, searchResultParent);
		GroupInfo groupInfoScript = suggestedResultObj.GetComponent<GroupInfo>(); 
		groupInfoScript.joinGroupClicked += HandleJoinClickWithPassword;
		groupInfoScript.SetupGroupInfo (foundGroups[0].Name, foundGroups[0].Score, foundGroups[0].Id, false);

	}

	public void StartLoadSuggestedGroups()
	{
		StartCoroutine (LoadSuggestedGroups ());
	}

	IEnumerator LoadSuggestedGroups()
	{
		screenManager.ShowLoadingScreen ();
		WWW result;
		yield return result = WWWUtils.DoWebRequest("list/");
		Debug.Log ("url result = " + result.text);


		if (result.text == "") {
			screenManager.ShowErrorScreen ("error acessing group service, try again later");
			yield break;
		}

		if (result.text.ToUpper ().Contains ("ERROR")) {
			screenManager.ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		} else {

			foreach (var item in screenManager.temporaryObjsList) {
				Destroy (item);
			}
			screenManager.temporaryObjsList.Clear ();

			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

			List<GroupData> suggestedGroups = JsonConvert.DeserializeObject<List<GroupData>>(json);

			foreach (var group in suggestedGroups) {
				GameObject item = Instantiate (screenManager.GroupObjectPrefabs, suggestedParent);
				screenManager.temporaryObjsList.Add (item);
				GroupInfo groupInfoScript = item.GetComponent<GroupInfo>(); 
				groupInfoScript.joinGroupClicked += HandleJoinClickWithPassword;
				groupInfoScript.SetupGroupInfo (group.Name, group.Score, group.Id, false);
			}

			screenManager.ShowGroup (ScreenType.JOIN_GROUP);	
		}
	}


	void HandleJoinClickWithPassword(int groupId, string groupName)
	{
		screenManager.ShowConfirmJoinGroupInviteScreen (groupId, groupName);
	}

	public IEnumerator Join(int groupID)
	{
		screenManager.ShowLoadingScreen ();
		WWW result;
		yield return result = WWWUtils.DoWebRequest ("join/" + groupID + "/" + PlayerPrefs.GetInt ("user_id") + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			screenManager.ShowErrorScreen ("Error joining group ->" + result.text);
		} else {
			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

			List<GroupData> account = JsonConvert.DeserializeObject<List<GroupData>>(json);
			GroupData groupData = account[0];
			screenManager.ShowExistingGroup (groupData);
		}
	}


}
