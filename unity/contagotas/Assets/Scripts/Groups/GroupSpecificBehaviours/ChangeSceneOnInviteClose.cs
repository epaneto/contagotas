using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneOnInviteClose : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (this.transform.childCount == 0)
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Map");
	}
}
