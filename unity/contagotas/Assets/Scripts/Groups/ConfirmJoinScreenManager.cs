﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;

public class ConfirmJoinScreenManager : BaseAssetsGroupManager {

	[SerializeField]
	InputField ConfirmJoinPasswordInput;
	[SerializeField]
	Text tryingToJoinGroupName;
	[SerializeField]
	Text joinConfirmFeedback;

	private int temporaryGroupId;

	public void SetScreenInfo (int groupId, string groupName)
	{
		temporaryGroupId = groupId;
		tryingToJoinGroupName.text = groupName;
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
		sb.Append (PlayerPrefs.GetInt ("user_id"));
		sb.Append ("\"");
		sb.Append (",");
		sb.Append ("\"password\"");
		sb.Append (":\"");
		sb.Append (password);
		sb.Append ("\"");
		sb.Append ("}");

		WWW result;
		screenManager.ShowLoadingScreen ();
		yield return result = WWWUtils.DoWebRequest("join/",sb.ToString());

		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("ERROR") || result.text.ToUpper().Contains("TIMEOUT")) {
			screenManager.ShowErrorScreen ("Error joining group ->" + result.text);
		} else {
			string data = StringUtils.DecodeBytesForUTF8 (result.bytes);

			if (data == "wrong password") {
				ConfirmJoinPasswordInput.text = "";
				joinConfirmFeedback.text = "WRONG PASSWORD";
				screenManager.ShowGroup (ScreenType.CONFIRM_JOIN_GROUP);
				Debug.Log ("Wrong Password");
			}
			else
			{
				List<GroupData> account = JsonConvert.DeserializeObject<List<GroupData>>(data);
				GroupData groupData = account[0];
				screenManager.ShowExistingGroup (groupData);
			}
		}
	}

	public void Retry()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Group");
	}
}