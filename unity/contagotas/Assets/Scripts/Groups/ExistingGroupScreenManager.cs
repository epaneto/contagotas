using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExistingGroupScreenManager : BaseAssetsGroupManager {

	[SerializeField]
	Text groupName;

	[SerializeField]
	Text groupScore;

	public void StartLeaveGroup()
	{
		StartCoroutine (LeaveGroup (PlayerPrefs.GetInt ("user_id")));
	}

	IEnumerator LeaveGroup(int userID)
	{
		screenManager.ShowLoadingScreen ();
		WWW result;
		yield return result = WWWUtils.DoWebRequest ("leave/" + userID + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("SUCCESS")) 
		{
			screenManager.ShowGroup (ScreenType.NEW_GROUP);	
		}
		else {
			screenManager.ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		}
	}

	public void ShowExistingGroup(GroupData info)
	{
		groupName.text = info.Name;
		groupScore.text = info.Score.ToString();
		screenManager.ShowGroup (ScreenType.EXISTING_GROUP);
	}

	public void ShowRankingScreen()
	{
		screenManager.ShowRankingScreen ();
	}

	public void ShowSendInviteScreen()
	{
		screenManager.ShowSendInviteScreen ();
	}

	public void GoToHomeScreen()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Home");
	}

}
