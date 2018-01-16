using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class FacebookInvite : MonoBehaviour {

	[SerializeField]
	Text facebookUserName;

	[SerializeField]
	Button inviteButton;

	[SerializeField]
	Image personImage;

	private string facebookCoverURL;

	private string userFacebookId;

	public System.Action<int> inviteButtonClicked = new System.Action<int>(delegate(int id) {});

	public void SetupInviteInfo(string facebookUserName, string facebookId, string facebookCoverURL)
	{
		this.facebookUserName.text = facebookUserName.Trim();
		this.userFacebookId = facebookId;
		this.facebookCoverURL = facebookCoverURL;
	
		inviteButton.onClick.AddListener(HandleAcceptClick);
	}

	public void Start()
	{
		StartCoroutine (LoadImage ());
	}

	IEnumerator LoadImage() {
		WWW www = new WWW(facebookCoverURL);
		yield return www;
		personImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	}

	private void HandleAcceptClick()
	{
		inviteButton.gameObject.SetActive (false);
		StartCoroutine(SendInvite());
	}

	IEnumerator SendInvite ()
	{
		Hashtable headers = new Hashtable ();
		headers.Add ("User-Agent", "app-contagotas");

		string finalURL = "http://contagotas.online/services/group/invite/create/" + PlayerPrefs.GetInt("user_id") + "/" + userFacebookId.Trim() + "/";
		WWW result = new WWW(finalURL, Encoding.UTF8.GetBytes("data="), headers);
		yield return result;

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			Debug.Log ("Error accepting invite ->" + result.text);
		} 
	}

}
