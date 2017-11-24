using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct MyGroupInfo
{
	public string groupName;
	public int groupID;
}

public class GroupManager : MonoBehaviour {

	bool hasGroup = false;

	//My User Info
	int UserId = 1;
	MyGroupInfo groupInfo;
	string urlBase = "http://localhost/contagotas/group/";

	public enum ObjectsGroup 
	{
		LOADING,
		EXISTING_GROUP,
		NEW_GROUP,
		CREATE_GROUP,
		JOIN_GROUP,
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

	// Use this for initialization
	void Start () {
		if (hasGroup) {
			ShowGroup(ObjectsGroup.EXISTING_GROUP);
			groupTitle.text = "Group Name goes here";
		} else {
			ShowGroup(ObjectsGroup.NEW_GROUP);
		}
	}

	public void ShowGroup(ObjectsGroup groupToShow)
	{
		LoadingGroupGameObjects.SetActive (groupToShow == ObjectsGroup.LOADING);
		ExistingGroupGameObjects.SetActive (groupToShow == ObjectsGroup.EXISTING_GROUP);
		NewGroupGameObjects.SetActive (groupToShow == ObjectsGroup.NEW_GROUP);
		CreateGroupGameObjects.SetActive(groupToShow == ObjectsGroup.CREATE_GROUP);
		JoinGroupGameObjects.SetActive(groupToShow == ObjectsGroup.JOIN_GROUP);
		ErrorGroupGameObjects.SetActive(groupToShow == ObjectsGroup.ERROR);
	}

	public void ShowJoinObjects()
	{
		ShowGroup(ObjectsGroup.JOIN_GROUP);
	}

	public void ShowCreateObjects()
	{
		ShowGroup(ObjectsGroup.CREATE_GROUP);
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

	IEnumerator HasGroup(int userID)
	{
		string url = urlBase + "hasGroup/" + userID + "/";
		WWW www = new WWW(url);
		ShowGroup (ObjectsGroup.LOADING);

		yield return www;
		Debug.Log ("url result = " + www.text);

		if (string.IsNullOrEmpty(www.text))
			ShowGroup (ObjectsGroup.EXISTING_GROUP);
		else {
			ShowGroup (ObjectsGroup.CREATE_GROUP);	
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
			SetGroupInfo (groupName, groupID);
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

	public void SetGroupInfo(string groupName, int groupID)
	{
		groupInfo = new MyGroupInfo ();
		groupInfo.groupName = groupName;
		groupInfo.groupID = groupID;
	}


}
