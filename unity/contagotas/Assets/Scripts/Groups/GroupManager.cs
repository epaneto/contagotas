using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class GroupData
{
	public string Name;
	public int Id;
	public int Score;
}

public enum ObjectsGroup 
{
	LOADING,
	EXISTING_GROUP,
	NEW_GROUP,
	CREATE_GROUP,
	JOIN_GROUP,
	RANKING_GROUP,
	ERROR,
}

public class GroupManager : MonoBehaviour {

	bool hasGroup = false;

	//My User Info
	int UserId = 1;
	GroupData groupInfo;
	string urlBase = "http://localhost/contagotas/group/";

	[Header("Loading Group References")]
	[SerializeField]
	GameObject LoadingGroupGameObjects;

	[Header("Error Group References")]
	[SerializeField]
	GameObject ErrorGroupGameObjects;
	[SerializeField]
	Text error_text;

	[Header("Existing Group References")]
	[SerializeField]
	GameObject ExistingGroupGameObjects;
	
	[SerializeField]
	Text groupTitle;

	[Header("New Group Group References")]
	[SerializeField]
	GameObject NewGroupGameObjects;

	[Header("Create Group References")]
	[SerializeField]
	GameObject CreateGroupGameObjects;
	
	[SerializeField]
	InputField createInput;

	[Header("Join Group References")]
	[SerializeField]
	GameObject JoinGroupGameObjects;
	[SerializeField]
	Transform searchResultParent;
	[SerializeField]
	InputField groupToSearchInput;
	[SerializeField]
	Transform suggestedParent;
	private GameObject suggestedResultObj;

	[Header("Ranking Group References")]
	[SerializeField]
	GameObject RankingGroupGameObjects;
	[SerializeField]
	Transform RankingGroupParent;
	[SerializeField]
	GameObject GroupObjectPrefabs;
	private List<GameObject> groupInfoObjList = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		StartCoroutine (HasGroup (UserId));
	}

	public void CreateAndJoinGroup()
	{
		StartCoroutine (CreateGroup(createInput.text));
	}

	public void LeaveGroup()
	{
		StartCoroutine (LeaveGroup (UserId));
	}

	public void Retry()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Group");
	}


	public void ShowGroup(ObjectsGroup groupToShow)
	{
		LoadingGroupGameObjects.SetActive (groupToShow == ObjectsGroup.LOADING);
		ExistingGroupGameObjects.SetActive (groupToShow == ObjectsGroup.EXISTING_GROUP);
		NewGroupGameObjects.SetActive (groupToShow == ObjectsGroup.NEW_GROUP);
		CreateGroupGameObjects.SetActive(groupToShow == ObjectsGroup.CREATE_GROUP);
		JoinGroupGameObjects.SetActive(groupToShow == ObjectsGroup.JOIN_GROUP);
		ErrorGroupGameObjects.SetActive(groupToShow == ObjectsGroup.ERROR);
		RankingGroupGameObjects.SetActive (groupToShow == ObjectsGroup.RANKING_GROUP);
	}

	public void ShowCreateScreen()
	{
		ShowGroup(ObjectsGroup.CREATE_GROUP);
	}


	public void ShowErrorScreen(string error_message)
	{
		error_text.text = error_message;
		ShowGroup (ObjectsGroup.ERROR);
	}


	public void ShowRankingScreen()
	{
		ShowGroup (ObjectsGroup.LOADING);
		StartCoroutine (LoadRanking ());
	}

	public void ShowJoinScreen()
	{
		ShowGroup (ObjectsGroup.LOADING);
		StartCoroutine (LoadSuggestedGroups());
	}

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
		
		WWW result;
		yield return result = DoWebRequest("list_by_name/" + groupToSearchInput.text + "/");
		Debug.Log ("url result = " + result.text);


		if (result.text == "[]") {
			ShowErrorScreen ("Grupo nao encontrado");
			yield break;
		}

		if (result.text.ToUpper ().Contains ("ERROR") || result.text.ToUpper ().Contains ("TIMEOUT")) {
			ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		} 

		ShowGroup (ObjectsGroup.JOIN_GROUP);

		List<GroupData> foundGroups = JsonConvert.DeserializeObject<List<GroupData>>(result.text);

		suggestedResultObj = Instantiate (GroupObjectPrefabs, searchResultParent);
		GroupInfo groupInfoScript = suggestedResultObj.GetComponent<GroupInfo>(); 
		groupInfoScript.joinGroupClicked += HandleJoinClick;
		groupInfoScript.SetupGroupInfo (foundGroups[0].Name, foundGroups[0].Score, foundGroups[0].Id, false);

	}

	IEnumerator LoadSuggestedGroups()
	{
		WWW result;
		yield return result = DoWebRequest("list/");
		Debug.Log ("url result = " + result.text);


		if (result.text == "") {
			ShowErrorScreen ("error acessing group service, try again later");
			yield break;
		}

		if (result.text.ToUpper ().Contains ("ERROR")) {
			ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		} else {

			foreach (var item in groupInfoObjList) {
				Destroy (item);
			}

			List<GroupData> suggestedGroups = JsonConvert.DeserializeObject<List<GroupData>>(result.text);

			foreach (var group in suggestedGroups) {
				GameObject item = Instantiate (GroupObjectPrefabs, suggestedParent);
				groupInfoObjList.Add (item);
				GroupInfo groupInfoScript = item.GetComponent<GroupInfo>(); 
				groupInfoScript.joinGroupClicked += HandleJoinClick;
				groupInfoScript.SetupGroupInfo (group.Name, group.Score, group.Id, false);
			}

			ShowGroup (ObjectsGroup.JOIN_GROUP);	
		}
	}

	void HandleJoinClick(int groupId)
	{
		ShowGroup (ObjectsGroup.LOADING);
		StartCoroutine(Join (groupId));
	}


	public WWW DoWebRequest(string url)
	{
		string finalUrl = urlBase + url;
		ShowGroup (ObjectsGroup.LOADING);
		return new WWW(finalUrl);

	}

	IEnumerator LoadRanking()
	{
		string url = urlBase + "score/top/";
		WWW www = new WWW(url);
		ShowGroup (ObjectsGroup.LOADING);

		yield return www;
		Debug.Log ("url result = " + www.text);

		if (www.text.ToUpper ().Contains ("ERROR")) {
			ShowErrorScreen ("error leaving group:" + www.text);
			yield break;
		} else {

			foreach (var item in groupInfoObjList) {
				Destroy (item);
			}

			List<GroupData> rankedGroups = JsonConvert.DeserializeObject<List<GroupData>>(www.text);

			foreach (var group in rankedGroups) {
				GameObject item = Instantiate (GroupObjectPrefabs, RankingGroupParent);
				groupInfoObjList.Add (item);
				item.GetComponent<GroupInfo> ().SetupGroupInfo (group.Name, group.Score, group.Id);
			}

			ShowGroup (ObjectsGroup.RANKING_GROUP);	
		}
	}

	IEnumerator HasGroup(int userID)
	{
		string url = urlBase + "hasGroup/" + userID + "/";
		WWW www = new WWW(url);
		ShowGroup (ObjectsGroup.LOADING);

		yield return www;
		Debug.Log ("url result = " + www.text);

		if (www.text == "true") {
			url = urlBase + "list_by_user_id/" + userID + "/";
			www = new WWW (url);
			
			yield return www;
			Debug.Log ("url result = " + www.text);

			if (www.text.ToUpper ().Contains ("ERROR")) {
				ShowErrorScreen ("error leaving group:" + www.text);
				yield break;
			} else {
				List<GroupData> account = JsonConvert.DeserializeObject<List<GroupData>>(www.text);
				groupInfo = account[0];
				groupTitle.text = groupInfo.Name;
				ShowGroup (ObjectsGroup.EXISTING_GROUP);	
			}

		}
		else {
			ShowGroup (ObjectsGroup.NEW_GROUP);	
		}
	}

	IEnumerator CreateGroup(string groupName)
	{
		string url = urlBase + "create/" + groupName + "/";
		WWW www = new WWW(url);
		ShowGroup (ObjectsGroup.LOADING);

		yield return www;
		Debug.Log ("url result = " + www.text);

		if (www.text.ToUpper().Contains ("ERROR")) 
		{
			ShowErrorScreen ("error creating group:" + www.text);
			yield break;
		}

		int groupID;
		if(!int.TryParse(www.text, out groupID))
		{
			ShowErrorScreen ("error on group id:" + www.text);
			yield break;
		}

		StartCoroutine (Join (groupID));
	}

	IEnumerator Join(int groupID)
	{
		WWW result;
		yield return result = DoWebRequest ("join/" + groupID + "/" + UserId + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			ShowErrorScreen ("Error joining group ->" + result.text);
		} else {
			List<GroupData> account = JsonConvert.DeserializeObject<List<GroupData>>(result.text);
			groupInfo = account[0];
			groupTitle.text = groupInfo.Name;
			ShowGroup (ObjectsGroup.EXISTING_GROUP);	
		}
	}

	IEnumerator LeaveGroup(int userID)
	{
		string url = urlBase + "leave/" + userID + "/";
		WWW www = new WWW(url);
		ShowGroup (ObjectsGroup.LOADING);

		yield return www;
		Debug.Log ("url result = " + www.text);

		if (www.text.ToUpper().Contains("SUCESS")) 
		{
			ShowGroup (ObjectsGroup.NEW_GROUP);	
		}
		else {
			ShowErrorScreen ("error leaving group:" + www.text);
			yield break;
		}
	}

	public void SetGroupData(string groupName, int groupID)
	{
		groupInfo = new GroupData ();
		groupInfo.Name = groupName;
		groupInfo.Id = groupID;
	}

}
