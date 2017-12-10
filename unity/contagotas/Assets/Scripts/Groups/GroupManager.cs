using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Facebook.Unity;
using Facebook.MiniJSON;
using System.Linq;
using System.Text;

public enum ScreenType 
{
	LOADING,
	EXISTING_GROUP,
	NEW_GROUP,
	CREATE_GROUP,
	JOIN_GROUP,
	CONFIRM_JOIN_GROUP,
	ERROR,
	RECEIVE_INVITES,
	SEND_INVITES
}

public class GroupManager : MonoBehaviour {

	GroupData groupInfo;

	[Header("General Prefabs And Utils Variables")]
	[SerializeField]
	public GameObject GroupObjectPrefabs;

	[HideInInspector]
	public List<GameObject> temporaryObjsList = new List<GameObject> ();

	[Header("Loading Screen References")]
	[SerializeField]
	BaseAssetsGroupManager loadingScreenManager;

	[Header("Error Screen References")]
	[SerializeField]
	ErrorScreenManager errorScreenManager;

	[Header("Existing Screen References")]
	[SerializeField]
	ExistingGroupScreenManager existingGroupScreenManager;

	[Header("New Group Screen References")]
	[SerializeField]
	NewGroupScreenManager newGroupScreenManager;

	[Header("Create Screen References")]
	[SerializeField]
	CreateNewGroupScreenManager createNewGroupScreenManager;

	[Header("Join Screen References")]
	[SerializeField]
	JoinScreenManager joinGroupScreenManager;

	[Header("Confirm Join Screen References")]
	[SerializeField]
	ConfirmJoinScreenManager confirmJoinScreenManager;

	[Header("Invite Screen References")]
	[SerializeField]
	InvitesReceivedScreenManager inviteReceivedScreenManager;

	[Header("Send Invite Screen References")]
	[SerializeField]
	SendInviteScreenManager sendInviteGroupScreenManager;


	// Use this for initialization
	void Start () {
		InitiliazeManagers ();
		CheckIfHasGroup ();
	}

	private void InitiliazeManagers()
	{
		loadingScreenManager.Initialize(this);
		errorScreenManager.Initialize(this);
		existingGroupScreenManager.Initialize(this);
		newGroupScreenManager.Initialize(this);
		createNewGroupScreenManager.Initialize(this);
		joinGroupScreenManager.Initialize(this);
		confirmJoinScreenManager.Initialize(this);
		inviteReceivedScreenManager.Initialize(this);
		sendInviteGroupScreenManager.Initialize(this);
	}

	public void CheckIfHasGroup()
	{
		StartCoroutine (HasGroup (PlayerPrefs.GetInt ("user_id")));
	}

	public void ShowGroup(ScreenType groupToShow)
	{
		loadingScreenManager.SetScreen (groupToShow == ScreenType.LOADING);
		errorScreenManager.SetScreen(groupToShow == ScreenType.ERROR);
		existingGroupScreenManager.SetScreen (groupToShow == ScreenType.EXISTING_GROUP);
		newGroupScreenManager.SetScreen (groupToShow == ScreenType.NEW_GROUP);
		createNewGroupScreenManager.SetScreen(groupToShow == ScreenType.CREATE_GROUP);
		joinGroupScreenManager.SetScreen(groupToShow == ScreenType.JOIN_GROUP);
		confirmJoinScreenManager.SetScreen(groupToShow == ScreenType.CONFIRM_JOIN_GROUP);
		inviteReceivedScreenManager.SetScreen (groupToShow == ScreenType.RECEIVE_INVITES);
		sendInviteGroupScreenManager.SetScreen (groupToShow == ScreenType.SEND_INVITES);
	}

	public void ShowLoadingScreen()
	{
		ShowGroup (ScreenType.LOADING);
	}

	public void ShowCreateScreen()
	{
		ShowGroup(ScreenType.CREATE_GROUP);
	}

	public void ShowErrorScreen(string error_message)
	{
		errorScreenManager.SetErrorMessage (error_message);
		ShowGroup (ScreenType.ERROR);
	}

	public void ShowJoinScreen()
	{
		ShowGroup (ScreenType.JOIN_GROUP);
		//joinGroupScreenManager.StartLoadSuggestedGroups ();
	}

	public void ShowSendInviteScreen()
	{
		ShowGroup (ScreenType.LOADING);
		sendInviteGroupScreenManager.LoadFriendsToInvite();
	}

	public void ShowExistingGroup(GroupData data)
	{
		existingGroupScreenManager.ShowExistingGroup (data);
	}

	public void ShowConfirmJoinGroupInviteScreen(int groupId,string groupName)
	{
		confirmJoinScreenManager.SetScreenInfo(groupId,	groupName);
		ShowGroup (ScreenType.CONFIRM_JOIN_GROUP);
	}

	IEnumerator HasGroup(int userID)
	{
		ShowLoadingScreen ();
		WWW result;
		yield return result = WWWUtils.DoWebRequest( "hasGroup/" + userID + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text == "true") {
			yield return result = WWWUtils.DoWebRequest( "list_by_user_id/" + userID + "/");
			Debug.Log ("url result = " + result.text);

			if (result.text.ToUpper ().Contains ("ERROR")) {
				ShowErrorScreen ("error checking group:" + result.text);
				yield break;
			} else {

				string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

				List<GroupData> account = JsonConvert.DeserializeObject<List<GroupData>>(json);
				groupInfo = account[0];
				existingGroupScreenManager.ShowExistingGroup (groupInfo);
			}

		}
		else {
			inviteReceivedScreenManager.StartSearchForInvites();
		}
	}

}
