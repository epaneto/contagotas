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

public class GroupManager : MonoBehaviour {

	bool hasGroup = false;

	//My User Info
	int UserId = 1;
	GroupData groupInfo;
	string urlBase = "http://localhost/contagotas/group/";

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

	[SerializeField]
	Text createFeedback;

	[Header("Join Group References")]
	[SerializeField]
	GameObject JoinGroupGameObjects;

	[Header("Ranking Group References")]
	[SerializeField]
	GameObject RankingGroupGameObjects;
	[SerializeField]
	Transform RankingGroupParent;
	[SerializeField]
	GameObject GroupObjectPrefabs;

	// Use this for initialization
	void Start () {
		StartCoroutine (HasGroup (UserId));
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

	public void ShowJoinObjects()
	{
		ShowGroup(ObjectsGroup.JOIN_GROUP);
	}

	public void ShowCreateObjects()
	{
		ShowGroup(ObjectsGroup.CREATE_GROUP);
	}

	public void ShowRankingScreen()
	{
		ShowGroup (ObjectsGroup.LOADING);
		StartCoroutine (LoadRanking ());
	}

	public void ShowErrorScreen(string error_message)
	{
		error_text.text = error_message;
		ShowGroup (ObjectsGroup.ERROR);
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
			List<GroupData> rankedGroups = JsonConvert.DeserializeObject<List<GroupData>>(www.text);

			foreach (var group in rankedGroups) {
				GameObject item = Instantiate (GroupObjectPrefabs, RankingGroupParent);
				item.GetComponent<GroupInfo> ().SetupGroupInfo (group.Name, group.Score);
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
			createFeedback.text = www.text;
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

		url = urlBase + "join/" + groupID + "/" + UserId+ "/";
		www = new WWW(url);
		yield return www;

		if (www.text.Contains ("sucess")) {
			SetGroupData (groupName, groupID);
			groupTitle.text = groupName;
			ShowGroup (ObjectsGroup.EXISTING_GROUP);
		} else {
			ShowErrorScreen ("Error joining group ->" + www.text);
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
