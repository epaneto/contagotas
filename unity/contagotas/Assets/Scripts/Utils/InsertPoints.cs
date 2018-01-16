using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertPoints : MonoBehaviour {

	// Use this for initialization
	void Start () {

		if (Application.internetReachability == NetworkReachability.NotReachable) {
			Debug.Log ("Error. Check internet connection!");
		} else {
			if (!PlayerPrefs.HasKey ("user_id")) {
				Debug.Log ("ERROR: Player não contem user id");
				return;
			}
			if (!PlayerPrefs.HasKey ("group_id")) {
				//player não tem grupo, nao precisa registrar score de grupo
				return;
			}

			StartCoroutine (AddScore(PlayerPrefs.GetInt ("group_id"), PlayerPrefs.GetInt ("user_id")));
		}

	}

	IEnumerator AddScore(int groupid, int score)
	{
		WWW result;
		yield return result = WWWUtils.DoWebRequest( "score/"+ groupid.ToString() + "/" + score.ToString() + "/" + PlayerPrefs.GetInt ("user_id") + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text == "success") 
			Debug.Log ("Inserido score");
		else
			Debug.Log ("ERROR:" + result.text);
	}

}
