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
	RANKING_GROUP,
	ERROR,
	RECEIVE_INVITES,
	SEND_INVITES
}

public class GroupManager : MonoBehaviour {

	[SerializeField]
	FacebookInviteManager facebookAPI;

	bool hasGroup = false;

	//My User Info
	int UserId = 1;
	GroupData groupInfo;
	string urlBase = "http://www.contagotas.online/services/group/";
	//string urlBase = "http://localhost/contagotas/group/";

	[Header("Loading Screen References")]
	[SerializeField]
	GameObject LoadingGroupGameObjects;

	[Header("Error Screen References")]
	[SerializeField]
	GameObject ErrorGroupGameObjects;
	[SerializeField]
	Text error_text;

	[Header("Existing Screen References")]
	[SerializeField]
	GameObject ExistingGroupGameObjects;
	
	[SerializeField]
	Text groupTitle;

	[Header("New Group Screen References")]
	[SerializeField]
	GameObject NewGroupGameObjects;

	[Header("Create Screen References")]
	[SerializeField]
	GameObject CreateGroupGameObjects;
	
	[SerializeField]
	InputField createInput;

	[SerializeField]
	InputField createPasswordInput;

	[Header("Join Screen References")]
	[SerializeField]
	GameObject JoinGroupGameObjects;
	[SerializeField]
	Transform searchResultParent;
	[SerializeField]
	InputField groupToSearchInput;
	[SerializeField]
	Transform suggestedParent;
	private GameObject suggestedResultObj;

	[Header("Confirm Join Screen References")]
	[SerializeField]
	GameObject ConfirmJoinGroupGameObjects;
	[SerializeField]
	InputField ConfirmJoinPasswordInput;
	[SerializeField]
	Text tryingToJoinGroupName;
	[SerializeField]
	Text joinConfirmFeedback;

	[Header("Ranking Screen References")]
	[SerializeField]
	GameObject RankingGroupGameObjects;
	[SerializeField]
	Transform RankingGroupParent;
	[SerializeField]
	GameObject GroupObjectPrefabs;
	private List<GameObject> temporaryObjsList = new List<GameObject> ();

	[Header("Invite Screen References")]
	[SerializeField]
	GameObject InviteGroupGameObjects;
	[SerializeField]
	Transform InviteGroupParent;
	[SerializeField]
	GameObject InvitePrefab;

	[Header("Send Invite Screen References")]
	[SerializeField]
	GameObject SendInviteGroupGameObjects;
	[SerializeField]
	Transform SendInviteGroupParent;
	[SerializeField]
	GameObject SendInvitePrefab;

	// Use this for initialization
	void Start () {
		UserId = PlayerPrefs.GetInt ("user_id");
		StartCoroutine (HasGroup (UserId));
	}

	public void CreateAndJoinGroup()
	{
		if (string.IsNullOrEmpty (createInput.text) || string.IsNullOrEmpty (createPasswordInput.text))
			return;
		
		StartCoroutine (CreateGroup(createInput.text,createPasswordInput.text));
	}

	public void LeaveGroup()
	{
		StartCoroutine (LeaveGroup (UserId));
	}

	public void Retry()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Group");
	}


	public void ShowGroup(ScreenType groupToShow)
	{
		LoadingGroupGameObjects.SetActive (groupToShow == ScreenType.LOADING);
		ExistingGroupGameObjects.SetActive (groupToShow == ScreenType.EXISTING_GROUP);
		NewGroupGameObjects.SetActive (groupToShow == ScreenType.NEW_GROUP);
		CreateGroupGameObjects.SetActive(groupToShow == ScreenType.CREATE_GROUP);
		JoinGroupGameObjects.SetActive(groupToShow == ScreenType.JOIN_GROUP);
		ConfirmJoinGroupGameObjects.SetActive(groupToShow == ScreenType.CONFIRM_JOIN_GROUP);
		ErrorGroupGameObjects.SetActive(groupToShow == ScreenType.ERROR);
		RankingGroupGameObjects.SetActive (groupToShow == ScreenType.RANKING_GROUP);
		InviteGroupGameObjects.SetActive (groupToShow == ScreenType.RECEIVE_INVITES);
		SendInviteGroupGameObjects.SetActive (groupToShow == ScreenType.SEND_INVITES);
	}

	public void ShowCreateScreen()
	{
		ShowGroup(ScreenType.CREATE_GROUP);
	}


	public void ShowErrorScreen(string error_message)
	{
		error_text.text = error_message;
		ShowGroup (ScreenType.ERROR);
	}

	public void ShowRankingScreen()
	{
		ShowGroup (ScreenType.LOADING);
		StartCoroutine (LoadRanking ());
	}

	public void ShowJoinScreen()
	{
		ShowGroup (ScreenType.LOADING);
		StartCoroutine (LoadSuggestedGroups());
	}

	public void SearchGroup()
	{
		StartCoroutine(StartSearchGroup());
	}

	public void ShowSendInviteScreen()
	{
		ShowGroup (ScreenType.LOADING);
		LoadFriendsToInvite();
	}


	int temporaryGroupId;
	public void ShowConfirmJoinGroupInviteScreen(int groupId)
	{
		ShowGroup (ScreenType.CONFIRM_JOIN_GROUP);
		temporaryGroupId = groupId;
	}

	public void LoadFriendsToInvite()
	{
		facebookAPI.GetFriends (ReceivedFriendList);
	}

	public void ReceivedFriendList(IResult result)
	{
		var dict = Json.Deserialize(result.RawResult)
			as Dictionary<string,object>;

		var friendList = new List<object>();
		friendList = (List<object>)(dict["data"]);

		foreach (var item in temporaryObjsList) {
			Destroy (item);
		}

		temporaryObjsList.Clear ();

		foreach (var friend in friendList) {

			var info = ((IEnumerable)friend).Cast<object> ()
				.Select (x => x.ToString ())
				.ToArray ();

			string name = info [1].Replace ('[', ' ').Replace (']', ' ').Trim ().Split (',') [1];
			string facebook_id = info [3].Replace ('[', ' ').Replace (']', ' ').Trim ().Split (',') [1];

			GameObject item = Instantiate (SendInvitePrefab, SendInviteGroupParent);
			temporaryObjsList.Add (item);
			FacebookInvite inviteObj = item.GetComponent<FacebookInvite> (); 
			inviteObj.SetupInviteInfo (name, facebook_id);
		}

		ShowGroup (ScreenType.SEND_INVITES);
	}

	IEnumerator SearchForInvites()
	{
		WWW result;
		yield return result = DoWebRequest ("invite/check/" + UserId + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper ().Contains ("ERROR") || result.text.ToUpper ().Contains ("TIMEOUT")) {
			ShowErrorScreen ("error checking invite:" + result.text);
			yield break;
		} 

		string jsonDecoded  = StringUtils.DecodeBytesForUTF8 (result.bytes);

		List<InviteData> invites = JsonConvert.DeserializeObject<List<InviteData>>(jsonDecoded);

		foreach (var obj in temporaryObjsList) {
			Destroy (obj);
		}
		temporaryObjsList.Clear ();

		if (invites.Count > 0) {
			
			foreach (var invite in invites) {
				GameObject item = Instantiate (InvitePrefab, InviteGroupParent);
				temporaryObjsList.Add (item);
				InviteObject inviteObj = item.GetComponent<InviteObject> (); 
				inviteObj.acceptButtonClicked += HandleAcceptInvite;
				inviteObj.declineButtonClicked += HandleDeclineInvite;
				inviteObj.SetupInviteInfo (invite.sender_name, invite.group_name, invite.id_invite);
			}

			ShowGroup (ScreenType.RECEIVE_INVITES);

		} else {
			ShowGroup (ScreenType.NEW_GROUP);
		}
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

		ShowGroup (ScreenType.JOIN_GROUP);

		string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

		List<GroupData> foundGroups = JsonConvert.DeserializeObject<List<GroupData>>(json);

		suggestedResultObj = Instantiate (GroupObjectPrefabs, searchResultParent);
		GroupInfo groupInfoScript = suggestedResultObj.GetComponent<GroupInfo>(); 
		groupInfoScript.joinGroupClicked += HandleJoinClickWithPassword;
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

			foreach (var item in temporaryObjsList) {
				Destroy (item);
			}
			temporaryObjsList.Clear ();

			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

			List<GroupData> suggestedGroups = JsonConvert.DeserializeObject<List<GroupData>>(json);

			foreach (var group in suggestedGroups) {
				GameObject item = Instantiate (GroupObjectPrefabs, suggestedParent);
				temporaryObjsList.Add (item);
				GroupInfo groupInfoScript = item.GetComponent<GroupInfo>(); 
				groupInfoScript.joinGroupClicked += HandleJoinClickWithPassword;
				groupInfoScript.SetupGroupInfo (group.Name, group.Score, group.Id, false);
			}

			ShowGroup (ScreenType.JOIN_GROUP);	
		}
	}

	void HandleJoinClickWithPassword(int groupId, string groupName)
	{
		tryingToJoinGroupName.text = groupName;
		ShowConfirmJoinGroupInviteScreen (groupId);
	}

	void HandleAcceptInvite(int inviteID)
	{
		StartCoroutine(AcceptInvite (inviteID));
	}

	void HandleDeclineInvite(int inviteID)
	{
		StartCoroutine(DeclineInvite (inviteID));
	}

	public WWW DoWebRequest(string url, string data = "data=")
	{
		Hashtable headers = new Hashtable ();
		headers.Add ("User-Agent", "app-contagotas");

		string finalUrl = urlBase + url;
		ShowGroup (ScreenType.LOADING);
		return new WWW(finalUrl, Encoding.UTF8.GetBytes(data), headers);
	}

	IEnumerator AcceptInvite (int inviteId)
	{
		WWW result;
		yield return result = DoWebRequest("invite/accept/" + inviteId + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			ShowErrorScreen ("Error accepting invite ->" + result.text);
		} else {
			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);
			List<GroupData> account = JsonConvert.DeserializeObject<List<GroupData>>(json);
			groupInfo = account[0];
			groupTitle.text = groupInfo.Name;
			ShowGroup (ScreenType.EXISTING_GROUP);	
		}
	}

	IEnumerator DeclineInvite (int inviteId)
	{
		WWW result;
		yield return result = DoWebRequest("invite/deny/" + inviteId+ "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			ShowErrorScreen ("Error declining invite ->" + result.text);
		} else {
			StartCoroutine (HasGroup (UserId));
		}
	}

	IEnumerator LoadRanking()
	{
		WWW result;
		yield return result = DoWebRequest("score/top/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper ().Contains ("ERROR")) {
			ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		} else {

			foreach (var item in temporaryObjsList) {
				Destroy (item);
			}
			temporaryObjsList.Clear ();

			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

			List<GroupData> rankedGroups = JsonConvert.DeserializeObject<List<GroupData>>(json);

			foreach (var group in rankedGroups) {
				GameObject item = Instantiate (GroupObjectPrefabs, RankingGroupParent);
				temporaryObjsList.Add (item);
				item.GetComponent<GroupInfo> ().SetupGroupInfo (group.Name, group.Score, group.Id);
			}

			ShowGroup (ScreenType.RANKING_GROUP);	
		}
	}

	IEnumerator HasGroup(int userID)
	{
		WWW result;
		yield return result = DoWebRequest( "hasGroup/" + userID + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text == "true") {
			yield return result = DoWebRequest( "list_by_user_id/" + userID + "/");
			Debug.Log ("url result = " + result.text);

			if (result.text.ToUpper ().Contains ("ERROR")) {
				ShowErrorScreen ("error checking group:" + result.text);
				yield break;
			} else {

				string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

				List<GroupData> account = JsonConvert.DeserializeObject<List<GroupData>>(json);
				groupInfo = account[0];
				groupTitle.text = groupInfo.Name + " Score = " + groupInfo.Score;
				ShowGroup (ScreenType.EXISTING_GROUP);	
			}

		}
		else {
			StartCoroutine (SearchForInvites ());
		}
	}

	IEnumerator CreateGroup(string groupName, string password)
	{
		StringBuilder sb = new StringBuilder ();
		sb.Append ("data={");
		sb.Append ("\"name\"");
		sb.Append (":\"");
		sb.Append (groupName);
		sb.Append ("\"");
		sb.Append (",");
		sb.Append ("\"password\"");
		sb.Append (":\"");
		sb.Append (password);
		sb.Append ("\"");
		sb.Append ("}");

		WWW result;
		yield return result = DoWebRequest("create/",sb.ToString());
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains ("ERROR")) 
		{
			ShowErrorScreen ("error creating group:" + result.text);
			yield break;
		}

		int groupID;
		if(!int.TryParse(result.text, out groupID))
		{
			ShowErrorScreen ("error on group id:" + result.text);
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
			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

			List<GroupData> account = JsonConvert.DeserializeObject<List<GroupData>>(json);
			groupInfo = account[0];
			groupTitle.text = groupInfo.Name;
			ShowGroup (ScreenType.EXISTING_GROUP);	
		}
	}

	public void ClickedJoinWithPassword()
	{
		joinConfirmFeedback.text = "";
		StartCoroutine(JoinWithPassword (temporaryGroupId, ConfirmJoinPasswordInput.text));
	}

	IEnumerator JoinWithPassword(int groupID, string password)
	{
		StringBuilder sb = new StringBuilder ();
		sb.Append ("data={");
		sb.Append ("\"groupid\"");
		sb.Append (":\"");
		sb.Append (groupID);
		sb.Append ("\"");
		sb.Append (",");
		sb.Append ("\"userid\"");
		sb.Append (":\"");
		sb.Append (UserId);
		sb.Append ("\"");
		sb.Append (",");
		sb.Append ("\"password\"");
		sb.Append (":\"");
		sb.Append (password);
		sb.Append ("\"");
		sb.Append ("}");

		WWW result;
		yield return result = DoWebRequest("join/",sb.ToString());

		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			ShowErrorScreen ("Error joining group ->" + result.text);
		} else {
			string data = StringUtils.DecodeBytesForUTF8 (result.bytes);

			if (data == "wrong password") {
				ConfirmJoinPasswordInput.text = "";
				joinConfirmFeedback.text = "WRONG PASSWORD";
				ShowGroup (ScreenType.CONFIRM_JOIN_GROUP);
				Debug.Log ("Wrong Password");
			}
			else
			{
				List<GroupData> account = JsonConvert.DeserializeObject<List<GroupData>>(data);
				groupInfo = account[0];
				groupTitle.text = groupInfo.Name;
				ShowGroup (ScreenType.EXISTING_GROUP);	
			}
		}
	}

	IEnumerator LeaveGroup(int userID)
	{
		WWW result;
		yield return result = DoWebRequest ("leave/" + userID + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("SUCCESS")) 
		{
			ShowGroup (ScreenType.NEW_GROUP);	
		}
		else {
			ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		}
	}

}
