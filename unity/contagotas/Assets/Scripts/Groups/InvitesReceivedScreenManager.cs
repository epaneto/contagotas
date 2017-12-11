using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class InvitesReceivedScreenManager : BaseAssetsGroupManager {

	[SerializeField]
	Transform InviteGroupParent;
	[SerializeField]
	GameObject InvitePrefab;

	public void StartSearchForInvites()
	{
		StartCoroutine (SearchForInvites ());
	}

	IEnumerator SearchForInvites()
	{
		screenManager.ShowLoadingScreen ();
		WWW result;
		yield return result = WWWUtils.DoWebRequest ("invite/check/" + PlayerPrefs.GetInt ("user_id") + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper ().Contains ("ERROR") || result.text.ToUpper ().Contains ("TIMEOUT")) {
			screenManager.ShowErrorScreen ("error checking invite:" + result.text);
			yield break;
		} 

		string jsonDecoded  = StringUtils.DecodeBytesForUTF8 (result.bytes);

		List<InviteData> invites = JsonConvert.DeserializeObject<List<InviteData>>(jsonDecoded);

		foreach (var obj in screenManager.temporaryObjsList) {
			Destroy (obj);
		}
		screenManager.temporaryObjsList.Clear ();

		if (invites.Count > 0) {

			foreach (var invite in invites) {
				GameObject item = Instantiate (InvitePrefab, InviteGroupParent);
				screenManager.temporaryObjsList.Add (item);
				InviteObject inviteObj = item.GetComponent<InviteObject> (); 
				inviteObj.acceptButtonClicked += HandleAcceptInvite;
				inviteObj.declineButtonClicked += HandleDeclineInvite;
				inviteObj.closeButtonClicked += HandleCloseButtonClicked;
				inviteObj.SetupInviteInfo (invite.sender_name, invite.group_name, invite.id_invite);
			}

			screenManager.ShowGroup (ScreenType.RECEIVE_INVITES);

		} else {
			screenManager.ShowGroup (ScreenType.NEW_GROUP);
		}
	}



	void HandleAcceptInvite(int inviteID)
	{
		StartCoroutine(AcceptInvite (inviteID));
	}

	void HandleDeclineInvite(int inviteID)
	{
		StartCoroutine(DeclineInvite (inviteID));
	}

	void HandleCloseButtonClicked(InviteObject obj)
	{
		screenManager.temporaryObjsList.Remove (obj.gameObject);
		Destroy (obj.gameObject);
		if (screenManager.temporaryObjsList.Count == 0)
			screenManager.ShowNewGroupScene();
	}


	IEnumerator AcceptInvite (int inviteId)
	{
		screenManager.ShowLoadingScreen ();
		WWW result;
		yield return result = WWWUtils.DoWebRequest("invite/accept/" + inviteId + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			screenManager.ShowErrorScreen ("Error accepting invite ->" + result.text);
		} else {
			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);
			List<GroupData> account = JsonConvert.DeserializeObject<List<GroupData>>(json);
			GroupData groupData = account[0];
			screenManager.ShowExistingGroup (groupData);
		}
	}

	IEnumerator DeclineInvite (int inviteId)
	{
		WWW result;
		yield return result = WWWUtils.DoWebRequest("invite/deny/" + inviteId+ "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			screenManager.ShowErrorScreen ("Error declining invite ->" + result.text);
		} else {
			screenManager.CheckIfHasGroup ();
		}
	}

	public void Retry()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Group");
	}
}
