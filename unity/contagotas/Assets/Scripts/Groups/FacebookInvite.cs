using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacebookInvite : MonoBehaviour {

	[SerializeField]
	Text facebookUserName;

	[SerializeField]
	Button inviteButton;

	private string userFacebookId;

	public System.Action<int> inviteButtonClicked = new System.Action<int>(delegate(int id) {});

	public void SetupInviteInfo(string facebookUserName, string facebookId)
	{
		this.facebookUserName.text = facebookUserName;
		this.userFacebookId = facebookId;

		inviteButton.onClick.AddListener(HandleAcceptClick);
	}

	private void HandleAcceptClick()
	{
		inviteButton.gameObject.SetActive (false);
		StartCoroutine(SendInvite());
	}

	IEnumerator SendInvite ()
	{
		string finalURL = "http://localhost/contagotas/group/invite/create/" + PlayerPrefs.GetInt("user_id") + "/" + userFacebookId + "/";
		WWW result = new WWW(finalURL);
		yield return result;

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			Debug.Log ("Error accepting invite ->" + result.text);
		} 
	}


}
