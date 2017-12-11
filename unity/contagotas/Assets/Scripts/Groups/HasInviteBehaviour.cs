using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class HasInviteBehaviour : MonoBehaviour {

	[SerializeField]
	Text notificationNumber;
	[SerializeField]
	GameObject notificationAssets;
	[SerializeField]
	GameObject internetIssuesAssets;

	// Use this for initialization
	void Start () {

		if (Application.internetReachability == NetworkReachability.NotReachable) {
			Debug.Log ("Error. Check internet connection!");
			internetIssuesAssets.SetActive (true);
		} else {
			internetIssuesAssets.SetActive (false);
			StartCoroutine (CheckInvite());
		}

	}

	public IEnumerator CheckInvite()
	{
		WWW result;
		yield return result = WWWUtils.DoWebRequest ("invite/check/" + PlayerPrefs.GetInt ("user_id") + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper ().Contains ("ERROR") || result.text.ToUpper ().Contains ("TIMEOUT")) {
			Debug.LogError ("error checking invite:" + result.text);
			yield break;
		} 

		string jsonDecoded  = StringUtils.DecodeBytesForUTF8 (result.bytes);

		List<InviteData> invites = JsonConvert.DeserializeObject<List<InviteData>>(jsonDecoded);

		if (invites.Count > 0) {
			notificationAssets.SetActive (true);
			notificationNumber.text = invites.Count.ToString ();
		} 
	}

}
