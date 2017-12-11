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


	// Use this for initialization
	void Start () {

		if (Application.internetReachability == NetworkReachability.NotReachable) {
			Debug.Log ("Error. Check internet connection!");
		} else {
			StartCoroutine (HasGroup());
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

	IEnumerator HasGroup()
	{
		WWW result;
		yield return result = WWWUtils.DoWebRequest( "hasGroup/" + PlayerPrefs.GetInt ("user_id") + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text == "false") {
			StartCoroutine (CheckInvite());
		}
	}

}
