using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupManager : MonoBehaviour {

	bool hasGroup = false;

	public enum ObjectsGroup 
	{
		LOADING,
		EXISTING_GROUP,
		NEW_GROUP,
		CREATE_GROUP,
		JOIN_GROUP,
	}

	[Header("Loading Group References")]
	[SerializeField]
	GameObject LoadingGroupGameObjects;

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
	}

	public void ShowJoinObjects()
	{
		ShowGroup(ObjectsGroup.JOIN_GROUP);
	}

	public void ShowCreateObjects()
	{
		ShowGroup(ObjectsGroup.CREATE_GROUP);
	}

	public void CreateAndJoinGroup()
	{
		StartCoroutine (CreateGroup(createInput.text));
	}

	IEnumerator CreateGroup(string groupName)
	{
		string url = "http://localhost/contagotas/group/create/" + groupName + "/";
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

}
