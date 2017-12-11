using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroupCheck : MonoBehaviour {

	[SerializeField]
	GameObject internetIssuesAssets;

	[SerializeField]
	Button groupButton;
	// Use this for initialization
	void Start () {
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			groupButton.enabled = false;
			internetIssuesAssets.SetActive (true);
		} else {
			groupButton.enabled = true;
			internetIssuesAssets.SetActive (false);
		}
	}

}
